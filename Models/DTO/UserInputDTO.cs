using System.ComponentModel.DataAnnotations;

namespace WebApplicationPractica.Models.DTO
{
    public class UserInputDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MinLength(3,ErrorMessage = "La longitud minima de caracteres es 3")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La clave es requerida")]
        [MinLength(8,ErrorMessage = "La longitud de clave minima es 8")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El id del rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El rol id no puede ser negativo")]
        public int RoleId { get; set; }
    }
}
