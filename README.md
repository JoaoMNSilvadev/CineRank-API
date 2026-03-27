# 🎬 CineRank API

O **CineRank** é uma API robusta desenvolvida em **ASP.NET Core** para o gerenciamento e ranking de filmes. O projeto utiliza uma lógica de **Média Ponderada** para classificar filmes com base em critérios técnicos e emocionais.

## 🚀 Funcionalidades

* **CRUD Completo:** Gerenciamento de Filmes, Pessoas (Atores e Diretores), Gêneros e Plataformas.
* **Cálculo de Média Ponderada:** Lógica automática de notas embutida na entidade `Filme`.
* **Relacionamentos Muitos-para-Muitos:** Vínculos dinâmicos entre Filmes, Atores e Plataformas de streaming.
* **Sistema de Ranking:** Endpoint dedicado para listar filmes do melhor para o pior.
* **Busca Inteligente:** Filtro de filmes por parte do título.

## ⚖️ Regras de Negócio (Pesos das Notas)

A `NotaFinal` é calculada no servidor seguindo a seguinte ponderação:

| Critério | Peso |
| :--- | :--- |
| **História** | 4 |
| **Emoção** | 3 |
| **Direção** | 1 |
| **Trilha Sonora** | 1 |
| **Visual** | 1 |

## 🛠️ Tecnologias Utilizadas

* **C# / .NET 8.0/9.0**
* **Entity Framework Core** (SQL Server)
* **Swagger/OpenAPI** (Documentação)
* **System.Text.Json** (Serialização com IgnoreCycles)
