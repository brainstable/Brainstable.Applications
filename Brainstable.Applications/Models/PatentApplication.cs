namespace Brainstable.Applications.Models;

internal class PatentApplication
{
    public required string BlockName { get; set; }
    public required string KindName { get; set; }
    public required string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public required string VarietyName { get; set; }
    public string? SelectionNumber { get; set; }
    public int ApplicationAdmissionId { get; set; }
    public int ApplicationAdguardId { get; set; }

    // а) заявитель
    public int ApplicatorId { get; set; }
    public string ApplicatorName { get; set; }

    // б) авторы
    public string AutorIds { get; set; }

    // г) оригинаторы
    public string OriginatorIds { get; set; }

    public DateTime? DateAdguardWithdrawal { get; set; }
    public DateTime? DateAddmissionWithdrawal { get; set; }

    public DateTime? DateAdguardExpertise { get; set; }
    public DateTime? DateAddmissionExpertise { get; set; }
}