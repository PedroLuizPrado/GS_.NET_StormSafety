using Xunit;
using StormSafety.API.Controllers;
using StormSafety.API.Data;
using StormSafety.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StormSafety.Tests
{
    public class UsuarioControllerTests
    {
        private AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CriarUsuario_DeveRetornarCreatedAtAction()
        {
            var context = GetContext();
            var controller = new UsuarioController(context);

            var usuarioDto = new UsuarioCreateDTO
            {
                Nome = "Teste",
                Email = "teste@email.com",
                Localizacao = "São Paulo"
            };

            var resultado = await controller.Create(usuarioDto);

            Assert.IsType<CreatedAtActionResult>(resultado.Result);
        }
    }
}
