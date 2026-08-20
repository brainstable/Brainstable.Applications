using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class KindMap : ClassMap<KindDto>
{
    public KindMap()
    {
        Map(m => m.Id).Name("Н_РОДА");
        Map(m => m.Name).Name("И_РОДА");
        Map(m => m.LatineName).Name("И_РОДА_ЛАТ");
        Map(m => m.BlockId).Name("Н_БЛОКА_РЕЕСТРА");
        Map(m => m.ClassId).Name("Н_БЛОКА_КЛАССОВ").Optional(); // Может быть пустым
    }
}