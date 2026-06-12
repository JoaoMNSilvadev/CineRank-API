# Análise Técnica Completa do Projeto CineRank

## Resumo Executivo

O CineRank é uma API REST em ASP.NET Core para catálogo, ranking e gestão de filmes, com autenticação JWT, Entity Framework Core e CRUDs de domínio. A base existe, mas o estado atual é de MVP/protótipo técnico, não de produto pronto para produção.

O principal problema não é falta de funcionalidade. O problema central é falta de controle de acesso, consistência de modelagem e endurecimento de segurança.

O sistema está preparado para ser consumido por um frontend como API JSON. Ele não contém frontend próprio.

## 1. Entendimento Do Projeto

O objetivo aparente é permitir que usuários consultem filmes, pessoas, gêneros e plataformas, criem conta, autentiquem-se com JWT, avaliem filmes e, em tese, favoritem itens. Há um perfil administrativo para cadastrar e manter o catálogo.

Funcionalidades claramente existentes no código:

- Registro de usuário
- Login JWT
- Consulta pública de filmes, pessoas, gêneros e plataformas
- CRUD de filmes
- CRUD de pessoas
- CRUD de gêneros
- CRUD de plataformas
- Avaliação de filmes
- Gestão de créditos
- Alteração de role

Funcionalidades incompletas ou quebradas:

- Favoritos, porque existe serviço e entidade, mas o controller não é um controller MVC funcional
- Falta lista/consulta para créditos
- Falta update para funções
- A regra de cálculo de nota do README não bate com o código

Público-alvo provável:

- Usuários que consomem catálogo e ranking de filmes
- Administradores que fazem curadoria do acervo

Maturidade atual:

- Abaixo de produção
- Eu classificaria como protótipo de portfolio / projeto acadêmico avançado, com partes de MVP funcional

API ou frontend:

- É apenas API
- Está razoavelmente pronta para ser consumida por frontend, mas não contém frontend próprio

## 2. Mapeamento Das Funcionalidades

| Funcionalidade | Implementada | Completa | Observações |
| --- | --- | --- | --- |
| Registro de usuário | Sim | Parcial | Cria usuário com senha hash, mas sem verificação de e-mail e sem constraint única no banco. |
| Login JWT | Sim | Parcial | Gera token, mas sem issuer, audience, refresh token ou revogação. |
| Roles | Sim | Parcial | Claim de role existe e há endpoints Admin-only, mas não há revogação de token após troca de role. |
| CRUD de Filmes | Sim | Parcial | Tem create/list/search/update/delete/ranking, mas faltam validações fortes. |
| Avaliações | Sim | Parcial | Funciona, mas permite manipulação por IDs informados pelo cliente e não impede duplicidade. |
| Favoritos | Parcial | Não | O controller não é controller MVC e não há evidência de migração para a tabela. |
| Pessoas | Sim | Parcial | CRUD + busca + paginação. |
| Gêneros | Sim | Sim | CRUD simples, admin-only para escrita. |
| Plataformas | Sim | Sim | CRUD simples, admin-only para escrita. |
| Funções | Sim | Parcial | Tem list/create/delete, mas não update e usa DbContext direto. |
| Créditos | Sim | Parcial | Adiciona/remove, mas não lista e valida pouco. |
| Ranking | Sim | Parcial | Existe, mas a regra de peso está inconsistente com o README. |
| Swagger | Sim | Parcial | Só no ambiente Development. |
| Logs estruturados | Não | Não | Não encontrei ILogger nem pipeline de observabilidade. |
| Uploads | Não | Não | Nenhuma evidência de IFormFile ou storage. |
| Integrações externas | Não | Não | Nenhuma evidência de TMDB, SMTP, OAuth ou cloud storage. |
| Testes automatizados | Não | Não | Não encontrei arquivos de teste. |

## 3. Revisão Da Arquitetura

A estrutura geral é simples e legível: Controllers, Services, DTOs, Models, Data, Middleware e Migrations. Isso é suficiente para uma API pequena, mas há inconsistência de aplicação do padrão.

Pontos positivos:

- Separação entre DTOs e Models
- Uso de bcrypt para senha
- JWT e autorização por role
- Paginação em listas principais
- Migrations e relacionamentos no EF Core

Pontos fracos:

- Não existe camada de repositório nem contratos
- Não existe boundary claro entre aplicação e persistência
- Há mistura de responsabilidades entre controllers e services
- O controller de favoritos não é funcional

Violações de SOLID e Clean Architecture:

- SRP: o serviço de filmes faz consulta, projeção, cálculo, persistência e montagem de DTO
- DIP: há dependência direta de AppDbContext em controllers e services concretos
- Clean Architecture: a camada de apresentação fala direto com EF em vários pontos

## 4. Revisão Do Backend

Problemas principais encontrados:

