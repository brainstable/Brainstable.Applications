using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class ApplicationApplicatorMap : ClassMap<ApplicationApplicatorDto>
{
    public ApplicationApplicatorMap()
    {
        // Н_ЗАЯВКИ -> ApplicationId
        Map(m => m.ApplicationId).Name("Н_ЗАЯВКИ");

        // Н_СУБЪЕКТА -> ApplicatorId
        Map(m => m.ApplicatorId).Name("Н_СУБЪЕКТА");

        // ПРИОРИТЕТ -> Priority
        Map(m => m.Priority).Name("ПРИОРИТЕТ");
    }
}