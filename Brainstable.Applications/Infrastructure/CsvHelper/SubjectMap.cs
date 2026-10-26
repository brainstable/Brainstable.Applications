using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла СУБЪЕКТ.csv со свойствами <see cref="SubjectDto"/>.
/// Часть колонок читается по индексам, так как порядок полей в выгрузке фиксирован.
/// </summary>
public class SubjectMap : ClassMap<SubjectDto>
{
    public SubjectMap()
    {
        // Номер и наименование субъекта. FgisId в текущей реализации читается из той же колонки 0.
        Map(m => m.Id).Index(0).Name("Н_СУБЪЕКТА");
        Map(m => m.FgisId).Index(0).Name("Н_СУБЪЕКТА");
        Map(m => m.Name).Index(1).Name("И_СУБЪЕКТА");

        // Реквизиты субъекта; в бюллетене не выводятся.
        Map(m => m.Inn).Index(10).Name("ИНН");
        Map(m => m.Snils).Index(11).Name("СНИЛС");
    }
}
