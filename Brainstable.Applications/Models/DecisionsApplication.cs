namespace Brainstable.Applications.Models;

/// <summary>
/// Сорта и гибриды, включенные в Государственный реестр охраняемых селекционных достижений, т.е. полученные патенты
/// </summary>
public class DecisionsApplication
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string SelectionNumber { get; set; }
    public int ApplicationAdguardId { get; set; }
    public string ApplicatorIds { get; set; }

    public string AutorIds { get; set; }

    public DateTime DateRegistrationPatent { get; set; }
    public int NumberPatent { get; set; }
}