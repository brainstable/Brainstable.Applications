using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла СОРТ_АВТОР.csv со свойствами <see cref="VarietyAutorDto"/>.
/// </summary>
internal class VarietyAutorMap : ClassMap<VarietyAutorDto>
{
    public VarietyAutorMap()
    {
        // К_СОРТА -> VarietyId
        Map(m => m.VarietyId).Name("К_СОРТА");

        // Н_СУБЪЕКТА -> AutorId; колонка Н_ИА игнорируется.
        Map(m => m.AutorId).Name("Н_СУБЪЕКТА");
    }
}
