namespace Brainstable.Applications.Models;

public class ProtectionEndApplication
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public int PatentId { get; set; }
    public string VarietyName { get; set; }
    public string SelectionNumber { get; set; }

    public int ApplicationAdguardId { get; set; }
    public string ApplicatorIds { get; set; }
    public DateTime? DateAdguardEnd { get; set; }
    public int CauseEnd { get; set; }
}