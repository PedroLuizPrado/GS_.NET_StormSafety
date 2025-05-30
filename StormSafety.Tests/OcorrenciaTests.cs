using StormSafety.API.Data;
using StormSafety.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;

namespace StormSafety.Tests
{
    public class OcorrenciaTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDB_Ocorrencia")
            .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void CriarOcorrencia_DeveSalvarComRelacionamentos()
        {
            var context = GetDbContext();

            var usuario = new Usuario { Nome = "Maria", Email = "maria@email.com", Localizacao = "RJ" };
            var tipo = new TipoOcorrencia { NomeTipo = "Deslizamento" };

            context.Usuarios.Add(usuario);
            context.TiposOcorrencias.Add(tipo);
            context.SaveChanges();

            var ocorrencia = new Ocorrencia
            {
                Descricao = "Morro caiu após chuva",
                UsuarioId = usuario.Id,
                TipoOcorrenciaId = tipo.Id,
                DataHora = DateTime.Now
            };

            context.Ocorrencias.Add(ocorrencia);
            context.SaveChanges();

            var resultado = context.Ocorrencias.Include(o => o.Usuario).Include(o => o.TipoOcorrencia).First();

            Assert.Equal("Deslizamento", resultado.TipoOcorrencia.NomeTipo);
            Assert.Equal("Maria", resultado.Usuario.Nome);
        }
    }
}
