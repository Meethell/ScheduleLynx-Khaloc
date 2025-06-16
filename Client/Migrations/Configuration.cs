using Client.Entities.TaskEntities;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using Client.Entities.UserEntity;
using Client.Entities.TicketEntities;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Client.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Client.Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Client.Data.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // Thêm dữ liệu ban đầu cho bảng Countries
            context.Countries.AddOrUpdate(
                c => c.Name,
                new Country { Name = "Thụy Sỹ" },
                new Country { Name = "Ý" },
                new Country { Name = "Trung Quốc" },
                new Country { Name = "Ai Cập" },
                new Country { Name = "Đức" },
                new Country { Name = "Japan" },
                new Country { Name = "Hàn Quốc" },
                new Country { Name = "Pháp" },
                new Country { Name = "Mỹ" },
                new Country { Name = "Đài Loan" },
                new Country { Name = "Hà Lan" },
                new Country { Name = "Tây Ban Nha" },
                new Country { Name = "Việt Nam" }
            );
            

            // Thêm dữ liệu ban đầu cho bảng TicketStatuses
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Status,
                new TicketStatus { Status = "Mới", Color = "Yellow" },
                new TicketStatus { Status = "Đang xử lý", Color = "Orange" },
                new TicketStatus { Status = "Đã đóng", Color = "DarkGray" },
                new TicketStatus { Status = "Đã hủy", Color = "Red" }
            );

            // Thêm dữ liệu ban đầu cho bảng TicketTypes
            context.TicketTypes.AddOrUpdate(
                tt => tt.Type,
                new TicketType { Type = "Sửa chữa", Color = "Red" },
                new TicketType { Type = "Bảo trì", Color = "OrangeRed" },
                new TicketType { Type = "Bảo hành", Color = "DodgerBlue" },
                new TicketType { Type = "Lắp đặt mới", Color = "Gold" } 
            );

            // Thêm dữ liệu ban đầu cho bảng TicketServiceTypes
            context.TicketServiceTypes.AddOrUpdate(
                tst => tst.Type,
                new TicketServiceType { Type = "Xử lý tại khách hàng", Color = "Yellow" },
                new TicketServiceType { Type = "Sửa chữa tại kho", Color = "LawnGreen" },
                new TicketServiceType { Type = "Xử lý online", Color = "DodgerBlue" }
            );

            // Thêm dữ liệu ban đầu cho bảng Priorities
            context.TicketPriorities.AddOrUpdate(
                p => p.Level,
                new TicketPriority { Name = "Không quan trọng - Không khẩn cấp", Color = "DodgerBlue", Level = 1 },
                new TicketPriority { Name = "Không quan trọng - Khẩn cấp", Color = "Orange", Level = 2 },
                new TicketPriority { Name = "Quan trọng - Không khẩn cấp", Color = "OrangeRed", Level = 3 },
                new TicketPriority { Name = "Quan trọng - Khẩn cấp", Color = "Red", Level = 4 }
            );

            // Thêm dữ liệu ban đầu cho bảng Priorities
            context.TaskPriorities.AddOrUpdate(
                p => p.Level,
                new TaskPriority { Level = 1, Name = "Not Important - Not Urgent", Color = "DodgerBlue" },
                new TaskPriority { Level = 2, Name = "Not Important - Urgent", Color = "Orange" },
                new TaskPriority { Level = 3, Name = "Important - Not Urgent", Color = "OrangeRed" },
                new TaskPriority { Level = 4, Name = "Important - Urgent", Color = "Red" }
            );

            // Thêm dữ liệu ban đầu cho bảng InstrumentStatuses
            context.InstrumentStatuses.AddOrUpdate(
                is_ => is_.Status,
                new InstrumentStatus { Status = "Tốt", Color = "LawnGreen" },
                new InstrumentStatus { Status = "Cần bảo trì", Color = "OrangeRed" },
                new InstrumentStatus { Status = "Cần sửa chữa", Color = "Red" },
                new InstrumentStatus { Status = "Hết sử dụng", Color = "DarkGray" }
            );

            // Thêm dữ liệu ban đầu cho bảng InstrumentTypes
            context.ModelTypes.AddOrUpdate(
                it => it.Type,
                new ModelType { Type = "Máy phân tích Huyết học", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Sinh hóa", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Miễn dịch", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Nước tiểu", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Khí máu", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Đông máu", IsConsumable = false },
                new ModelType { Type = "Máy phân tích Di truyền", IsConsumable = false },
                new ModelType { Type = "Máy xét nghiệm HbA1c", IsConsumable = false },
                new ModelType { Type = "Máy siêu âm", IsConsumable = false },
                new ModelType { Type = "Máy X quang", IsConsumable = false }
            );

            // Thêm dữ liệu ban đầu cho bảng TaskStatuses
            context.TaskStatuses.AddOrUpdate(
                ts => ts.Status,
                new AppTaskStatus { Status = "New", Color = "Yellow" },
                new AppTaskStatus { Status = "In Progress", Color = "OrangeRed" },
                new AppTaskStatus { Status = "Done", Color = "LawnGreen" },
                new AppTaskStatus { Status = "Cancelled", Color = "Red" }
            );

            // Thêm dữ liệu ban đầu cho bảng TaskTypes
            context.TaskTypes.AddOrUpdate(
                tt => tt.Type,
                new TaskType { Type = "Công việc văn phòng", Color = "LightSkyBlue" },
                new TaskType { Type = "Học tập - Đào tạo", Color = "PowderBlue" },
                new TaskType { Type = "Liên hệ khách hàng", Color = "Cyan" },
                new TaskType { Type = "Công tác", Color = "DeepSkyBlue" }
            );


            // Lưu các thay đổi vào cơ sở dữ liệu

            if (!context.Dealers.Any(d => d.Id == 1))
            {
                context.Dealers.AddOrUpdate(new Dealer
                {
                    Id = 1,
                    Name = "FUJIMED",
                    Address = "A002 – A Khu Thương Mại Tầng Trệt, Cao ốc An Bình, 787 Lũy Bán Bích, P. Phú Thọ Hòa, Q. Tân Phú, TP. HCM"
                });
            }
            context.SaveChanges();
        }
    }
}
