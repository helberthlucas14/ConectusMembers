# 🧩 Connectus — Microserviço de Cadastro de Membros

O **Connectus** é um **microserviço em .NET 8** responsável pelo **cadastro e gerenciamento de membros** de uma plataforma.  
O projeto foi desenvolvido seguindo os princípios de **Clean Architecture**, **DDD (Domain-Driven Design)** e **TDD (Test-Driven Development)**, garantindo alta manutenibilidade, testabilidade e qualidade de código.

---

## 🚀 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core
- Swagger / Swashbuckle
- Docker & Docker Compose
- xUnit (testes unitários e de integração)
- Moq (mocking)
- FluentAssertions (asserts mais expressivos)

---

## 🧱 Arquitetura

O **Connectus** adota **Clean Architecture** e **DDD**, organizando o código em camadas independentes:

src/
- ├── Connectus.Api → Camada de apresentação (Controllers, Swagger, Middlewares)
- ├── Connectus.Application → Casos de uso (Services, DTOs, Validations, Mapeamentos)
- ├── Connectus.Domain → Entidades e interfaces de domínio
- ├── Connectus.Infrastructure → Persistência (EF Core, Repositórios, Configurações)
- └── tests/
- ├── Connectus.UnitTests → Testes de unidade (TDD)
- └── Connectus.IntegrationTests → Testes de integração (API e banco)
