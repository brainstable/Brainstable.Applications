namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы СОРТ_ПАТЕНТ — патент на селекционное достижение.
/// По датам начала и окончания охраны строятся разделы II.2, II.3, II.4, IV.1 и VI.1.
/// </summary>
public class PatentDto
{
    /// <summary>Идентификатор сорта (К_СОРТА), он же ключ словаря патентов.</summary>
    public int VarietyId { get; set; }

    /// <summary>Дата начала действия охраны (ДАТА_Н_ОХРАНЫ), может отсутствовать.</summary>
    public DateTime DateStart { get; set; }

    /// <summary>Дата окончания действия охраны (ДАТА_К_ОХРАНЫ), может отсутствовать.</summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>Расчётная дата окончания охраны (CALC_END_DATE), в бюллетене не используется.</summary>
    public DateTime? DateCalculateEnd { get; set; }

    /// <summary>Номер патента (Н_ПАТЕНТА).</summary>
    public int NumberPatent { get; set; }
}
