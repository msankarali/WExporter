using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class AppUser : BaseStateEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}