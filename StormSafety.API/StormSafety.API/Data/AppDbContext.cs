using Microsoft.EntityFrameworkCore;
using StormSafety.API.Models;

namespace StormSafety.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<TipoOcorrencia> TiposOcorrencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nomes personalizados das tabelas
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("StormDatabase_Usuario");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Localizacao).HasMaxLength(200);
            });

            modelBuilder.Entity<TipoOcorrencia>(entity =>
            {
                entity.ToTable("StormDatabase_TipoOcorrencia");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.NomeTipo).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Ocorrencia>(entity =>
            {
                entity.ToTable("StormDatabase_Ocorrencia");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Descricao).IsRequired().HasMaxLength(500);
                entity.Property(o => o.DataHora).IsRequired();

                entity.HasOne(o => o.Usuario)
                      .WithMany(u => u.Ocorrencias)
                      .HasForeignKey(o => o.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(o => o.TipoOcorrencia)
                      .WithMany(t => t.Ocorrencias)
                      .HasForeignKey(o => o.TipoOcorrenciaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