| Gravidade | Arquivo | Classe | Método | Impacto | Solução recomendada |
| --- | --- | --- | --- | --- | --- |
| Crítica | [appsettings.json](appsettings.json), [Program.cs](Program.cs) | Configuração de JWT | Uso da chave JWT | Segredo hardcoded. Se o repositório vazar, qualquer token pode ser forjado. | Mover segredo para variável de ambiente ou Secret Manager e rotacionar a chave. |
| Crítica | [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs) | UsuarioController | BuscarUsuarioPorId, AtualizarUsuario, DeletarUsuario, TrocarSenha | IDOR / Broken Access Control. Qualquer usuário autenticado pode operar qualquer conta apenas mudando o ID. | Derivar o usuário do claim do token e validar ownership. |
| Alta | [Controllers/AvaliacaoController.cs](Controllers/AvaliacaoController.cs) | AvaliacaoController | AtualizarAvaliacao, ObterAvaliacao | Também há IDOR: o cliente escolhe usuarioId e filmeId. | Ignorar usuarioId vindo da rota e usar o usuário autenticado. |
| Alta | [Services/UsuarioService.cs](Services/UsuarioService.cs) | UsuarioService | AtualizarUsuario | E-mail pode ser duplicado no update. | Criar índice único no banco e validar duplicidade também em update. |
| Alta | [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs) | AvaliacaoService | AdicionarAvaliacao, AtualizarAvaliacao | Não impede múltiplas avaliações do mesmo usuário para o mesmo filme. | Criar constraint única em UsuarioId + FilmeId. |
| Alta | [Services/AuthService.cs](Services/AuthService.cs) e [Services/UsuarioService.cs](Services/UsuarioService.cs) | AuthService / UsuarioService | Login, AlterarRole, TrocarSenha | Tokens continuam válidos após troca de senha ou role. | Incluir token version, expiração curta e revogação. |
| Alta | [Controllers/FavoritoController.cs](Controllers/FavoritoController.cs) e [Services/FavoritoService.cs](Services/FavoritoService.cs) | FavoritoController / FavoritoService | Todos | A funcionalidade de favoritos está incompleta. | Transformar em controller real, registrar service e criar migração. |
| Média | [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs) | ExceptionMiddleware | HandleExceptionAsync | Responde com exception.Message, vazando detalhes internos e sem logging. | Logar exceções e devolver mensagem genérica ao cliente. |
| Média | [Program.cs](Program.cs) | CORS | AllowAnyOrigin | Política aberta demais para produção. | Restringir origens conhecidas. |
| Média | [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs), [README.md](README.md) | AvaliacaoService / documentação | CalcularNotaFinal | O README diz um peso e o código usa outro. | Alinhar regra de negócio e criar teste de regressão. |

Para SQL Injection, não encontrei uso de SQL cru. O uso é majoritariamente LINQ do EF Core. O problema dominante é autorização e integridade.

## 5. Banco De Dados

Pontos fortes:

- Relacionamentos existem entre Filme, Gênero, Avaliação, Usuário, Crédito, Pessoa, Função e Plataforma
- Há tabela de junção para many-to-many entre Filmes e Plataformas
- Há índices nos campos de FK nas migrations

Problemas:

- Não há índice único em Usuarios.Email
- Não há índice único composto em Avaliacoes por UsuarioId + FilmeId
- Não há índice único composto em Favoritos por UsuarioId + FilmeId
- Não há configuração explícita de constraints no AppDbContext
- Não encontrei migration para Favoritos

Possíveis gargalos:

- Buscas com Contains podem virar varredura ampla
- Listagens com Include e sem AsNoTracking escalam mal
- O ranking depende de agregação em tempo de consulta

## 6. Segurança

Auditoria objetiva:

- JWT: implementado, mas com chave hardcoded e sem rotação. Gravidade crítica.
- Controle de acesso: há autorização por role, mas existe IDOR grave. Gravidade crítica.
- Segredos expostos: sim, a chave JWT está no repositório. Gravidade crítica.
- SQL Injection: não encontrei evidência. Risco baixo.
- Mass Assignment: não encontrei binding direto de entidades públicas para escrita, mas alguns endpoints aceitam IDs sem validação contextual. Risco médio.
- Broken Access Control: sim. Gravidade crítica.
- IDOR: sim. Gravidade crítica.
- Exposição de dados sensíveis: o middleware devolve mensagem da exceção. Gravidade média.
- CORS: aberto para qualquer origem. Gravidade média.
- Revogação de sessão: não existe. Gravidade alta.

## 7. Integrações Externas

Não encontrei TMDB, SMTP, OAuth, storage em nuvem, filas, webhooks nem outra integração externa. O projeto é autocontido.

## 8. Qualidade Do Código

O que está bom:

- Uso de DTOs
- Paginação em listas importantes
- Uso de bcrypt para senha
- Separação básica entre controllers e services em parte do sistema

O que está ruim:

- Arquitetura inconsistente
- Lógica de negócio e persistência misturadas
- Falta de logging
- Falta de testes
- Falta de contratos e interfaces
- Endpoint de favoritos não é endpoint real

## 9. Testes

Não encontrei arquivos de teste.

Riscos diretos da ausência de testes:

- Ajustes em autenticação, regra de média, atualização de usuário ou modelagem de banco podem quebrar comportamento sem alerta
- Não há cobertura para regressões de segurança

