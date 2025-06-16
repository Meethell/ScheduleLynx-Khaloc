using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.UserEntity
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
