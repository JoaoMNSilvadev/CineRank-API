# Roadmap Técnico do CineRank

Documento derivado exclusivamente da análise do código-fonte disponível no workspace. O objetivo é transformar os achados técnicos em um plano de implementação, sem gerar código e sem alterar o sistema neste momento.

## Parte 1 — O que precisa ser feito para deixar o CineRank sólido como projeto de portfólio

Nesta seção entram apenas os itens que aumentam de forma clara a percepção de qualidade técnica, organização, maturidade e capacidade de execução do projeto em contexto de apresentação, entrevista ou avaliação técnica.

### 1. Segredo JWT fora do repositório

## Descrição
O projeto mantém a chave JWT em arquivo de configuração versionado. Isso expõe um segredo sensível e reduz a confiança no projeto, mesmo em contexto de portfólio.

## Evidências Encontradas
- [appsettings.json](appsettings.json)
- [Program.cs](Program.cs)
- [Services/AuthService.cs](Services/AuthService.cs)

## Impacto Atual
O token é gerado com uma chave facilmente recuperável do repositório. Isso não afeta só produção; também derruba a percepção de maturidade técnica da PoC.

## Impacto na PoC/Portfólio
Prejudica fortemente. Em avaliação técnica, segredo hardcoded é um dos sinais mais rápidos de baixa maturidade em segurança.

## Impacto em Produção
Permite forja de tokens se a chave vazar. O risco é crítico.

## Como Implementar
1. Remover a chave do arquivo versionado.
2. Mover o valor para variável de ambiente ou secret store local de desenvolvimento.
3. Definir estratégia de rotação da chave.
4. Validar que o bootstrap da aplicação falha de forma controlada quando a chave não estiver presente.
5. Revisar documentação de ambiente para explicar como configurar o segredo.

## Arquivos Envolvidos
- [appsettings.json](appsettings.json)
- [appsettings.Development.json](appsettings.Development.json)
- [Program.cs](Program.cs)
- [README.md](README.md)

## Nível de Dificuldade
Fácil

## Tempo Estimado
2 a 4 horas

## Prioridade para Portfólio/PoC
Obrigatório

## Prioridade para Produto Final
Crítica

---

### 2. Correção de IDOR em usuários

## Descrição
O backend permite consultar, atualizar, trocar senha e excluir usuários usando um `id` informado pelo cliente, sem garantir que o usuário autenticado é o dono do recurso.

## Evidências Encontradas
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)
- [Services/AuthService.cs](Services/AuthService.cs)

## Impacto Atual
Qualquer usuário autenticado pode tentar operar outra conta apenas alterando o parâmetro da rota. Isso invalida a confiança do modelo de segurança.

## Impacto na PoC/Portfólio
Reduz a nota técnica porque expõe uma falha clássica de autorização, especialmente em APIs REST.

## Impacto em Produção
Permite acesso, alteração e exclusão indevida de contas de terceiros. O impacto é crítico.

## Como Implementar
1. Parar de usar o `id` de rota como fonte única de verdade para operações do próprio usuário.
2. Ler o identificador do usuário autenticado a partir do token.
3. Validar ownership antes de qualquer leitura ou escrita.
4. Separar claramente ações de admin de ações de autoatendimento.
5. Criar cenários de teste para tentativa de acesso cruzado.

