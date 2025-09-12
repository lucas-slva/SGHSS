# SGHSS 🏥

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Secure-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![xUnit](https://img.shields.io/badge/xUnit-Testing-5B2A89?style=flat-square&logo=xunit&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-Logging-512BD4?style=flat-square&logo=nuget&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/CI/CD-GitHub%20Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white)
![.NET 9 CI](https://github.com/lucas-slva/SGHSS/actions/workflows/dotnet.yml/badge.svg)



## 📖 Overview
SGHSS (**Sistema de Gestão Hospitalar e de Serviços de Saúde**) é uma API robusta desenvolvida em **.NET 8** para gerenciamento de pacientes, profissionais e consultas, com autenticação JWT e boas práticas de arquitetura.


## 🚀 Features
- [ ] Autenticação com JWT
- [ ] CRUD de Pacientes
- [ ] CRUD de Profissionais
- [ ] CRUD de Consultas
- [ ] Validações com FluentValidation
- [ ] Logs com Serilog + Middleware Customizado
- [ ] Seed Data inicial no banco
- [ ] Testes Unitários com xUnit
- [ ] Documentação de Endpoints com Swagger
- [ ] CI/CD com GitHub Actions

&nbsp;

## 🏗️ Project Architecture
```

/SGHSS
├── SGHSS.Api          -> Projeto Web API (.NET 8)
├── SGHSS.Core         -> Entidades, DTOs, Interfaces
├── SGHSS.Infrastructure -> EF Core, Contexto, Repositórios
├── SGHSS.Tests        -> Testes unitários

```

&nbsp;

## 📦 Tech Stack
- **.NET 8.0**
- **Entity Framework Core 8**
- **SQL Server / PostgreSQL**
- **JWT Authentication**
- **Swagger / Swashbuckle**
- **Serilog** para logging estruturado
- **AutoMapper** para mapeamento DTO ↔ Entidade
- **FluentValidation** para validação
- **xUnit & Moq** para testes
- **GitHub Actions** para CI/CD