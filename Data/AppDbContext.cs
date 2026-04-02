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
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Credito> Creditos { get; set; }


    }
}