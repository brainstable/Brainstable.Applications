namespace Brainstable.Applications.Dto;

public class LicenceDto
{
    public int ApplicationId { get; set; }
    public int PatentId { get; set; }
    public DateTime DateRegistration { get; set; }
    public string NumberLicence { get; set; }
    public int TypeLicence { get; set; }
    public int VarietyId { get; set; }
    public DateTime? DateAnnulation { get; set; }
    public DateTime? DateExpiration { get; set; }
    public int SubjectId { get; set; }
    public int? Version { get; set; }
}