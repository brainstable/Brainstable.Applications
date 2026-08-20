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

    //// +) заявка опубликована ранее
    //public bool PublicationAddmissionEarlier { get; set; }
    //public bool PublicationAdguardEarlier { get; set; }

    //// ++) заявка не подана
    //public bool NotAddmissionSubmitted { get; set; }
    //public bool NotAdguardSubmitted { get; set; }

    //// *) Предоставляется временная правовая охрана?
    //public int ApplicationAdmissionId { get; set; }
    //public DateTime? DateAddmissionRegistration { get; set; }

    //public DateTime? DateAddmissionExpertise { get; set; }
}