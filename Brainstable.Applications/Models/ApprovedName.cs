namespace Brainstable.Applications.Models;

/// <summary>
/// Одобренное название впервые включенного в реестр на охрану
/// </summary>
public class ApprovedName
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string ApplicatorIds { get; set; }
}