using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplicationPractica.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string  Name { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; } = null;
        public int CategoryId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;
    }
}
