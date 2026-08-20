namespace Brainstable.Applications.Dto;

public class ContractDto
{
    public int ApplicationId { get; set; }
    public string PatentId { get; set; }
    public DateTime DateRegistration { get; set; }
    public int TypeLicence { get; set; }
    public string ContractNumber { get; set; }
    public string Conditions { get; set; }
    public int? WhoSubjectId { get; set; }
    public int? WhomSubjectId { get; set; }
    public DateTime? DateAnnulation { get; set; }
    public DateTime? DateExpiration { get; set; }
    public int VarietyId { get; set; }
}