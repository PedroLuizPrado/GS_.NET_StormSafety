using System.Collections.Generic;

namespace StormSafety.API.Models
{
    public class TipoOcorrencia
    {
        public int Id { get; set; }
        public string NomeTipo { get; set; }
        public ICollection<Ocorrencia> Ocorrencias { get; set; }
    }
}
