using System.ComponentModel.DataAnnotations;

namespace Acme.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Relacion con la tabla Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}