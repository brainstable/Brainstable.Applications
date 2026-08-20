namespace Brainstable.Applications.Models;

public class TransferLicence
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int ApplicationId { get; set; }
    public int PatentNumber { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public int WhoId { get; set; }
    public int WhomId { get; set; }
    public DateTime Dateregistration { get; set; }
    public string NumberContract { get; set; }
    public int TypeContract { get; set; }
}