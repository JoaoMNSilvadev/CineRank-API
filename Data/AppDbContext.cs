using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Models;
using Microsoft.EntityFrameworkCore;

namespace CineRank.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filme>()
                .HasOne(f => f.Diretor)
                .WithMany(p => p.FilmesDiretor)
                .HasForeignKey(f => f.DiretorId)
                .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Filme>()
                .HasMany(f => f.Atores)
                .WithMany(p => p.FilmesAtor)
                .UsingEntity(j => j.ToTable("FilmeAtores"));

            base.OnModelCreating(modelBuilder);

            // Configurações adicionais, como chaves compostas ou relacionamentos, podem ser feitas aqui
        }


    }
}