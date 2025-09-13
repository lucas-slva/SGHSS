using FluentValidation;
using SGHSS.Core.DTOs;

namespace SGHSS.Core.Validations;

public class CreateConsultaDtoValidator : AbstractValidator<CreateConsultaDto>
{
    public CreateConsultaDtoValidator()
    {
        RuleFor(x => x.DataConsulta)
            .GreaterThan(DateTime.Now).WithMessage("A data da consulta deve ser futura");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status é obrigatório")
            .MaximumLength(50).WithMessage("O status deve ter no máximo 50 caracteres");

        RuleFor(x => x.PacienteId)
            .GreaterThan(0).WithMessage("PacienteId inválido");

        RuleFor(x => x.ProfissionalId)
            .GreaterThan(0).WithMessage("ProfissionalId inválido");
    }
}