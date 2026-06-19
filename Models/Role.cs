namespace WebApplicationPractica.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<User> Users { get; set; }
    }
}
