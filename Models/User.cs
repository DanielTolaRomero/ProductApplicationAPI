namespace WebApplicationPractica.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
