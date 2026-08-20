using Brainstable.Applications.Dto;
using Brainstable.Applications.Infrastructure.Reader;
using Brainstable.Applications.Services.Interfaces;

namespace Brainstable.Applications.Services.Implementations;

public class DataObjectFactory : IDataObjectFactory
{
    private readonly string _dataDirectory;

    public DataObjectFactory(string dataDirectory = "data/sbaza/")
    {
        _dataDirectory = dataDirectory;
    }

    private string GetFilePath(string fileName) => Path.Combine(_dataDirectory, fileName);

    public List<ApplicationDto> CreateApplicationDtos()
    {
        return LoaderCsv.LoadApplicaitons(GetFilePath(DataFileNames.Applications));
    }

    public Dictionary<int, VarietyDto> CreateVarietyDtos()
    {
        return LoaderCsv.LoadVarieties(GetFilePath(DataFileNames.Varieties))
            .ToDictionary(x => x.VarietyId);
    }

    public Dictionary<int, BlockDto> CreateBlockDtos()
    {
        return LoaderCsv.LoadBlocks(GetFilePath(DataFileNames.Blocks))
            .ToDictionary(x => x.Id);
    }

    public Dictionary<int, KindDto> CreateKindDtos()
    {
        return LoaderCsv.LoadKinds(GetFilePath(DataFileNames.Kinds))
            .ToDictionary(x => x.Id);
    }

    public List<ApplicationApplicatorDto> CreateApplicationApplicatorDtos()
    {
        return LoaderCsv.LoadApplicationsApplicators(GetFilePath(DataFileNames.ApplicationApplicators));
    }

    public List<VarietyAutorDto> CreateVarietyAutorDtos()
    {
        return LoaderCsv.LoadVarietiesAutors(GetFilePath(DataFileNames.VarietyAutors));
    }

    public List<VarietyNameApprovedDto> CreateVarietyNameApprovedDtos()
    {
        return LoaderCsv.LoadVarietyNameApprovedDtos(GetFilePath(DataFileNames.VarietySynonyms));
    }

    public Dictionary<int, PatentDto> CreatePatentDtos()
    {
        return LoaderCsv.LoadPatentDtos(GetFilePath(DataFileNames.Patents))
            .ToDictionary(x => x.VarietyId);
    }

    public List<BlockClassDto> CreateBlockClassDtos()
    {
        return LoaderCsv.LoadFromCsvFile(GetFilePath(DataFileNames.BlockClasses));
    }

    public List<SubjectChangeDto> CreateSubjectChangeDtos()
    {
        return LoaderCsv.LoadSubjectChangeDtos(GetFilePath(DataFileNames.SubjectChanges));
    }

    public Dictionary<int, SubjectDto> CreateSubjectDtos()
    {
        return LoaderCsv.LoadSubjects(GetFilePath(DataFileNames.Subjects)).ToDictionary(x => x.Id);
    }

    public List<LicenceDto> CreateLicenceDtos()
    {
        return LoaderCsv.LoadLicenceDtos(GetFilePath(DataFileNames.Licences));
    }

    public List<ContractDto> CreateContractDtos()
    {
        return LoaderCsv.LoadContractDtos(GetFilePath(DataFileNames.Contracts)).Where(x => !string.IsNullOrEmpty(x.PatentId) && x.WhomSubjectId.HasValue).ToList();
    }
}

public static class DataFileNames
{
    public const string Applications = "ЗАЯВКА.csv";
    public const string Varieties = "СОРТ.csv";
    public const string Blocks = "БЛОК_РЕЕСТРА.csv";
    public const string Kinds = "РОД.csv";
    public const string ApplicationApplicators = "ЗАЯВКА_ЗАЯВИТЕЛЬ.csv";
    public const string VarietyAutors = "СОРТ_АВТОР.csv";
    public const string VarietySynonyms = "СОРТ_СИНОНИМ.csv";
    public const string Patents = "СОРТ_ПАТЕНТ.csv";
    public const string BlockClasses = "БЛОК_КЛАССОВ.csv";
    public const string SubjectChanges = "АРХИВ_СД.csv";
    public const string Subjects = "СУБЪЕКТ.csv";

    public const string Contracts = "ДОГОВОР.csv";
    public const string Licences = "ЛИЦЕНЗИЯ.csv";
    public const string LicencesOpen = "ЛИЦЕНЗИЯ_ОТКРЫТА.csv";
}