using Brainstable.Applications.Dto;

namespace Brainstable.Applications.Services.Interfaces;

public interface IDataObjectFactory
{
    List<ApplicationDto> CreateApplicationDtos();
    Dictionary<int, VarietyDto> CreateVarietyDtos();
    Dictionary<int, BlockDto> CreateBlockDtos();
    Dictionary<int, KindDto> CreateKindDtos();
    List<ApplicationApplicatorDto> CreateApplicationApplicatorDtos();
    List<VarietyAutorDto> CreateVarietyAutorDtos();
    List<VarietyNameApprovedDto> CreateVarietyNameApprovedDtos();
    Dictionary<int, PatentDto> CreatePatentDtos();
    List<BlockClassDto> CreateBlockClassDtos();
    List<SubjectChangeDto> CreateSubjectChangeDtos();
    Dictionary<int, SubjectDto> CreateSubjectDtos();
    List<LicenceDto> CreateLicenceDtos();
    List<ContractDto> CreateContractDtos();
}