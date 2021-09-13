using Core.Entities.Concrete;

namespace WExporter.Entities.Concrete
{
    public class GroupRole : BaseEntity<int>
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}