using StormSafety.API.Data;
using StormSafety.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace StormSafety.Tests
{
    public class UsuarioTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDB_Usuario")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void CriarUsuario_DeveSalvarNoBanco()
        {
            var context = GetDbContext();
            var usuario = new Usuario
            {
                Nome = "Pedro",
                Email = "pedro@email.com",
                Localizacao = "SP"
            };

            context.Usuarios.Add(usuario);
            context.SaveChanges();

            Assert.Single(context.Usuarios);
            Assert.Equal("Pedro", context.Usuarios.First().Nome);
        }
    }
}
