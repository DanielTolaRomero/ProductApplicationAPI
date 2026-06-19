namespace WebApplicationPractica.Models.DTO
{
    public class ProductOutputDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
