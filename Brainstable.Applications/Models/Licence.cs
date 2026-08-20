namespace Brainstable.Applications.Models;

public class Licence
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int ApplicationId { get; set; }
    public int PatentNumber { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string PatentownerIds { get; set; }
    public int? Version { get; set; }
    public List<LicenceItem> LicenceItems { get; set; } = [];
}

public class LicenceItem
{
    public int LicenseeId { get; set; }
    public DateTime DateRegistration { get; set; }
    public DateTime? DateAnnulation { get; set; }
    public DateTime? DateExpiration { get; set; }
    public string NumberLicence { get; set; }
    public string TypeLicence { get; set; }
}