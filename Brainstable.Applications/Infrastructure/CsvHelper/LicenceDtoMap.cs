using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла ЛИЦЕНЗИЯ.csv со свойствами <see cref="LicenceDto"/>.
/// </summary>
internal class LicenceDtoMap : ClassMap<LicenceDto>
{
    public LicenceDtoMap()
    {
        // Заявка, патент и лицензиат.
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");
        Map(m => m.PatentId).Name("Н_ПАТЕНТА");
        Map(m => m.SubjectId).Name("Н_СУБЪЕКТА");

        // Регистрационные данные лицензии.
        Map(m => m.DateRegistration).Name("ДАТА_РЕГИСТРАЦИИ");
        Map(m => m.NumberLicence).Name("Н_ЛИЦЕНЗИИ");
        Map(m => m.TypeLicence).Name("ВИД");
        Map(m => m.VarietyId).Name("К_СОРТА");

        // Даты прекращения действия и версия записи могут отсутствовать.
        Map(m => m.DateAnnulation).Name("ДАТА_АННУЛИРОВАНИЯ").Optional();
        Map(m => m.DateExpiration).Name("ДАТА_К_ДЕЙСТВИЯ").Optional();
        Map(m => m.Version).Name("ВЕРСИЯ").Optional();
    }
}
