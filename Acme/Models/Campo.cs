namespace Acme.Models
{
    public class Campo
    {
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public bool EsRequerido { get; set; }
        public TipoCampo TipoCampo { get; set; }
    }
}