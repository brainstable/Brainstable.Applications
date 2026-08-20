using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class VarietyMap : ClassMap<VarietyDto>
{
    public VarietyMap()
    {
        // К_СОРТА -> VarietyId
        Map(m => m.VarietyId).Name("К_СОРТА");

        // И_СОРТА -> VarietyName
        Map(m => m.VarietyName).Name("И_СОРТА");

        // Н_РОДА -> KindId
        Map(m => m.KindId).Name("Н_РОДА");

        // И_СОРТА_ЛАТ -> LatineName
        Map(m => m.LatineName).Name("И_СОРТА_ЛАТ");

        // Н_СЕЛЕКЦИОННЫЙ -> SelectionNumber
        Map(m => m.SelectionNumber).Name("Н_СЕЛЕКЦИОННЫЙ").Optional();

        // Остальные поля (И_СОРТА_20, CATEGORY, ШИФР) игнорируем
    }
}