# 📊 Modelagem do Sistema SGHSS

Este documento apresenta a **modelagem conceitual e lógica** do sistema **SGHSS (Sistema de Gestão Hospitalar e de Serviços de Saúde)**, atendendo ao ponto 4 do projeto.  
Foram construídos dois diagramas principais:
1. **Diagrama de Classes (UML)** → visão orientada a objetos.
2. **Diagrama Entidade-Relacionamento (DER)** → visão do banco de dados relacional.

---

## 📐 Diagrama de Classes (UML)

O diagrama de classes descreve a estrutura das entidades de domínio da aplicação, bem como seus atributos e relacionamentos.

```mermaid
classDiagram
direction LR

class Usuario {
  +int Id
  +string Nome
  +string Email
  +string SenhaHash
  +string Role
}

class Paciente {
  +int Id
  +string Nome
  +string Cpf
  +DateTime DataNascimento
}

class Profissional {
  +int Id
  +string Nome
  +string Cpf
  +string Especialidade
}

class Consulta {
  +int Id
  +DateTime DataConsulta
  +string Status
  +int PacienteId
  +int ProfissionalId
}

Paciente "1" -- "0..*" Consulta : possui
Profissional "1" -- "0..*" Consulta : realiza
Consulta --> Paciente : PacienteId
Consulta --> Profissional : ProfissionalId
```

📌 **Observações sobre as entidades:**
- `Usuario`: controla autenticação e autorização (roles: Admin, Profissional, Paciente).
- `Paciente`: armazena dados pessoais e suas consultas.
- `Profissional`: registra dados do médico/profissional de saúde e as consultas realizadas.
- `Consulta`: associa paciente e profissional, com data e status (Agendada, Cancelada, Realizada).

---

## 🗄️ Diagrama Entidade-Relacionamento (DER)

O DER representa a modelagem lógica do banco de dados relacional que dará suporte ao sistema.

```mermaid
erDiagram
    PACIENTE ||--o{ CONSULTA : possui
    PROFISSIONAL ||--o{ CONSULTA : realiza

    PACIENTE {
        INT Id PK
        VARCHAR Nome
        CHAR(11) Cpf UK
        DATE DataNascimento
    }

    PROFISSIONAL {
        INT Id PK
        VARCHAR Nome
        CHAR(11) Cpf UK
        VARCHAR Especialidade
    }

    CONSULTA {
        INT Id PK
        DATETIME DataConsulta
        VARCHAR Status
        INT PacienteId FK
        INT ProfissionalId FK
    }

    USUARIO {
        INT Id PK
        VARCHAR Nome
        VARCHAR Email UK
        VARCHAR SenhaHash
        VARCHAR Role
    }
```

📌 **Observações sobre as tabelas e constraints:**
- `USUARIO.Email` e `PACIENTE.Cpf` / `PROFISSIONAL.Cpf` são **únicos** (constraints `UK`).
- `CONSULTA.PacienteId` → `PACIENTE.Id` (**FK**)
- `CONSULTA.ProfissionalId` → `PROFISSIONAL.Id` (**FK**)
- Relacionamento **1:N**: um paciente pode ter várias consultas.
- Relacionamento **1:N**: um profissional pode realizar várias consultas.

---

## ✅ Conclusão

A modelagem define claramente:
- A **estrutura orientada a objetos** (UML).
- O **modelo lógico relacional** (DER).
- Regras de unicidade (CPF, Email) e integridade referencial (FKs).

Com esses diagramas, garantimos que o sistema SGHSS possui uma base sólida de dados e entidades para evoluir nas próximas etapas do projeto.
