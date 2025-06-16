using Client.Data;
using Client.DTOs;
using Client.Entities.UserEntity;
using Client.Responses;
using ClientLibrary.Services.Constracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Implementations
{
    public class UserAccountService : IUserAccountInterface
    {
        private readonly AppDbContext appDbContext;

        public UserAccountService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user == null)
                return new GeneralResponse(false, "Yêu cầu dữ liệu người dùng");

            var checkUser = await FindUserByEmail(user.Name);
            if (checkUser != null)
                return new GeneralResponse(false, "Người dùng đã tồn tại");

            // Save user
            var applicationUser = await AddToDatabase(new User()
            {
                Name = user.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                IsActivated = true
            });

            // Check, create and assign role
            var checkAdminRole = appDbContext.SystemRoles
                .Where(_ => _.Name != null && _.Name.Equals(Client.Helpers.Constants.Admin))
                .FirstOrDefault();
            if (checkAdminRole == null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole() { Name = Client.Helpers.Constants.Admin });
                await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Tài khoản Admin khởi tạo thành công");
            }

            var checkUserRole = appDbContext.SystemRoles
                .Where(_ => _.Name != null && _.Name.Equals(Client.Helpers.Constants.User))
                .FirstOrDefault();
            SystemRole response = null;
            if (checkUserRole == null)
            {
                response = await AddToDatabase(new SystemRole() { Name = Client.Helpers.Constants.User });
                await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
            }
            return new GeneralResponse(true, applicationUser.Id.ToString());
        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if (user == null) return new LoginResponse(false, "Yêu cầu dữ liệu người dùng", 0);
            var applicationUser = await FindUserByEmail(user.Name);
            if (applicationUser == null) return new LoginResponse(false, "Tên đăng nhập hoặc mật khẩu không đúng", 0);

            //Verify password
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
                return new LoginResponse(false, "Tên đăng nhập hoặc mật khẩu không đúng", 0);

            var getUserRole = await FindUserRole(applicationUser.Id);
            if (getUserRole == null) return new LoginResponse(false, "Không tìm thấy User Role", 0);

            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getRoleName == null) return new LoginResponse(false, "Không tìm thấy User role", 0);

            // Check if user is activated
            if (!applicationUser.IsActivated)
                return new LoginResponse(false, "User chưa được kích hoạt", 0);

            return new LoginResponse(true, "Đăng nhập thành công", applicationUser.Id);
        }

        private Task<UserRole> FindUserRole(int userId)
        {
            // Synchronous query wrapped in Task.FromResult for compatibility
            var result = appDbContext.UserRoles.Where(_ => _.UserId == userId).FirstOrDefault();
            return Task.FromResult(result);
        }

        private Task<SystemRole> FindRoleName(int roleId)
        {
            var result = appDbContext.SystemRoles.Where(_ => _.Id == roleId).FirstOrDefault();
            return Task.FromResult(result);
        }

        private Task<User> FindUserByEmail(string email)
        {
            var result = appDbContext.Users
                .Where(_ => _.Name != null && _.Name.ToLower().Equals(email.ToLower()))
                .FirstOrDefault();
            return Task.FromResult(result);
        }

        private async Task<T> AddToDatabase<T>(T model) where T : class
        {
            var result = appDbContext.Set<T>().Add(model);
            await appDbContext.SaveChangesAsync();
            return result;
        }

        public Task<GeneralResponse> CheckActivated(int userId)
        {
            var user = appDbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.IsActivated })
                .FirstOrDefault();

            if (user == null || !user.IsActivated)
                return Task.FromResult(new GeneralResponse(false, "Ngươi dùng chưa được kích hoạt"));

            return Task.FromResult(new GeneralResponse(true, "Người dùng đã được kích hoạt"));
        }

        public async Task<GeneralResponse> ChangeActivateAccountStatus(int userId, bool isActivated)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user == null) return new GeneralResponse(false, "Không tìm thấy người dùng");
            if (user.IsActivated == isActivated) return new GeneralResponse(true, "Không có thay đổi nào");

            user.IsActivated = isActivated;
            await appDbContext.SaveChangesAsync();
            return new GeneralResponse(true, "Trạng thái người dùng được thay đổi thành công");
        }

        public async Task<GeneralResponse> UpdateUserAccountAsync(int userId, string newName, string newPassword)
        {
            using (var _context = new AppDbContext())
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                    return new GeneralResponse(false, "Không tìm thấy người dùng.");

                user.Name = newName;

                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    // Hash the new password with Bcrypt
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return new GeneralResponse(true, "Cập nhật thông tin thành công.");
                }
                catch (Exception ex)
                {
                    return new GeneralResponse(false, "Lỗi khi cập nhật thông tin: " + ex.Message);
                }
            }
        }
    }
}
