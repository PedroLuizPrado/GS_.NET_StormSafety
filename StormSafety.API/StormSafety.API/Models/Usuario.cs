using System.Collections.Generic;

namespace StormSafety.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Localizacao { get; set; }
        public ICollection<Ocorrencia> Ocorrencias { get; set; }
    }
}
