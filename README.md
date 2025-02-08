# Uai Granja API

A **Uai Granja API** é uma aplicação desenvolvida para gerenciar operações relacionadas à avicultura, fornecendo funcionalidades essenciais para o controle e automação de granjas.

## Funcionalidades

- Gerenciamento de lotes de aves
- Controle de alimentação e consumo
- Monitoramento de saúde e bem-estar das aves
- Relatórios de produção e desempenho

## Tecnologias Utilizadas

- **Linguagem de Programação:** C#
- **Framework:** .NET 6
- **Banco de Dados:** PostgreSQL
  
## Estrutura do Projeto

O projeto está organizado da seguinte forma:

```
uai-granja-api/
├── src/
│   ├── UaiGranja.Avicultura.Domain/
│   ├── UaiGranja.Avicultura.Application/
│   ├── UaiGranja.Avicultura.Infrastructure/
│   └── UaiGranja.Avicultura.API/
├── tests/
│   └── UaiGranja.Avicultura.Domain.Tests/
├── .gitignore
├── LICENSE
└── UaiGranja.sln
```

- **src/**: Contém o código-fonte da aplicação dividido em camadas de Domínio, Aplicação, Infraestrutura e API.
- **tests/**: Inclui os testes unitários do projeto.
- **UaiGranja.sln**: Arquivo de solução do Visual Studio.

## Pré-requisitos

- .NET 6 SDK ou superior
- Banco de dados PostgreSQL configurado

## Como Executar

1. Clone o repositório:

   ```bash
   git clone https://github.com/victorfg21/uai-granja-api.git
   ```

2. Navegue até o diretório do projeto:

   ```bash
   cd uai-granja-api
   ```

3. Restaure as dependências:

   ```bash
   dotnet restore
   ```

4. Atualize as configurações de conexão com o banco de dados no arquivo `appsettings.json` conforme necessário.

5. Execute a aplicação:

   ```bash
   dotnet run --project src/UaiGranja.Avicultura.API/UaiGranja.Avicultura.API.csproj
   ```

## Testes

Para executar os testes unitários, utilize o seguinte comando:

```bash
   dotnet test
```

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

## Licença

Este projeto está licenciado sob a Licença MIT. Consulte o arquivo [LICENSE](LICENSE) para mais informações.
