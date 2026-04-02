using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
public class AvaliacaoService
    {
    private readonly AppDbContext _context;

    public AvaliacaoService(AppDbContext context)
    {
        _context = context;
    }

    public void AdicionarOuAtualizarAvaliacao(AvaliacaoCreateDTO dto)
    {
       double calculo = (dto.NotaHistoria * 4 +
                         dto.NotaEmocao * 3 +
                         dto.NotaDirecao * 2 +
                         dto.NotaTrilha * 1 +
                         dto.NotaVisual * 1) / 11.0;

        double notaFinal = Math.Round(calculo, 2);

        // 2. Verificar se este usuário já avaliou este filme antes
        var avaliacaoExistente = _context.Avaliacoes
            .FirstOrDefault(a => a.UsuarioId == dto.UsuarioId && a.FilmeId == dto.FilmeId);

        if (avaliacaoExistente != null)
        {
            // ATUALIZAÇÃO
            avaliacaoExistente.NotaHistoria = dto.NotaHistoria;
            avaliacaoExistente.NotaEmocao = dto.NotaEmocao;
            avaliacaoExistente.NotaDirecao = dto.NotaDirecao;
            avaliacaoExistente.NotaTrilha = dto.NotaTrilha;
            avaliacaoExistente.NotaVisual = dto.NotaVisual;
            avaliacaoExistente.NotaFinal = notaFinal; // Média atualizada

        }
        else
        {
            // CRIAÇÃO NOVA
            var novaAvaliacao = new Avaliacao
            {
                UsuarioId = dto.UsuarioId,
                FilmeId = dto.FilmeId,
                NotaHistoria = dto.NotaHistoria,
                NotaEmocao = dto.NotaEmocao,
                NotaDirecao = dto.NotaDirecao,
                NotaTrilha = dto.NotaTrilha,
                NotaVisual = dto.NotaVisual,
                NotaFinal = notaFinal,
            };
            _context.Avaliacoes.Add(novaAvaliacao);
        }

        _context.SaveChanges();
        }

        public AvaliacaoSaidaDTO? ObterMinhaAvaliacao(int usuarioId, int filmeId)
        {
            var avaliacao = _context.Avaliacoes
                .FirstOrDefault(a => a.UsuarioId == usuarioId && a.FilmeId == filmeId);

            if (avaliacao == null)
                return null;

            return new AvaliacaoSaidaDTO
            {
                NotaHistoria = avaliacao.NotaHistoria,
                NotaEmocao = avaliacao.NotaEmocao,
                NotaDirecao = avaliacao.NotaDirecao,
                NotaTrilha = avaliacao.NotaTrilha,
                NotaVisual = avaliacao.NotaVisual,
                NotaFinal = avaliacao.NotaFinal,
                DataAvaliacao = avaliacao.DataAvaliacao
            };
        }

        public double ObterRankingFilme(int filmeId)
        {
            var notas = _context.Avaliacoes
            .Where(a => a.FilmeId == filmeId)
            .Select(a => a.NotaFinal);

            return notas.Any() ? Math.Round(notas.Average(), 1) : 0;
        }
    }
}