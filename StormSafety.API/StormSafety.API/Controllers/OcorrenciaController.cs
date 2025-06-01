using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.Models;
using StormSafety.API.Services;
using StormSafety.API.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Registro de ocorrências relacionadas a desastres climáticos.")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQService _rabbit;
        private readonly MLModelService _mlModel;

        public OcorrenciaController(AppDbContext context, RabbitMQService rabbit, MLModelService mlModel)
        {
            _context = context;
            _rabbit = rabbit;
            _mlModel = mlModel;
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
            Description = @"Exemplo:
{
  ""descricao"": ""Rua completamente alagada após forte chuva"",
  ""usuarioId"": 1,
  ""tipoOcorrenciaId"": 2
}")]
        public async Task<ActionResult<Ocorrencia>> Create([FromBody] OcorrenciaCreateDTO dto)
        {
            var novaOcorrencia = new Ocorrencia
            {
                Descricao = dto.Descricao,
                UsuarioId = dto.UsuarioId,
                TipoOcorrenciaId = dto.TipoOcorrenciaId,
                DataHora = DateTime.Now
            };

            _context.Ocorrencias.Add(novaOcorrencia);
            await _context.SaveChangesAsync();

            var json = JsonSerializer.Serialize(novaOcorrencia);
            _rabbit.Publish(json);

            return CreatedAtAction(nameof(GetById), new { id = novaOcorrencia.Id }, novaOcorrencia);
        }

        [HttpPost("prever")]
        [SwaggerOperation(
            Summary = "Prever tipo da ocorrência com base na descrição",
            Description = @"Exemplo:
{
  ""descricao"": ""Ficamos sem energia após a tempestade""
}")]
        public ActionResult PreverTipo([FromBody] DescricaoInput input)
        {
            var tipo = _mlModel.PreverTipo(input.Descricao);
            return Ok(new { TipoPrevisto = tipo });
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

        public class DescricaoInput
        {
            public string Descricao { get; set; }
        }
    }
}
