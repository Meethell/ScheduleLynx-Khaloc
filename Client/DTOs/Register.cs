
using CLient.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Client.DTOs
{
    public class Register : AccountBase
    {
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
