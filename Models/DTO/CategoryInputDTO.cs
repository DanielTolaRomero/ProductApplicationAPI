using System.ComponentModel.DataAnnotations;

namespace WebApplicationPractica.Models.DTO
{
    public class CategoryInputDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo categoria es requerido.")]
        [MinLength(3, ErrorMessage = "El campo debe tener al menos 3 caracteres.")]
        public string Category { get; set; } = string.Empty;
    }
}
