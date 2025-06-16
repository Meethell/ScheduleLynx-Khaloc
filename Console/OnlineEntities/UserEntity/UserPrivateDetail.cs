using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.UserEntity
{
    public class UserPrivateDetail
    {
        public int Id { get; set; }
        [Required] public int UserId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PrivateEmail { get; set; } = string.Empty;
    }
}
