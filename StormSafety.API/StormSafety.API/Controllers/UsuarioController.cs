using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gerenciamento de usuários que registram ocorrências.")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os usuários")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar usuário por ID com HATEOAS")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var response = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.Localizacao,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id }), method = "GET" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id }), method = "DELETE" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id }), method = "PUT" }
                }
            };

            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo usuário",
            Description = @"Exemplo de uso:
{
  ""nome"": ""Pedro Luiz"",
  ""email"": ""pedro@email.com"",
  ""localizacao"": ""São Paulo, SP""
}")]
        [SwaggerResponse(201, "Usuário criado com sucesso")]
        public async Task<ActionResult<Usuario>> Create([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar dados do usuário")]
        public async Task<IActionResult> Update(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir usuário por ID")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
