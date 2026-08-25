using Brainstable.Applications.Dto;
using Brainstable.Applications.Models;
using Brainstable.Applications.Services.Interfaces;

namespace Brainstable.Applications.Services.Implementations;

internal class BuilderPatentFeatureService
{
    private readonly IDataObjectFactory _factory;

    private List<ApplicationDto> applicationDtos = [];
    private Dictionary<int, VarietyDto> varietyDtos = [];
    private Dictionary<int, BlockDto> blockDtos = [];
    private Dictionary<int, KindDto> kindDtos = [];
    private List<ApplicationApplicatorDto> applicationApplicatorDtos = [];
    private List<VarietyAutorDto> varietyAutorDtos = [];
    private HashSet<int> _subjectIds = [];
    HashSet<BlockClassVarietyName> _blockClassVarietyNames = [];

    private Dictionary<int, SubjectDto> subjectDtos = [];

    public BuilderPatentFeatureService(IDataObjectFactory factory)
    {
        _factory = factory;
    }

    public void Load()
    {
        applicationDtos = _factory.CreateApplicationDtos();
        varietyDtos = _factory.CreateVarietyDtos();
        blockDtos = _factory.CreateBlockDtos();
        kindDtos = _factory.CreateKindDtos();
        applicationApplicatorDtos = _factory.CreateApplicationApplicatorDtos();
        varietyAutorDtos = _factory.CreateVarietyAutorDtos();
        subjectDtos = _factory.CreateSubjectDtos();
    }

    private void AddSubjects(IEnumerable<int> subjectIds)
    {
        foreach (var subjectId in subjectIds)
        {
            _subjectIds.Add(subjectId);
        }
    }

    public List<BlockClassVarietyName> GetBlockClassVarietyNames()
    {
        return _blockClassVarietyNames.ToList();
    }

    // 
    public List<PatentFutureApplication> GetPatentFeatureApplications()
    {
        var list = new List<PatentFutureApplication>();

        var ll = applicationDtos.Where(x => !x.DateRejection.HasValue && !x.DateInclusion.HasValue && x.Type == 1).ToList();

        foreach (var dto in ll)
        {
            var aa = new PatentFutureApplication();

            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var autors = varietyAutorDtos.Where(x => x.VarietyId == dto.VarietyId).Select(x => x.AutorId).OrderBy(x => x);
            var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == dto.Id).OrderBy(x => x.Priority)
                .Select(x => x.ApplicatorId);

            aa.ApplicationAdguardId = dto.Id;
            aa.VarietyId = dto.VarietyId;
            aa.ApplicationAdguardId = dto.Id;
            aa.ApplicatorIds = string.Join(", ", applicators);
            aa.BlockName = block.Name;
            aa.BlockOrderNumber = block.OrderNumber;
            aa.KindName = kind.Name;
            aa.KindLatineName = kind.LatineName;
            aa.VarietyName = variety.VarietyName;
            aa.DateAdguardRegistration = dto.DateRegistration;
            aa.DateAdguardExpertise = dto.DateExpertise;
            aa.AutorIds = string.Join(", ", autors);

            var applicationNames = "";
            foreach (var id in applicators)
            {
                var subject = subjectDtos[id].Name;
                aa.ApplicatorNames.Add(subject);
            }

            foreach (var id in autors)
            {
                var subject = subjectDtos[id].Name;
                aa.AutorNames.Add(subject);
            }

            aa.AutorNames = aa.AutorNames.OrderBy(x => x).ToList();

            list.Add(aa);
        }

        return list;
    }
}