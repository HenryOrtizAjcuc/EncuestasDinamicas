﻿using System.ComponentModel.DataAnnotations;

namespace Acme.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Relacion con la tabla Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}