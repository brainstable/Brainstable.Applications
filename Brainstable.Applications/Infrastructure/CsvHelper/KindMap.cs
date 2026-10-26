using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла РОД.csv со свойствами <see cref="KindDto"/>.
/// </summary>
internal class KindMap : ClassMap<KindDto>
{
    public KindMap()
    {
        // Идентификация рода, его названия и привязка к блоку реестра.
        Map(m => m.Id).Name("Н_РОДА");
        Map(m => m.Name).Name("И_РОДА");
        Map(m => m.LatineName).Name("И_РОДА_ЛАТ");
        Map(m => m.BlockId).Name("Н_БЛОКА_РЕЕСТРА");

        // Блок классов для сводной таблицы II.5 может быть не заполнен.
        Map(m => m.ClassId).Name("Н_БЛОКА_КЛАССОВ").Optional();
    }
}
