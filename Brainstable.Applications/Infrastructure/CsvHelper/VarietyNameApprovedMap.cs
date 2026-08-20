using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

public class VarietyNameApprovedMap : ClassMap<VarietyNameApprovedDto>
{
    public VarietyNameApprovedMap()
    {
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.ApprovedName).Name("И_СОРТА");
        Map(m => m.DateChanged).Name("ДАТА_ИЗМЕНЕНИЯ").Optional();
        Map(m => m.DateApproved).Name("ДАТА_ОДОБРЕНИЯ").Optional();
    }
}