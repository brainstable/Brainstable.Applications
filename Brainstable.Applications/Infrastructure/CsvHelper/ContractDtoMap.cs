using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла ДОГОВОР.csv со свойствами <see cref="ContractDto"/>.
/// </summary>
internal class ContractDtoMap : ClassMap<ContractDto>
{
    public ContractDtoMap()
    {
        // Заявка, патент и сорт, к которым относится договор.
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");
        Map(m => m.PatentId).Name("Н_ПАТЕНТА");
        Map(m => m.VarietyId).Name("К_СОРТА");

        // Регистрационные данные договора.
        Map(m => m.DateRegistration).Name("ДАТА_РЕГИСТРАЦИИ");
        Map(m => m.TypeLicence).Name("ВИД");
        Map(m => m.ContractNumber).Name("Н_ДОГОВОРА");
        Map(m => m.Conditions).Name("УСЛОВИЯ").Optional();

        // Кто и кому передаёт право; поля могут отсутствовать.
        Map(m => m.WhoSubjectId).Name("КТО").Optional();
        Map(m => m.WhomSubjectId).Name("КОМУ").Optional();

        // Даты прекращения действия договора.
        Map(m => m.DateAnnulation).Name("ДАТА_АННУЛИРОВАНИЯ").Optional();
        Map(m => m.DateExpiration).Name("ДАТА_К_ДЕЙСТВИЯ").Optional();
    }
}
