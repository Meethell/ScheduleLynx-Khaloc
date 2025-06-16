using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.UserEntity
{
    public class UserPublicDetail
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public byte[] ProfilePicture { get; set; }

        [Required] public int UserId { get; set; }
        // Many-to-one relationship
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
