using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class ApplicationMap : ClassMap<ApplicationDto>
{
    public ApplicationMap()
    {
        Map(m => m.Id).Name("Н_ЗАЯВКИ");
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.Type).Name("Н_ТИПА_ЗАЯВКИ");
        Map(m => m.DateRegistration).Name("ДАТА_РЕГИСТРАЦИИ");
        Map(m => m.DateRejection).Name("ДАТА_ОТКЛОНЕНИЯ").Optional();
        Map(m => m.CauseRejection).Name("Н_ПРИЧ_ОТКЛОНЕНИЯ").Optional();
        Map(m => m.DateExpertise).Name("ДАТА_ЭКСПЕРТИЗЫ").Optional();
        Map(m => m.ApplicaitorId).Name("Н_СУБЪЕКТА_П");
        Map(m => m.DateInclusion).Name("ДАТА_ВКЛЮЧЕНИЯ").Optional();
        Map(m => m.NumberCaudeAdguardEnd).Name("Н_ПРИЧ_К_ОХР_ДОП").Optional();

        // Остальные поля игнорируем
    }
}