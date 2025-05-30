using StormSafety.API.Data;
using StormSafety.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace StormSafety.Tests
{
    public class TipoOcorrenciaTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDB_Tipo")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void CriarTipoOcorrencia_DeveSalvarNoBanco()
        {
            var context = GetDbContext();
            var tipo = new TipoOcorrencia { NomeTipo = "Enchente" };

            context.TiposOcorrencias.Add(tipo);
            context.SaveChanges();

            Assert.Single(context.TiposOcorrencias);
            Assert.Equal("Enchente", context.TiposOcorrencias.First().NomeTipo);
        }
    }
}
