namespace WebApplicationPractica.Models
{
    public class Category
    {
        public int id { get; set; }
        public string category { get; set; } = string.Empty;
        public ICollection<Product> products { get; set; } = new List<Product>();
        public bool active { get; set; } = true;
    }
}
