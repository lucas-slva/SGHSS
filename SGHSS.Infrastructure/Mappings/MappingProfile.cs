using AutoMapper;
using SGHSS.Core.Entities;
using SGHSS.Core.DTOs;

namespace SGHSS.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Paciente
        CreateMap<Paciente, PacienteDto>().ReverseMap();
        CreateMap<Paciente, CreatePacienteDto>().ReverseMap();
        
        // Profissionais
        CreateMap<Profissional, ProfissionalDto>().ReverseMap();
        CreateMap<CreateProfissionalDto, Profissional>();
        
        // Consulta
        CreateMap<Consulta, ConsultaDto>()
            .ForMember(dest => dest.PacienteNome, opt => opt.MapFrom(src => src.Paciente!.Nome))
            .ForMember(dest => dest.ProfissionalNome, opt => opt.MapFrom(src => src.Profissional!.Nome));

        CreateMap<Consulta, CreateConsultaDto>().ReverseMap();
    }
}