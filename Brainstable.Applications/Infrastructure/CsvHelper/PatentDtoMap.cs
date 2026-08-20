using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

public class PatentDtoMap : ClassMap<PatentDto>
{
    public PatentDtoMap()
    {
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.DateStart).Name("ДАТА_Н_ОХРАНЫ")
            .Optional();
        Map(m => m.NumberPatent).Name("Н_ПАТЕНТА");
        Map(m => m.DateEnd).Name("ДАТА_К_ОХРАНЫ")
            .Optional();
        Map(m => m.DateCalculateEnd).Name("CALC_END_DATE")
            .Optional();
    }
}