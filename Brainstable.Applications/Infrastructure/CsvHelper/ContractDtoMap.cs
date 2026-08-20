using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class ContractDtoMap : ClassMap<ContractDto>
{
    public ContractDtoMap()
    {
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");
        Map(m => m.PatentId).Name("Н_ПАТЕНТА");
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.DateRegistration).Name("ДАТА_РЕГИСТРАЦИИ");
        Map(m => m.TypeLicence).Name("ВИД");
        Map(m => m.ContractNumber).Name("Н_ДОГОВОРА");
        Map(m => m.Conditions).Name("УСЛОВИЯ").Optional();
        Map(m => m.WhoSubjectId).Name("КТО").Optional();
        Map(m => m.WhomSubjectId).Name("КОМУ").Optional();
        Map(m => m.DateAnnulation).Name("ДАТА_АННУЛИРОВАНИЯ").Optional();
        Map(m => m.DateExpiration).Name("ДАТА_К_ДЕЙСТВИЯ").Optional();
    }
}