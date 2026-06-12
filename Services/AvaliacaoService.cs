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

    private static double CalcularNotaFinal(double historia, double emocao, double direcao, double trilha, double visual)
        {
            double calculo = (historia * 4 +
                              emocao   * 3 +
                              direcao  * 2 +
                              trilha   * 1 +
                              visual   * 1) / 11.0;

            return Math.Round(calculo, 2);
        }

   public void AdicionarAvaliacao(int usuarioId, AvaliacaoCreateDTO dto)
        {
            var avaliacaoExistente = _context.Avaliacoes
                .FirstOrDefault(a => a.UsuarioId == usuarioId && a.FilmeId == dto.FilmeId);

            if (avaliacaoExistente != null)
            {
                throw new ArgumentException("Você já avaliou este filme.");
            }

            double notaFinal = CalcularNotaFinal(
                dto.NotaHistoria,
                dto.NotaEmocao,
                dto.NotaDirecao,
                dto.NotaTrilha,
                dto.NotaVisual
            );

                var novaAvaliacao = new Avaliacao
                {
                    UsuarioId    = usuarioId,
                    FilmeId      = dto.FilmeId,
                    NotaHistoria = dto.NotaHistoria,
                    NotaEmocao   = dto.NotaEmocao,
                    NotaDirecao  = dto.NotaDirecao,
                    NotaTrilha   = dto.NotaTrilha,
                    NotaVisual   = dto.NotaVisual,
                    NotaFinal    = notaFinal,
                };
                _context.Avaliacoes.Add(novaAvaliacao);
                

            _context.SaveChanges();
        }

        public void AtualizarAvaliacao(int usuarioId, int filmeId, AvaliacaoUpdateDTO dto)
        {
            var avaliacao = _context.Avaliacoes
                .FirstOrDefault(a => a.UsuarioId == usuarioId && a.FilmeId == filmeId);

            if (avaliacao == null)
                throw new KeyNotFoundException("Avaliação não encontrada.");

            // Só atualiza os campos que foram enviados — mantém os demais
            if (dto.NotaHistoria.HasValue) avaliacao.NotaHistoria = dto.NotaHistoria.Value;
            if (dto.NotaEmocao.HasValue)   avaliacao.NotaEmocao   = dto.NotaEmocao.Value;
            if (dto.NotaDirecao.HasValue)  avaliacao.NotaDirecao  = dto.NotaDirecao.Value;
            if (dto.NotaTrilha.HasValue)   avaliacao.NotaTrilha   = dto.NotaTrilha.Value;
            if (dto.NotaVisual.HasValue)   avaliacao.NotaVisual   = dto.NotaVisual.Value;

            // Sempre recalcula a nota final com os valores atuais
            avaliacao.NotaFinal = CalcularNotaFinal(
                avaliacao.NotaHistoria,
                avaliacao.NotaEmocao,
                avaliacao.NotaDirecao,
                avaliacao.NotaTrilha,
                avaliacao.NotaVisual
            );

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