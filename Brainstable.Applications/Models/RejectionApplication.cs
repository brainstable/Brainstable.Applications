namespace Brainstable.Applications.Models;

/// <summary>
/// 4.6 Отклоненные заявки на выдачу патента
/// </summary>
public class RejectionApplication
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

    public DateTime DateAdguardRejection { get; set; }
    public int CauseWithdrawal { get; set; }
}