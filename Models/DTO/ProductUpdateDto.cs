using System.ComponentModel.DataAnnotations;

namespace WebApplicationPractica.Models.DTO
{
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "El id del producto es obligatorio")]
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre del producto debe tener al menos 3 caracteres")]
        public string name { get; set; }

        [Required(ErrorMessage = "La categoria es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo mayor a 0.")]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser un número positivo mayor a 0.")]
        public decimal price { get; set; } 
    }
}