## Arquivos Envolvidos
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)
- [Program.cs](Program.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Obrigatório

## Prioridade para Produto Final
Crítica

---

### 3. Correção de IDOR em avaliações

## Descrição
As operações de avaliação aceitam `usuarioId` e `filmeId` vindos da rota, o que permite manipular registros de outros usuários.

## Evidências Encontradas
- [Controllers/AvaliacaoController.cs](Controllers/AvaliacaoController.cs)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [DTOs/AvaliacaoCreateDTO.cs](DTOs/AvaliacaoCreateDTO.cs)

## Impacto Atual
O cliente consegue tentar consultar e atualizar avaliações de terceiros. O modelo de posse do dado não está protegido.

## Impacto na PoC/Portfólio
Compromete a apresentação da API como sistema minimamente seguro.

## Impacto em Produção
Gera exposição e alteração indevida de avaliações, invalidando reputação, ranking e integridade dos dados.

## Como Implementar
1. Derivar o usuário autenticado do token.
2. Remover a dependência de `usuarioId` recebido do cliente para operações da própria conta.
3. Impedir atualização de avaliação de outro usuário.
4. Adicionar verificação de existência e ownership antes de salvar.
5. Cobrir com testes negativos.

## Arquivos Envolvidos
- [Controllers/AvaliacaoController.cs](Controllers/AvaliacaoController.cs)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [DTOs/AvaliacaoCreateDTO.cs](DTOs/AvaliacaoCreateDTO.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Obrigatório

## Prioridade para Produto Final
Crítica

---

### 4. Índices e unicidade no banco

## Descrição
O modelo atual carece de constraints únicas para e-mail e para combinações que deveriam ser exclusivas, como avaliação por usuário e filme.

## Evidências Encontradas
- [Migrations/20260402191049_InitialCreate.cs](Migrations/20260402191049_InitialCreate.cs)
- [Migrations/AppDbContextModelSnapshot.cs](Migrations/AppDbContextModelSnapshot.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)

## Impacto Atual
Duplicidades podem ser criadas por concorrência ou por ausência de validação consistente em atualizações.

## Impacto na PoC/Portfólio
Passa a impressão de que a modelagem relacional não foi finalizada.

## Impacto em Produção
Pode gerar contas duplicadas, avaliações duplicadas e inconsistência de ranking.

## Como Implementar
1. Mapear quais combinações devem ser exclusivas.
2. Criar constraints únicas no banco.
3. Ajustar validações de service para retornar erro amigável.
4. Rever migrações para refletir o estado atual do modelo.
5. Validar tratamento de concorrência.

## Arquivos Envolvidos
- [Data/AppDbContext.cs](Data/AppDbContext.cs)
- [Migrations/20260402191049_InitialCreate.cs](Migrations/20260402191049_InitialCreate.cs)
- [Migrations/AppDbContextModelSnapshot.cs](Migrations/AppDbContextModelSnapshot.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Obrigatório

## Prioridade para Produto Final
Crítica

---

### 5. Padronização da regra de nota e documentação

## Descrição
A fórmula de cálculo da nota final implementada no código não bate com o texto do README.

## Evidências Encontradas
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [README.md](README.md)

## Impacto Atual
O projeto transmite inconsistência entre documentação e implementação.

## Impacto na PoC/Portfólio
Pode gerar dúvida sobre cuidado com requisitos e revisão técnica.

## Impacto em Produção
Pode produzir ranking incorreto se a regra estiver realmente divergente do esperado pelo negócio.

## Como Implementar
1. Definir a regra oficial de negócio.
2. Ajustar a documentação para refletir a regra real ou corrigir o código para bater com a regra documentada.
3. Criar teste de regressão para a fórmula.
4. Revisar impacto no ranking e na apresentação dos dados.

## Arquivos Envolvidos
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [README.md](README.md)

## Nível de Dificuldade
Fácil

## Tempo Estimado
2 a 4 horas

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Alta

---

### 6. Controller de favoritos funcional

## Descrição
Existe serviço para favoritos, mas o controller não está implementado como endpoint REST de fato.

## Evidências Encontradas
- [Controllers/FavoritoController.cs](Controllers/FavoritoController.cs)
- [Services/FavoritoService.cs](Services/FavoritoService.cs)
- [Data/AppDbContext.cs](Data/AppDbContext.cs)
- [Migrations/20260402191049_InitialCreate.cs](Migrations/20260402191049_InitialCreate.cs)

## Impacto Atual
A funcionalidade existe parcialmente no código, mas não está exposta corretamente à API.

## Impacto na PoC/Portfólio
Enfraquece a percepção de domínio do ASP.NET Core Web API porque há um serviço sem endpoint utilizável.

## Impacto em Produção
Usuários não conseguem consumir a funcionalidade de forma confiável.

## Como Implementar
1. Transformar o arquivo em um controller MVC real.
2. Definir rotas e verbos HTTP apropriados.
3. Registrar o service no container de DI.
4. Garantir que a tabela exista no banco e esteja refletida na migration.
5. Adicionar validação e autorização.

## Arquivos Envolvidos
- [Controllers/FavoritoController.cs](Controllers/FavoritoController.cs)
- [Services/FavoritoService.cs](Services/FavoritoService.cs)
- [Program.cs](Program.cs)
- [Data/AppDbContext.cs](Data/AppDbContext.cs)
- [Migrations/20260402191049_InitialCreate.cs](Migrations/20260402191049_InitialCreate.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 dia

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Alta

---

### 7. Logging e tratamento de exceções mais profissional

## Descrição
O middleware trata exceções, mas devolve diretamente a mensagem da exception e não há logging estruturado.

## Evidências Encontradas
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)
- [Program.cs](Program.cs)

## Impacto Atual
Falhas são retornadas de forma muito genérica do ponto de vista operacional, e ao mesmo tempo expõem detalhes internos ao cliente.

## Impacto na PoC/Portfólio
Reduz o acabamento técnico percebido, especialmente em entrevistas focadas em produção.

## Impacto em Produção
Dificulta diagnóstico e pode vazar detalhes internos de erro.

## Como Implementar
1. Incluir logging estruturado no pipeline.
2. Diferenciar mensagens internas de mensagens retornadas ao cliente.
3. Padronizar payload de erro.
4. Cobrir casos conhecidos com status code adequados.
5. Adicionar correlação de requisição.

## Arquivos Envolvidos
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)
- [Program.cs](Program.cs)

## Nível de Dificuldade
Fácil

## Tempo Estimado
4 a 6 horas

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Alta

---

### 8. Padronização arquitetural entre controllers e services

## Descrição
Alguns controllers usam service, outros acessam o DbContext diretamente. Isso mostra inconsistência de desenho.

## Evidências Encontradas
- [Controllers/FilmeController.cs](Controllers/FilmeController.cs)
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Controllers/GenerosController.cs](Controllers/GenerosController.cs)
- [Controllers/PlataformasController.cs](Controllers/PlataformasController.cs)
- [Controllers/FuncaoController.cs](Controllers/FuncaoController.cs)
- [Controllers/CreditoController.cs](Controllers/CreditoController.cs)

## Impacto Atual
A manutenção fica desigual porque cada controller adota um padrão diferente.

## Impacto na PoC/Portfólio
Não destrói o projeto, mas entrega sensação de arquitetura improvisada.

## Impacto em Produção
Eleva custo de manutenção, dificulta testes e aumenta o risco de regressão.

## Como Implementar
1. Definir um padrão único de aplicação.
2. Decidir se todos os fluxos passam por services ou por use cases.
3. Tirar acesso direto ao DbContext dos controllers.
4. Separar consulta, validação e persistência em responsabilidades claras.
5. Uniformizar retorno de erros e status codes.

## Arquivos Envolvidos
- [Controllers/FilmeController.cs](Controllers/FilmeController.cs)
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Controllers/GenerosController.cs](Controllers/GenerosController.cs)
- [Controllers/PlataformasController.cs](Controllers/PlataformasController.cs)
- [Controllers/FuncaoController.cs](Controllers/FuncaoController.cs)
- [Controllers/CreditoController.cs](Controllers/CreditoController.cs)
- [Services/*.cs](Services)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 3 dias

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Alta

---

### 9. Testes automatizados mínimos

## Descrição
Não há evidência de testes unitários ou de integração no workspace.

## Evidências Encontradas
- Ausência de arquivos de teste no workspace

## Impacto Atual
Mudanças em autenticação, avaliação, CRUDs e regras de nota entram sem proteção contra regressão.

## Impacto na PoC/Portfólio
Projeto fica mais frágil em demonstrações, e avaliações técnicas tendem a penalizar a falta de cobertura.

## Impacto em Produção
Cada ajuste passa a depender só de validação manual, o que aumenta o risco de falhas silenciosas.

## Como Implementar
1. Definir o que é mais crítico para cobertura inicial.
2. Criar testes para autenticação e autorização.
3. Criar testes para regra de nota e ranking.
4. Criar testes para duplicidade de usuário e avaliação.
5. Criar testes para respostas de erro e status codes.

## Arquivos Envolvidos
- Estrutura de testes a ser criada no workspace
- [Controllers/*.cs](Controllers)
- [Services/*.cs](Services)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 3 dias

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Crítica

---

## Parte 2 — O que precisa ser feito para transformar o CineRank em um produto pronto para produção

Nesta seção entram os itens necessários para suportar usuários reais, segurança, escalabilidade, observabilidade e manutenção de longo prazo.

### 1. JWT com issuer, audience, expiração e revogação

## Descrição
O fluxo atual de autenticação usa JWT de forma básica, sem amarração completa de emissor, público, revogação ou gestão de sessão.

## Evidências Encontradas
- [Program.cs](Program.cs)
- [Services/AuthService.cs](Services/AuthService.cs)
- [Controllers/AuthController.cs](Controllers/AuthController.cs)

## Impacto Atual
Funciona para autenticar, mas não oferece o nível de controle esperado para um sistema com usuários reais.

## Impacto na PoC/Portfólio
Não é o maior problema de apresentação se os demais pontos forem corrigidos, mas ainda demonstra solução incompleta.

## Impacto em Produção
Usuários deslogados, trocas de senha e alteração de role continuam com tokens válidos até expirarem.

## Como Implementar
1. Definir issuer, audience e tempo de expiração.
2. Adicionar estratégia de refresh token ou revogação.
3. Invalidar sessões após troca de senha e alteração de role.
4. Revisar como os claims são consumidos no restante da API.
5. Cobrir com testes de segurança e login.

## Arquivos Envolvidos
- [Program.cs](Program.cs)
- [Services/AuthService.cs](Services/AuthService.cs)
- [Controllers/AuthController.cs](Controllers/AuthController.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)

## Nível de Dificuldade
Difícil

## Tempo Estimado
2 a 4 dias

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Crítica

---

### 2. Controle de acesso baseado em ownership e claims

## Descrição
O sistema precisa deixar de confiar em IDs enviados pelo cliente para qualquer operação que envolva dados próprios do usuário.

## Evidências Encontradas
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Controllers/AvaliacaoController.cs](Controllers/AvaliacaoController.cs)
- [Services/AuthService.cs](Services/AuthService.cs)

## Impacto Atual
O risco de acesso indevido está concentrado nos endpoints mais sensíveis do sistema.

## Impacto na PoC/Portfólio
Mesmo como PoC, isso é uma falha grave se o objetivo for demonstrar maturidade em APIs seguras.

## Impacto em Produção
Permite exibição, alteração e remoção indevida de dados de terceiros.

## Como Implementar
1. Definir claramente o que é recurso do próprio usuário e o que é recurso administrativo.
2. Ler o usuário autenticado em todas as rotas sensíveis.
3. Validar ownership antes de atualizar, excluir ou consultar.
4. Separar fluxos de admin e usuário comum.
5. Adicionar logs de tentativas inválidas.

## Arquivos Envolvidos
- [Controllers/UsuarioController.cs](Controllers/UsuarioController.cs)
- [Controllers/AvaliacaoController.cs](Controllers/AvaliacaoController.cs)
- [Services/UsuarioService.cs](Services/UsuarioService.cs)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 3 dias

## Prioridade para Portfólio/PoC
Obrigatório

## Prioridade para Produto Final
Crítica

---

### 3. Integridade relacional no banco

## Descrição
O banco precisa refletir as regras de negócio com constraints e índices adequados, não apenas com FKs básicas.

## Evidências Encontradas
- [Data/AppDbContext.cs](Data/AppDbContext.cs)
- [Migrations/20260402191049_InitialCreate.cs](Migrations/20260402191049_InitialCreate.cs)
- [Migrations/20260417224454_AdicionaRoleUsuario.cs](Migrations/20260417224454_AdicionaRoleUsuario.cs)

## Impacto Atual
Parte da integridade depende de validação em memória e da boa vontade do código de aplicação.

## Impacto na PoC/Portfólio
Melhora a percepção técnica se for tratada, porque demonstra cuidado com modelagem.

## Impacto em Produção
Sem constraints corretas, concorrência e erros de integração viram inconsistência persistente.

## Como Implementar
1. Listar regras de unicidade e cardinalidade.
2. Ajustar o modelo EF para refletir essas regras.
3. Gerar migrações novas ou corretivas.
4. Revisar comportamento de cascade e delete.
5. Testar inserções duplicadas e casos limites.

## Arquivos Envolvidos
- [Data/AppDbContext.cs](Data/AppDbContext.cs)
- [Models/*.cs](Models)
- [Migrations/*.cs](Migrations)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 3 dias

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Crítica

---

### 4. Validações de entrada e respostas de erro consistentes

## Descrição
O projeto possui validações pontuais nos DTOs, mas falta consistência de tratamento em todos os fluxos sensíveis.

## Evidências Encontradas
- [DTOs/*.cs](DTOs)
- [Controllers/*.cs](Controllers)
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)

## Impacto Atual
Alguns erros retornam mensagens adequadas, outros dependem de exceções genéricas ou validações implícitas.

## Impacto na PoC/Portfólio
Pode ser percebido como falta de acabamento de API profissional.

## Impacto em Produção
Mensagens inconsistentes e status codes fracos dificultam integração com frontend e observabilidade.

## Como Implementar
1. Padronizar validação por tipo de recurso.
2. Diferenciar erro de validação, erro de negócio e erro técnico.
3. Ajustar payload de resposta para erro.
4. Garantir consistência de status codes.
5. Rever contratos de DTO com foco em integração de frontend.

## Arquivos Envolvidos
- [DTOs/*.cs](DTOs)
- [Controllers/*.cs](Controllers)
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)

## Nível de Dificuldade
Fácil

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Alta

---

### 5. Observabilidade e logs estruturados

## Descrição
O sistema não possui evidência de logging, métricas, tracing ou health checks suficientes.

## Evidências Encontradas
- [Program.cs](Program.cs)
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)
- Ausência de classes de logging dedicadas

## Impacto Atual
Sem visibilidade operacional, o diagnóstico depende de reprodução manual.

## Impacto na PoC/Portfólio
Não impede a demonstração, mas reduz o valor percebido para avaliação sênior.

## Impacto em Produção
Dificulta identificar falhas, latência, abuso e problemas de disponibilidade.

## Como Implementar
1. Escolher um padrão de logging.
2. Registrar eventos de autenticação, autorização e falha de persistência.
3. Adicionar correlacionamento de requisições.
4. Criar health checks básicos.
5. Planejar dashboards e alertas.

## Arquivos Envolvidos
- [Program.cs](Program.cs)
- [Middleware/ExceptionMiddleware.cs](Middleware/ExceptionMiddleware.cs)
- Estrutura adicional de observabilidade a ser criada

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Opcional

## Prioridade para Produto Final
Alta

---

### 6. Performance de leitura e projeção

## Descrição
As consultas fazem uso intenso de Include e projeção sem evidência de otimização explícita para leitura em maior escala.

## Evidências Encontradas
- [Services/FilmeService.cs](Services/FilmeService.cs)
- [Services/PessoaService.cs](Services/PessoaService.cs)

## Impacto Atual
Funciona para baixo volume, mas pode degradar com crescimento de dados.

## Impacto na PoC/Portfólio
É aceitável como base de demonstração, mas não como referência de design de performance.

## Impacto em Produção
Pode aumentar latência, custo de banco e consumo de memória.

## Como Implementar
1. Revisar consultas mais pesadas.
2. Avaliar uso de projeções mais enxutas.
3. Reduzir carregamento desnecessário de navegações.
4. Introduzir consultas mais específicas para listagens e ranking.
5. Medir antes e depois.

## Arquivos Envolvidos
- [Services/FilmeService.cs](Services/FilmeService.cs)
- [Services/PessoaService.cs](Services/PessoaService.cs)
- [Controllers/FilmeController.cs](Controllers/FilmeController.cs)

## Nível de Dificuldade
Médio

## Tempo Estimado
1 a 2 dias

## Prioridade para Portfólio/PoC
Opcional

## Prioridade para Produto Final
Alta

---

### 7. Padrão de documentação e manutenção de regras

## Descrição
O projeto precisa manter documentação, comportamento e nomeação alinhados para reduzir ambiguidade durante manutenção futura.

## Evidências Encontradas
- [README.md](README.md)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [Controllers/*.cs](Controllers)

## Impacto Atual
Há discrepâncias entre o que o README descreve e o que o código realmente executa.

## Impacto na PoC/Portfólio
Documentação incoerente derruba a confiança em apresentação técnica.

## Impacto em Produção
Em produto real, documentação desatualizada produz erro de operação e dificuldade de onboarding.

## Como Implementar
1. Revisar o README para refletir o comportamento real.
2. Manter regras de negócio explicitadas em local único.
3. Atualizar nomes e descrições de endpoints conforme a semântica real.
4. Criar checklist de revisão documental sempre que a regra mudar.

## Arquivos Envolvidos
- [README.md](README.md)
- [Services/AvaliacaoService.cs](Services/AvaliacaoService.cs)
- [Controllers/*.cs](Controllers)

## Nível de Dificuldade
Fácil

## Tempo Estimado
2 a 4 horas

## Prioridade para Portfólio/PoC
Importante

## Prioridade para Produto Final
Média

---

## Tabela Consolidada

| Item | Dificuldade | Tempo Estimado | Prioridade Portfólio | Prioridade Produção | Importante |
| --- | --- | --- | --- | --- | --- |
| Segredo JWT fora do repositório | Fácil | 2 a 4 horas | Obrigatório | Crítica | Sim |
| Correção de IDOR em usuários | Médio | 1 a 2 dias | Obrigatório | Crítica | Sim |
| Correção de IDOR em avaliações | Médio | 1 a 2 dias | Obrigatório | Crítica | Sim |
| Índices e unicidade no banco | Médio | 1 a 2 dias | Obrigatório | Crítica | Sim |
| Padronização da regra de nota e documentação | Fácil | 2 a 4 horas | Importante | Alta | Sim |
| Controller de favoritos funcional | Médio | 1 dia | Importante | Alta | Sim |
| Logging e tratamento de exceções profissional | Fácil | 4 a 6 horas | Importante | Alta | Sim |
| Padronização arquitetural entre controllers e services | Médio | 1 a 3 dias | Importante | Alta | Sim |
| Testes automatizados mínimos | Médio | 1 a 3 dias | Importante | Crítica | Sim |
| JWT com issuer, audience, expiração e revogação | Difícil | 2 a 4 dias | Importante | Crítica | Sim |
| Controle de acesso por ownership e claims | Médio | 1 a 3 dias | Obrigatório | Crítica | Sim |
| Integridade relacional no banco | Médio | 1 a 3 dias | Importante | Crítica | Sim |
| Validações de entrada e respostas consistentes | Fácil | 1 a 2 dias | Importante | Alta | Sim |
| Observabilidade e logs estruturados | Médio | 1 a 2 dias | Opcional | Alta | Sim |
| Performance de leitura e projeção | Médio | 1 a 2 dias | Opcional | Alta | Sim |
| Padrão de documentação e manutenção de regras | Fácil | 2 a 4 horas | Importante | Média | Sim |

## Observação Final

Este documento não altera o projeto e não gera código. Ele organiza os achados técnicos em um plano de implementação para orientar a evolução do CineRank em duas fases: primeiro como portfólio sólido, depois como produto apto a produção.