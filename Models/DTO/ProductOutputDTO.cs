namespace WebApplicationPractica.Models.DTO
{
    public class ProductOutputDTO
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string category { get; set; }
        public decimal price { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
