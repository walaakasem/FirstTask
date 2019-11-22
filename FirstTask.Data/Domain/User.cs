using FirstTask.Data.Enums;

namespace FirstTask.Data.Domain
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
