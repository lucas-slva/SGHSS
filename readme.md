# SGHSS 🏥

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Secure-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![xUnit](https://img.shields.io/badge/xUnit-Testing-5B2A89?style=flat-square&logo=xunit&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-Logging-512BD4?style=flat-square&logo=nuget&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/CI/CD-GitHub%20Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-SQL%20Server-2496ED?style=flat-square&logo=docker&logoColor=white)
![.NET 9 CI](https://github.com/lucas-slva/SGHSS/actions/workflows/dotnet.yml/badge.svg)



## 📖 Overview
SGHSS (**Sistema de Gestão Hospitalar e de Serviços de Saúde**) é uma API robusta desenvolvida em **.NET 8** para gerenciamento de pacientes, profissionais e consultas, com autenticação JWT e boas práticas de arquitetura.


### 🚀 Features
- [x] Setup inicial do projeto (.NET 8, Solution, GitHub Actions)
- [x] Definição da arquitetura (Core, Api, Infrastructure, Tests)
- [x] Configuração do SQL Server via Docker + Docker Compose
- [x] CRUD de Pacientes
- [x] CRUD de Profissionais
- [x] CRUD de Consultas
- [x] Seed Data inicial no banco
- [x] Validações com FluentValidation
- [ ] Logs com Serilog + Middleware Customizado
- [ ] Autenticação com JWT
- [ ] Testes Unitários com xUnit
- [ ] Documentação de Endpoints com Swagger
- [ ] CI/CD com GitHub Actions

&nbsp;

### 🏗️ Project Architecture
```
/SGHSS
├── SGHSS.Api            -> Projeto Web API (.NET 8)
├── SGHSS.Core           -> Entidades, DTOs, Interfaces
├── SGHSS.Infrastructure -> EF Core, Contexto, Repositórios
├── SGHSS.Tests          -> Testes unitários
```

&nbsp;

### 📦 Tech Stack
- **.NET 8.0**
- **Entity Framework Core 8**
- **SQL Server (via Docker)**
- **JWT Authentication**
- **Swagger / Swashbuckle**
- **Serilog** para logging estruturado
- **AutoMapper** para mapeamento DTO ↔ Entidade
- **FluentValidation** para validação
- **xUnit & Moq** para testes
- **GitHub Actions** para CI/CD
- **Docker Compose** para orquestração do banco

&nbsp;

## 🔧 Getting Started

### ✅ Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para o SQL Server)

### ▶️ Como rodar o projeto

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/SGHSS.git
   cd SGHSS
   ```
2. **Suba o banco de dados via Docker**

   ```bash
   docker-compose up -d
   #Isso vai iniciar o SQL Server 2022 na porta `1433`.
   ```

3. **Restaure e compile o projeto**

   ```bash
   dotnet restore
   dotnet build
   ```

4. **Rode a API**

   ```bash
   dotnet run --project SGHSS.Api
   ```

5. **Acesse a API**

    * Swagger UI: [https://localhost:7001/swagger](https://localhost:7001/swagger)
    * Health Check básico: [https://localhost:7001](https://localhost:7001)

&nbsp;

### 🛠️ Estrutura do Banco

* **Banco:** SGHSS
* **Usuário:** `sa`
* **Senha:** `Your_password123` (definida no `docker-compose.yml`)
* **Porta:** `1433`

> ⚠️ O banco é criado e atualizado automaticamente via **Entity Framework Core Migrations**.

&nbsp;

### 🧪 Rodando os Testes

Para executar todos os testes unitários:

```bash
dotnet test SGHSS.sln
```

