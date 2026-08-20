using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class VarietyAutorMap : ClassMap<VarietyAutorDto>
{
    public VarietyAutorMap()
    {
        // К_СОРТА -> VarietyId
        Map(m => m.VarietyId).Name("К_СОРТА");

        // Н_СУБЪЕКТА -> AutorId
        Map(m => m.AutorId).Name("Н_СУБЪЕКТА");

        // Н_ИА - игнорируем
    }
}