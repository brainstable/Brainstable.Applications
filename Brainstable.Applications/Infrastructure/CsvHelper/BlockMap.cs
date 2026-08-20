using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class BlockMap : ClassMap<BlockDto>
{
    public BlockMap()
    {
        Map(m => m.Id).Index(0).Name("Н_БЛОКА_РЕЕСТРА");
        Map(m => m.Name).Index(1).Name("И_БЛОКА_РЕЕСТРА");
        Map(m => m.OrderNumber).Index(2).Name("Н_ПОРЯДКОВЫЙ");
        // Остальные колонки (TOM_1, TOM_2, TOM_3, TOM_4) игнорируем
    }
}