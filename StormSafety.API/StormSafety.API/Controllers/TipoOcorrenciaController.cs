using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StormSafety.API.Data;
using StormSafety.API.Models;

namespace StormSafety.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoOcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoOcorrenciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoOcorrencia>>> GetAll()
        {
            return await _context.TiposOcorrencias.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TipoOcorrencia>> Create(TipoOcorrencia tipo)
        {
            _context.TiposOcorrencias.Add(tipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = tipo.Id }, tipo);
        }
    }
}
