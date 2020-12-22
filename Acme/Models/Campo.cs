namespace Acme.Models
{
    public class Campo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public bool EsRequerido { get; set; }
        public TipoCampo TipoCampo { get; set; }

        // Relacion con la tabla Encuesta
        public int EncuestaId { get; set; }
        public Encuesta encuesta { get; set; }
    }
}