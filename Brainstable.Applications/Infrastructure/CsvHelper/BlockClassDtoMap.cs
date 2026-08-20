using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

public class BlockClassDtoMap : ClassMap<BlockClassDto>
{
    public BlockClassDtoMap()
    {
        Map(m => m.Id).Name("Н_БЛОКА_КЛАССОВ");
        Map(m => m.Name).Name("И_БЛОКА_КЛАССОВ");
    }
}