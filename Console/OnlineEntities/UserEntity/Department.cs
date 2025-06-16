using System.Collections.Generic;

namespace BaseLibrary.Entities.UserEntity
{
    public class Department : BaseEntity
    {
        // One-to-many relationship
        public List<UserPublicDetail> UserPublicDetails { get; set; }
    }
}
