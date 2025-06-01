using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.DTOs;
using StormSafety.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Cadastro dos tipos de ocorrência disponíveis.")]
    public class TipoOcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoOcorrenciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os tipos de ocorrência")]
        public async Task<ActionResult<IEnumerable<TipoOcorrencia>>> GetAll()
        {
            return await _context.TiposOcorrencias.ToListAsync();
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo tipo de ocorrência",
            Description = @"Exemplo de uso:
{
  ""nomeTipo"": ""Alagamento""
}")]
        public async Task<ActionResult<TipoOcorrencia>> Create([FromBody] TipoOcorrenciaCreateDTO dto)
        {
            var tipo = new TipoOcorrencia
            {
                NomeTipo = dto.NomeTipo
            };

            _context.TiposOcorrencias.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = tipo.Id }, tipo);
        }
    }
}
