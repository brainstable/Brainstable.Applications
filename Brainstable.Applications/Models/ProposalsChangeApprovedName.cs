namespace Brainstable.Applications.Models;

/// <summary>
/// 2.3 Предложения по изменению одобренных названий
/// </summary>
public class ProposalsChangeApprovedName 
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string PreviousVarietyName { get; set; }
    public string ProposedVarietyName { get; set; }
    public string SelectionNumber { get; set; }
    public DateTime DateProposed { get; set; }
    public DateTime DateChanged { get; set; }
    public string ApplicatorIds { get; set; }
    public int PatentNumber { get; set; }
}