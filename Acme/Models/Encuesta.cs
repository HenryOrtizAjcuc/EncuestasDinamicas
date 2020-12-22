using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.Models
{
    public class Encuesta
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Relacion con la tabla campo
        public ICollection<Campo> Campos { get; set; }
    }
}