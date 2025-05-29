using System;

namespace StormSafety.API.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int TipoOcorrenciaId { get; set; }
        public TipoOcorrencia TipoOcorrencia { get; set; }
    }
}
