using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class FuncaoService
    {
        private readonly AppDbContext _context;

        public FuncaoService(AppDbContext context)
        {
            _context = context;
        }

        public List<Funcao> Listar()
        {
            return _context.Funcoes.ToList();
        }

        public Funcao Criar(FuncaoCreateDTO dto)
        {
            var novaFuncao = new Funcao { Nome = dto.Nome };
            _context.Funcoes.Add(novaFuncao);
            _context.SaveChanges();
            return novaFuncao;
        }

        public Funcao? Deletar(int id)
        {
            var funcao = _context.Funcoes.Find(id);
            if (funcao == null)
            {
                return null;
            }

            _context.Funcoes.Remove(funcao);
            _context.SaveChanges();
            return funcao;
        }
    }
}