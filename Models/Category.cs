namespace WebApplicationPractica.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public bool Active { get; set; } = true;
    }
}
