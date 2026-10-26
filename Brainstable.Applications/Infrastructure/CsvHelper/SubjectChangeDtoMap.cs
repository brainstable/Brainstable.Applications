using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла АРХИВ_СД.csv со свойствами <see cref="SubjectChangeDto"/>.
/// </summary>
public class SubjectChangeDtoMap : ClassMap<SubjectChangeDto>
{
    public SubjectChangeDtoMap()
    {
        // Заявка и сорт, по которым произошло изменение состава субъектов.
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");
        Map(m => m.TypeApplication).Name("Н_ТИПА_ЗАЯВКИ");
        Map(m => m.VarietyId).Name("К_СОРТА");

        // Старый и новый субъекты: один из них может отсутствовать.
        Map(m => m.OldSubjectId).Name("Н_СУБ_СТАРЫЙ").Optional();
        Map(m => m.NewSubjectId).Name("Н_СУБ_НОВЫЙ").Optional();

        // Тип субъекта, причина и дата изменения.
        Map(m => m.TypeSubject).Name("ТИП_СУБЪЕКТА");
        Map(m => m.CauseChange).Name("П_ИЗМЕНЕНИЯ");
        Map(m => m.DateChanged).Name("ДАТА_ИЗМЕНЕНИЯ").Optional();
    }
}
