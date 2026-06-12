# 🎬 CineRank API

O **CineRank** é uma API desenvolvida em **ASP.NET Core** para o gerenciamento e ranking de filmes. O projeto calcula a classificação com uma **Média Ponderada** aplicada no servidor com base em critérios técnicos e emocionais.

## 🚀 Funcionalidades

* **CRUD Completo:** Gerenciamento de Filmes, Pessoas (Atores e Diretores), Gêneros, Funções, Plataformas e Créditos.
* **Sistema de Favoritos:** Usuários autenticados podem favoritar e remover filmes da própria lista.
* **Cálculo de Média Ponderada:** Lógica automática de notas aplicada no serviço de avaliação.
* **Relacionamentos Muitos-para-Muitos:** Vínculos dinâmicos entre Filmes, Atores e Plataformas de streaming.
* **Sistema de Ranking:** Endpoint dedicado para listar filmes do melhor para o pior.
* **Busca Inteligente:** Filtro de filmes por parte do título.

## ⚖️ Regras de Negócio (Pesos das Notas)

A `NotaFinal` é calculada no servidor no fluxo de avaliações seguindo a seguinte ponderação:

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
