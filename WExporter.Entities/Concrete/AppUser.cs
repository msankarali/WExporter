using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class AppUser : BaseStateEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}