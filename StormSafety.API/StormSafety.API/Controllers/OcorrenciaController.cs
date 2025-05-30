using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Registro de ocorrências relacionadas a desastres climáticos.")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OcorrenciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as ocorrências registradas")]
        public async Task<ActionResult<IEnumerable<Ocorrencia>>> GetAll()
        {
            return await _context.Ocorrencias
                .Include(o => o.Usuario)
                .Include(o => o.TipoOcorrencia)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar ocorrência por ID")]
        public async Task<ActionResult<Ocorrencia>> GetById(int id)
        {
            var ocorrencia = await _context.Ocorrencias
                .Include(o => o.Usuario)
                .Include(o => o.TipoOcorrencia)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ocorrencia == null)
                return NotFound();

            return ocorrencia;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar nova ocorrência",
            Description = @"Exemplo de uso:
{
  ""descricao"": ""Rua completamente alagada após forte chuva"",
  ""usuarioId"": 1,
  ""tipoOcorrenciaId"": 1
}")]
        public async Task<ActionResult<Ocorrencia>> Create([FromBody] Ocorrencia ocorrencia)
        {
            ocorrencia.DataHora = DateTime.Now;

            _context.Ocorrencias.Add(ocorrencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ocorrencia.Id }, ocorrencia);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir ocorrência por ID")]
        public async Task<IActionResult> Delete(int id)
        {
            var ocorrencia = await _context.Ocorrencias.FindAsync(id);
            if (ocorrencia == null)
                return NotFound();

            _context.Ocorrencias.Remove(ocorrencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
