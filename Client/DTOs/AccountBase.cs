using System.ComponentModel.DataAnnotations;

namespace CLient.DTOs
{
    public class AccountBase
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
