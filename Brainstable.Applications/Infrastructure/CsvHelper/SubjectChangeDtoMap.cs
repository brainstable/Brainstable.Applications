using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

public class SubjectChangeDtoMap : ClassMap<SubjectChangeDto>
{
    public SubjectChangeDtoMap()
    {
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");
        Map(m => m.TypeApplication).Name("Н_ТИПА_ЗАЯВКИ");
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.OldSubjectId).Name("Н_СУБ_СТАРЫЙ").Optional();
        Map(m => m.TypeSubject).Name("ТИП_СУБЪЕКТА");
        Map(m => m.CauseChange).Name("П_ИЗМЕНЕНИЯ");
        Map(m => m.DateChanged).Name("ДАТА_ИЗМЕНЕНИЯ").Optional();
        Map(m => m.NewSubjectId).Name("Н_СУБ_НОВЫЙ").Optional();
    }
}