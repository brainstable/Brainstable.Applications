namespace Brainstable.Applications.Models;

/// <summary>
/// Отозванные заявки
/// </summary>
public class WithdrawnApplication
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string SelectionNumber { get; set; }
    public int ApplicationAdguardId { get; set; }
    // а) заявитель
    public string ApplicatorIds { get; set; }

    // б) авторы
    public string AutorIds { get; set; }

    public DateTime DateAdguardWithdrawal { get; set; }
    public int CauseWithdrawal { get; set; }
}