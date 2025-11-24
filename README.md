# Desafio API – Documentação Completa

## Visão Geral

Este projeto foi desenvolvido como parte de um desafio técnico, utilizando **.NET 9** e uma arquitetura organizada em múltiplas camadas.
O objetivo principal é apresentar uma solução clara, modular e escalável, contendo listagem de produtos e um processo controlado por lock distribuído para evitar concorrência.

A seguir está toda a documentação do desenvolvimento, incluindo arquitetura, decisões técnicas e etapas executadas.

---

## Arquitetura da Solução

A aplicação foi desenvolvida com uma arquitetura limpa, dividida em camadas independentes.

### Domain

Contém exclusivamente a entidade principal:
**Produto**

### Application

Armazena as interfaces que definem os contratos da aplicação, como:
**IProdutoRepository**
**ILockService**

### Infrastructure

Implementa os recursos de infraestrutura, incluindo:
**ProdutoRepository**
**AppDbContext**
**RedisLockService** (controle de concorrência distribuída)

### API

Exposição dos endpoints utilizando o **HomeController**, retornando Views e processando requisições.

---

## Tecnologias Utilizadas

* .NET 9
* ASP.NET Core MVC
* Entity Framework Core
* InMemoryDatabase
* Redis com StackExchange.Redis
* xUnit para testes unitários
* Swagger para testes manuais

---

# Documentação do Desenvolvimento

## 1. Abertura do Chamado

Ao iniciar o desafio, analisei atentamente os requisitos e defini o uso do **.NET 9** pela modernidade da plataforma. Com isso, estabeleci uma arquitetura em múltiplas camadas (Domain, Application, Infrastructure e API) para garantir organização e facilidade de manutenção.

Na preparação inicial, defini a entidade principal, as interfaces necessárias e o fluxo de comunicação entre as camadas.

---

## 2. Solução Escolhida

Optei por uma arquitetura limpa e segmentada para proporcionar clareza e escalabilidade. Mesmo sendo um projeto pequeno, utilizei boas práticas de desenvolvimento.

Estrutura adotada:

**Domain**
Entidade Produto.

**Application**
Interfaces IProdutoRepository e ILockService.

**Infrastructure**
Repositório de produtos, contexto de banco e serviço de lock distribuído.

**API**
Controller responsável pela listagem e pelo processamento exclusivo.

Essa arquitetura garante separação de responsabilidades, organização e facilidade para expansões futuras.

---

## 3. Possíveis Problemas na Execução

Alguns pontos exigiram atenção especial:

* Separação clara de responsabilidades entre as camadas
* Comunicação entre Application e Infrastructure
* Implementação correta do lock distribuído para evitar concorrência

Testes contínuos permitiram validar o funcionamento dos endpoints e garantir o fluxo esperado.

---

## 4. Resultado Final

A API final apresenta dois endpoints principais:

### Listagem de Produtos (Index)

Busca todos os produtos utilizando o repositório:

```
var produtos = await _repo.GetAllAsync();
```

Se não houver itens, a View exibe uma mensagem informativa.

### Processamento de Relatório (/processar-relatorio)

Este endpoint simula um processo exclusivo protegido por lock distribuído.

Fluxo:

1. Tenta adquirir o lock
2. Se já estiver ocupado, retorna **HTTP 423 Locked**
3. Se obtiver o lock, executa o processamento simulado
4. Libera o lock ao final

Esse mecanismo impede execuções simultâneas em diferentes instâncias.

---

# Etapas do Desenvolvimento

### Etapa 1 – Estrutura Inicial

Criação dos projetos Domain, Application, Infrastructure e API.

### Etapa 2 – Entidade Produto

Modelagem da entidade dentro da camada Domain.

### Etapa 3 – Interfaces

Criação das interfaces **IProdutoRepository** e **ILockService**.

### Etapa 4 – Banco e AppDbContext

Configuração do contexto e definição dos dados utilizando InMemoryDatabase.

### Etapa 5 – Infraestrutura

Implementação do repositório e do serviço de lock distribuído.

### Etapa 6 – HomeController

Desenvolvimento dos endpoints Index e ProcessarRelatorio.

### Etapa 7 – Testes

Validação dos fluxos com Swagger.
Criação de testes unitários para o serviço de lock garantindo:

* Aquisição do lock
* Bloqueio de concorrência
* Liberação correta do recurso

### Etapa 8 – Revisão Final

Refinamentos, ajustes finais e preparação para entrega.

---

## Como eu faria o deploy do projeto em um servidor Linux

Para colocar esse projeto em produção em um servidor Linux, eu seguiria um processo simples, direto e seguro. A ideia é garantir que a aplicação rode de forma estável, tenha logs centralizados e esteja preparada para reiniciar automaticamente em caso de falhas.

A primeira etapa seria preparar o servidor. Atualizaria os pacotes do sistema e instalaria o .NET 9 Runtime, que é necessário para executar a aplicação. Depois disso, faria a publicação do projeto em modo Release usando o comando `dotnet publish`. Isso gera uma versão otimizada da API, pronta para ser enviada para produção. O conteúdo publicado seria enviado para o servidor, normalmente via SSH, para uma pasta específica como `/var/www/desafioapi`.

Com os arquivos no servidor, eu criaria um usuário dedicado para rodar a aplicação, evitando que ela seja executada como root, o que é uma boa prática de segurança. Em seguida, configuraria um serviço no systemd para que a API fosse executada como um serviço do sistema. Dessa forma, ela iniciaria junto com o servidor e seria reiniciada automaticamente caso ocorresse algum erro.

Se o ambiente exigisse o uso real de lock distribuído, eu instalaria o Redis no servidor. Ele pode ser instalado facilmente pelo gerenciador de pacotes do Linux, e funciona de forma imediata após ativado. Caso o projeto esteja utilizando InMemoryDatabase apenas para testes, isso não seria necessário no ambiente final.

Depois disso, configuraria o Nginx como proxy reverso. O objetivo dessa camada é simples: permitir que a aplicação seja acessada pela porta 80, além de proteger a API e melhorar o controle de tráfego. O Nginx recebe as requisições externas e repassa para a aplicação .NET, que normalmente roda em uma porta interna, como a 5000.

Por fim, faria uma verificação geral: status do serviço, logs, resposta no navegador e funcionamento do Swagger. A partir daí, o deploy estaria concluído. Nas próximas atualizações, bastaria publicar novamente o projeto, enviar os arquivos para o servidor e reiniciar o serviço.

Esse processo garante que o projeto rode de forma organizada, confiável e seguindo boas práticas de deploy para aplicações .NET em ambientes Linux.

---

# Como Executar o Projeto

## 1. Instalar Dependências

Certifique-se de ter instalado:

* .NET 9
* Redis (opcional caso teste o lock real)

## 2. Executar a API

A partir da pasta `/backend`:

```
dotnet run --project src/DesafioApi.Api
```

## 3. Acessar Swagger

Acesse no navegador:

```
http://localhost:5000/swagger
```

---

# Estrutura de Pastas

```
backend/
 └── src/
     ├── DesafioApi.Api
     ├── DesafioApi.Application
     ├── DesafioApi.Domain
     ├── DesafioApi.Infrastructure
     └── DesafioApi.Tests
```

---

# Licença

Este projeto pode ser utilizado para fins educacionais e de estudo.