## 10. Avaliação Profissional

| Área | Nota | Justificativa |
| --- | --- | --- |
| Arquitetura | 4/10 | Há estrutura básica, mas a aplicação é inconsistente e mistura camadas. |
| Backend | 5/10 | Há funcionalidade real, mas os problemas de acesso e integridade são graves. |
| Segurança | 2/10 | Chave hardcoded, IDOR, tokens sem revogação e CORS aberto derrubam a nota. |
| Banco de Dados | 5/10 | Relacionamentos existem, mas faltam constraints únicas e o schema está incompleto. |
| Organização | 5/10 | Pastas existem e a ideia é compreensível, mas há drift entre módulos. |
| Escalabilidade | 4/10 | Paginação ajuda, mas consultas e modelagem não foram pensadas para volume maior. |
| Manutenibilidade | 4/10 | Inconsistência arquitetural e falta de testes tornam manutenção arriscada. |
| Qualidade de Código | 5/10 | Há uso de DTOs e validações pontuais, mas também há muito acoplamento. |

## 11. Nível Provável Do Desenvolvedor

Classificação provável: Júnior Avançado.

Motivos:

- Há familiaridade com ASP.NET Core, EF Core, JWT, bcrypt, DTOs, paginação e roles
- Há noção de separação por camadas e algumas boas práticas
- Mas faltam fundamentos mais maduros de segurança, consistência arquitetural e modelagem
- Um pleno ou sênior normalmente não deixaria passar IDOR, ausência de constraints únicas e falta de testes

## 12. Roadmap De Evolução

| Prioridade | Tarefa | Dificuldade | Impacto |
| --- | --- | --- | --- |
| Crítico | Remover segredo JWT do repositório e rotacionar a chave | Média | Muito alto |
| Crítico | Corrigir IDOR em usuários e avaliações usando claims do token | Média | Muito alto |
| Crítico | Criar constraints únicas para e-mail, avaliações e favoritos | Média | Muito alto |
| Crítico | Implementar revogação/expiração adequada de sessão | Alta | Muito alto |
| Crítico | Transformar favoritos em endpoint real e criar migração da tabela | Média | Alto |
| Importante | Criar testes unitários e de integração para auth, usuário e ranking | Média | Alto |
| Importante | Padronizar camada de serviço para todos os controllers | Média | Alto |
| Importante | Adicionar logging e observabilidade | Média | Alto |
| Importante | Validar existência de FKs em criação de filme, crédito e avaliação | Baixa | Alto |
| Importante | Ajustar a fórmula de nota para bater com a regra documentada | Baixa | Alto |
| Opcional | Adicionar refresh token e política de sessão | Alta | Médio |
| Opcional | Otimizar consultas com AsNoTracking e melhor paginação | Baixa | Médio |
| Opcional | Integrar TMDB ou outra fonte externa de catálogo | Alta | Médio |

## 13. O Que Falta Para Virar Um Produto Real

Segurança:

- Remover segredo do código
- Corrigir IDOR
- Revogar tokens
- Restringir CORS
- Revisar middleware de erro

Arquitetura:

- Padronizar a camada de aplicação
- Criar interfaces e contratos
- Separar regras de negócio da persistência

Banco:

- Constraints únicas
- Índices de busca
- Limites de tamanho
- Migrations alinhadas com o modelo atual

Performance:

- AsNoTracking
- Paginação em buscas
- Revisão de Includes
- Possível cache para ranking

Testes:

- Cobertura para autenticação, autorização, avaliações, ranking e CRUDs críticos

DevOps:

- Pipeline de build
- Migração controlada
- Secrets management
- Ambientes separados

Observabilidade:

- Logs estruturados
- Correlation ID
- Métricas e alertas

## 14. Comparação Com O Mercado

- Projeto de faculdade: acima da média de um CRUD básico
- Projeto de portfólio: se encaixa aqui hoje
- Projeto de estágio: pode passar como base técnica, mas ainda com falhas sérias
- Projeto júnior: ainda não
- Produto comercial: não está perto

## 15. Veredito Final

1. Estágio atual do projeto: MVP funcional, com sinais de projeto acadêmico/portfolio.
2. O backend está realmente concluído? Não.
3. O projeto está pronto para um frontend profissional? Como API de consumo, sim parcialmente; como produto, não.
4. O sistema suportaria usuários reais? Não com segurança suficiente.
5. Os 10 problemas mais graves: chave JWT hardcoded, IDOR em usuário, IDOR em avaliação, tokens sem revogação, favoritos incompletos, falta de constraints únicas, divergência da regra de nota, middleware vazando erro, CORS aberto demais, ausência total de testes.
6. Minha decisão como Tech Lead: não aprovaria para produção.
7. Aprovação:
   - Portfólio: sim
   - Processo seletivo de estágio: sim, com críticas fortes
   - Processo seletivo júnior: não como código pronto
   - Produção: não

## Observação Final

Este relatório foi derivado apenas do código disponível no workspace. Não houve uso de documentação funcional externa nem suposições sem evidência no código.