using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла БЛОК_РЕЕСТРА.csv со свойствами <see cref="BlockDto"/>.
/// Первые три колонки читаются по индексам; остальные (TOM_1..TOM_4) игнорируются.
/// </summary>
internal class BlockMap : ClassMap<BlockDto>
{
    public BlockMap()
    {
        // Номер, название и порядок вывода блока реестра.
        Map(m => m.Id).Index(0).Name("Н_БЛОКА_РЕЕСТРА");
        Map(m => m.Name).Index(1).Name("И_БЛОКА_РЕЕСТРА");
        Map(m => m.OrderNumber).Index(2).Name("Н_ПОРЯДКОВЫЙ");
    }
}
