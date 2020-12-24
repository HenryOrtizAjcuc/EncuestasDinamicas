using System.ComponentModel.DataAnnotations;

namespace Acme.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Relacion con la tabla Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}