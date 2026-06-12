using CineRank.Data;
using CineRank.Models;

namespace CineRank.Services
{
    public class CreditoService
    {
        private readonly AppDbContext _context;

        public CreditoService(AppDbContext context)
        {
            _context = context;
        }

        public void AdicionarCredito(int filmeId, int pessoaId, int funcaoId)
        {
            var filmeExiste = _context.Filmes.Any(f => f.Id == filmeId);
            if (!filmeExiste)
            {
                throw new KeyNotFoundException("Filme não encontrado.");
            }

            var pessoaExiste = _context.Pessoas.Any(p => p.Id == pessoaId);
            if (!pessoaExiste)
            {
                throw new KeyNotFoundException("Pessoa não encontrada.");
            }

            var funcaoExiste = _context.Funcoes.Any(f => f.Id == funcaoId);
            if (!funcaoExiste)
            {
                throw new KeyNotFoundException("Função não encontrada.");
            }

            var existe = _context.Creditos.Any(c =>
                c.FilmeId == filmeId && c.PessoaId == pessoaId && c.FuncaoId == funcaoId);

            if (existe)
            {
                throw new ArgumentException("Este crédito já foi cadastrado.");
            }

            var credito = new Credito
            {
                FilmeId = filmeId,
                PessoaId = pessoaId,
                FuncaoId = funcaoId
            };

            _context.Creditos.Add(credito);
            _context.SaveChanges();
        }

        public void RemoverCredito(int id)
        {
            var credito = _context.Creditos.Find(id);
            if (credito == null)
            {
                throw new KeyNotFoundException("Crédito não encontrado.");
            }

            _context.Creditos.Remove(credito);
            _context.SaveChanges();
        }
    }
}