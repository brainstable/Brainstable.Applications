using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла ЗАЯВКА.csv со свойствами <see cref="ApplicationDto"/>.
/// Колонки, не указанные в карте, при чтении игнорируются.
/// </summary>
internal class ApplicationMap : ClassMap<ApplicationDto>
{
    public ApplicationMap()
    {
        // Идентификация заявки и связанного сорта.
        Map(m => m.Id).Name("Н_ЗАЯВКИ");
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.Type).Name("Н_ТИПА_ЗАЯВКИ");

        // Даты регистрации, экспертизы и отклонения заявки.
        Map(m => m.DateRegistration).Name("ДАТА_РЕГИСТРАЦИИ");
        Map(m => m.DateRejection).Name("ДАТА_ОТКЛОНЕНИЯ").Optional();
        Map(m => m.CauseRejection).Name("Н_ПРИЧ_ОТКЛОНЕНИЯ").Optional();
        Map(m => m.DateExpertise).Name("ДАТА_ЭКСПЕРТИЗЫ").Optional();

        // Заявитель из самой заявки и служебные поля.
        Map(m => m.ApplicaitorId).Name("Н_СУБЪЕКТА_П");
        Map(m => m.DateInclusion).Name("ДАТА_ВКЛЮЧЕНИЯ").Optional();
        Map(m => m.NumberCaudeAdguardEnd).Name("Н_ПРИЧ_К_ОХР_ДОП").Optional();
    }
}
