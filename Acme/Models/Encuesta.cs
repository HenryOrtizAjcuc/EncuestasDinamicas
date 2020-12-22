using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.Models
{
    public class Encuesta
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Campo> Campos { get; set; }
    }
}