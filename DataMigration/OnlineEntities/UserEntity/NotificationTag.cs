using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.UserEntity
{
    public class NotificationTag
    {
        public int Id { get; set; }
        [Required] public bool IsRead { get; set; }
        [Required] public int NotificationId { get; set; }
        [Required] public int UserId { get; set; }
    }
}
