using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.UserEntity
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Required] public bool IsActivated { get; set; }

        // Many-to-one relationship
    }
}
