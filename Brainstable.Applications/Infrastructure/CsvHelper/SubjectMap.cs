using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

internal class SubjectMap : ClassMap<SubjectDto>
{
    public SubjectMap()
    {
        Map(m => m.Id).Index(0).Name("Н_СУБЪЕКТА");
        Map(m => m.FgisId).Index(0).Name("Н_СУБЪЕКТА"); // или игнорируйте
        Map(m => m.Name).Index(1).Name("И_СУБЪЕКТА");
        Map(m => m.Inn).Index(10).Name("ИНН");
        Map(m => m.Snils).Index(11).Name("СНИЛС");
    }
}