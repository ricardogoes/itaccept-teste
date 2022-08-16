# Introdução

Olá, este é o código fonte do projeto de teste solicitado. Acredito que cumpri com todas as regras... mas mais abaixo descrevo alguns TO DOs e possíveis próximos passos...

O repositório acabou ficando gigante, pois subi os arquivos de banco de dados também. Apesar de ser má prática, achei mais prudente vocês receberem os dados todos.
# Como executar

Para executar basta executar o comando: docker-compose up -d

# Estrutura do código

O código está estruturado da seguinte forma:

./db
=====
Contém o banco de dados mysql que utilizei nos testes. Criei o banco de dados usando uma imagem docker, pois assim ficou mais simples.

O comando para iniciar a image encontra-se no arquivo docker-compose.yaml.

./src/angular
=============

Contém todo o código fonte do site. Optei por fazer em angular, pois como não havia restrição de linguagem, utilizei a que estou usando com mais frequência no momento.

./src/api
=============

Contém todo o código fonte da API desenvolvida em .NET 6. 

Para o desenvolvimento da API utilizei:

Autenticação com JWT
Geração de password com SALT (https://docs.microsoft.com/pt-br/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0)
AutoMapper
Dapper

# Testes

Acabei não conseguindo avançar na criação dos testes para o projeto todo, então o que fiz foi criar a arquitetura básica para testar a classe TokenAuthController para testes unitários e testes funcionais de api.

Para os testes unitários, estou usando as seguintes bibliotecas:

Bogus (https://github.com/bchavez/Bogus) para a geração de dados fake
FluentAssertions (https://fluentassertions.com/) que faz com a escrita das validações (asserts) seja mais clara e mais assertiva
Moq (https://github.com/moq/moq4) para "mocar" as dependências das classes

Já para os testes de api, estou usando as seguintes bibliotecas:

Microsoft.AspNetCore.TestHost (https://docs.microsoft.com/pt-br/aspnet/core/test/integration-tests?view=aspnetcore-6.0) para criar o ambiente em que o teste está executado

# Possíveis próximos passos

1. Remover o código morto da API principalmente
2. Implementar notificações (SignalR, por exemplo) para informar a transportadora quando seu lance for mmarcado como vencedor ou quando a oferta não estiver mais disponível
3. Avançar na criação dos testes unitários e de api;