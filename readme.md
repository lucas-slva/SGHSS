# SGHSS 🏥

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![xUnit](https://img.shields.io/badge/xUnit-Testing-5B2A89?style=flat-square&logo=xunit&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-Logging-512BD4?style=flat-square&logo=nuget&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/CI/CD-GitHub%20Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-SQL%20Server-2496ED?style=flat-square&logo=docker&logoColor=white)
![.NET 9 CI](https://github.com/lucas-slva/SGHSS/actions/workflows/dotnet.yml/badge.svg)



## 📖 Overview
SGHSS (**Sistema de Gestão Hospitalar e de Serviços de Saúde**) é uma API robusta desenvolvida em **.NET 8** para gerenciamento de pacientes, profissionais e consultas, com autenticação JWT e boas práticas de arquitetura.


### 🚀 Features
✅ Setup inicial do projeto (.NET 8, Solution, GitHub Actions)  
✅ Definição da arquitetura (Core, Api, Infrastructure, Tests)  
✅ Configuração do SQL Server via Docker + Docker Compose  
✅ CRUD de Pacientes  
✅ CRUD de Profissionais  
✅ CRUD de Consultas  
✅ Seed Data inicial no banco  
✅ Validações com FluentValidation  
✅ Documentação de Endpoints com Swagger  
✅ Autenticação com JWT  
✅ Logs com Serilog + Middleware Customizado  
✅ Testes Unitários com xUnit  
⬜ CI/CD com GitHub Actions

&nbsp;

### 🏗️ Arquitetura do projeto
```
/SGHSS
├── SGHSS.Api               -> Camada de apresentação
│   ├── Controllers         -> Endpoints da aplicação (Pacientes, Profissionais, Consultas, Auth)
│   ├── Logs                -> Configuração de logging com Serilog
│   ├── Middlewares         -> Tratamento centralizado de erros e extensões
│   ├── appsettings.json    -> Configurações globais
│   └── Program.cs          -> Entry point da aplicação
│
├── SGHSS.Core              -> Regras de negócio e objetos centrais
│   ├── DTOs                -> Objetos de transferência de dados (input/output da API)
│   ├── Entities            -> Entidades do domínio (Paciente, Profissional, Consulta, Usuário)
│   ├── Services            -> Serviços centrais (ex: geração de JWT)
│   └── Validations         -> Regras de validação com FluentValidation
│
├── SGHSS.Infrastructure    -> Infraestrutura e persistência
│   ├── Data                -> DbContext e configurações de banco de dados
│   ├── Mappings            -> Perfis de mapeamento do AutoMapper
│   └── Migrations          -> Migrações do Entity Framework Core
│
├── SGHSS.Tests             -> Testes automatizados (xUnit + FluentAssertions)
│   ├── Controllers         -> Testes unitários para os controllers
│   └── Services            -> Testes unitários para serviços
│
└── SGHSS.sln               -> Solution principal

```

&nbsp;

### 📦 Stacks usadas
- **.NET 8.0**
- **Entity Framework Core 8**
- **SQL Server (via Docker)**
- **JWT Authentication**
- **Swagger** (com botão de autenticação JWT)
- **Serilog** para logging estruturado
- **Middleware global de tratamento de erros** para respostas padronizadas
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

    * Swagger UI: [https://localhost:5293/swagger](https://localhost:7001/swagger)
    * Health Check básico: [https://localhost:5293](https://localhost:7001)

### 🔑 Autenticação

#### O login é feito via endpoint:

```
POST /api/auth/login
{
  "email": "admin@sghss.com",
  "senha": "admin123"
}
```

#### Resposta (exemplo):

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR..."
}
```

#### O token deve ser enviado no header:

```
Authorization: Bearer {token}
```

&nbsp;

### 🧪 Testes Unitários

O projeto conta com uma suíte de **testes unitários** desenvolvida com **xUnit** e **FluentAssertions**, garantindo a qualidade e robustez da API.

Os testes foram organizados da seguinte forma:

- **Controllers**
    - `PacientesControllerTests`
    - `ProfissionaisControllerTests`
    - `ConsultasControllerTests`
    - `AuthControllerTests`
- **Serviços**
    - `JwtTokenServiceTests`

🔹 Para simulação do banco de dados, foi utilizado o **Entity Framework InMemory**, permitindo cenários realistas sem a necessidade de um SQL Server rodando.

🔹 Cada CRUD é testado com múltiplos cenários, incluindo:
- Retorno de todos os registros
- Busca por ID válido e inválido
- Criação de entidades
- Atualização com sucesso e falha (IDs divergentes, não encontrados)
- Exclusão existente e inexistente
- Fluxo de autenticação com sucesso e falha

#### ▶️ Executando os testes

Para rodar todos os testes:

```bash
dotnet test SGHSS.sln
```

Exemplo de saída esperada:

```
Test Run Successful.
Total tests: 29
     Passed: 29
     Failed: 0
     Skipped: 0
Test execution time: 1.4s
```

Com isso, asseguramos que a API está **totalmente coberta nos principais cenários** e pronta para ser evoluída com segurança 🚀.