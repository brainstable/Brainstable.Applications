using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла БЛОК_КЛАССОВ.csv со свойствами <see cref="BlockClassDto"/>.
/// </summary>
public class BlockClassDtoMap : ClassMap<BlockClassDto>
{
    public BlockClassDtoMap()
    {
        // Номер и название блока классов.
        Map(m => m.Id).Name("Н_БЛОКА_КЛАССОВ");
        Map(m => m.Name).Name("И_БЛОКА_КЛАССОВ");
    }
}
