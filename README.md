# API de Produtos

Este é um projeto de API desenvolvido para gerenciamento de produtos utilizando .NET Core e Entity Framework.

## Funcionalidades

- CRUD de produtos (Create, Read, Update, Delete).
- Autenticação via JWT.
- Testes unitários com cobertura de 80%+.
- Documentação da API com Swagger/OpenAPI.

## Como rodar o projeto

1. Clone este repositório.
2. Abra no Visual Studio ou no seu editor de preferência.
3. Execute o projeto.

## Endpoints
- `GET /api/Produtos` - Lista todos os produtos.
- `POST /api/Produtos` - Cria um novo produto.
- `PUT /api/Produtos/{id}` - Atualiza um produto existente.
- `DELETE /api/Produtos/{id}` - Deleta um produto existente.
- `GET /WeatherForecast` - Retorna uma previsão do tempo (exemplo de integração externa).

## Testes

Os testes unitários estão na pasta `ProdutoAPITests`, e você pode executá-los utilizando o xUnit.
