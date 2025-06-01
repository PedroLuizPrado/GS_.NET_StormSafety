namespace StormSafety.API.DTOs
{
   
    public class OcorrenciaCreateDTO
    {
        public string Descricao { get; set; }

        public int UsuarioId { get; set; }

        
        public int TipoOcorrenciaId { get; set; }
    }
}
