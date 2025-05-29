using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.Models;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OcorrenciaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ocorrencia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ocorrencia>>> GetAll()
        {
            return await _context.Ocorrencias
                .Include(o => o.Usuario)
                .Include(o => o.TipoOcorrencia)
                .ToListAsync();
        }

        // GET: api/Ocorrencia/5
        [HttpGet("{id}")]
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

        // POST: api/Ocorrencia
        [HttpPost]
        public async Task<ActionResult<Ocorrencia>> Create(Ocorrencia ocorrencia)
        {
            ocorrencia.DataHora = DateTime.Now; // seta data atual

            _context.Ocorrencias.Add(ocorrencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ocorrencia.Id }, ocorrencia);
        }

        // PUT: api/Ocorrencia/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Ocorrencia ocorrencia)
        {
            if (id != ocorrencia.Id)
                return BadRequest();

            _context.Entry(ocorrencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ocorrencias.Any(o => o.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Ocorrencia/5
        [HttpDelete("{id}")]
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
