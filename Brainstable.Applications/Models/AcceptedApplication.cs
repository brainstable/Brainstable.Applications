namespace Brainstable.Applications.Models;

public class AcceptedApplication
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

    // г) оригинаторы
    //public string OriginatorIds { get; set; }
    public DateTime? DateAdguardRegistration { get; set; }
    public DateTime? DateAdguardExpertise { get; set; }
}