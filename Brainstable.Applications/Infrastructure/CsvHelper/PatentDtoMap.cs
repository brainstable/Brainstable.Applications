using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла СОРТ_ПАТЕНТ.csv со свойствами <see cref="PatentDto"/>.
/// </summary>
public class PatentDtoMap : ClassMap<PatentDto>
{
    public PatentDtoMap()
    {
        // Сорт и номер патента.
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.NumberPatent).Name("Н_ПАТЕНТА");

        // Период охраны: даты могут отсутствовать в выгрузке.
        Map(m => m.DateStart).Name("ДАТА_Н_ОХРАНЫ").Optional();
        Map(m => m.DateEnd).Name("ДАТА_К_ОХРАНЫ").Optional();
        Map(m => m.DateCalculateEnd).Name("CALC_END_DATE").Optional();
    }
}
