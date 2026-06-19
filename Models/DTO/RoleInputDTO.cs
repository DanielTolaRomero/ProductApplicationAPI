using System.ComponentModel.DataAnnotations;

namespace WebApplicationPractica.Models.DTO
{
    public class RoleInputDTO
    {
        [Required(ErrorMessage = "Role name is required.")]
        [MinLength(3, ErrorMessage = "Role name must be at least 3 characters long.")]
        public string RoleName { get; set; }
    }
}
