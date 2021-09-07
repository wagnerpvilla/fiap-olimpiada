using System;

namespace OlimpiadaAPI
{
    public class Artigo
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public string Categoria { get; set; }
        public string Url { get; set; }
        public DateTime PublicadoEm { get; set; }
        public string Autor { get; set; }
        public string Pais { get; set; }
        public string Esporte { get; set; }
        public string Modalidade { get; set; }
        public string[] Tags { get; set; }
    }
}
