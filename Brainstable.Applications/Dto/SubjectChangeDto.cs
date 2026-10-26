namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы АРХИВ_СД — изменение состава субъектов селекционного достижения
/// (раздел V.1). Одна строка описывает одно изменение: замену, добавление или удаление.
/// </summary>
public record SubjectChangeDto
{
    /// <summary>Номер заявки, по которой произошло изменение (Н_ЗАЯВКИ).</summary>
    public int ApplicationId { get; set; }

    /// <summary>Тип заявки (Н_ТИПА_ЗАЯВКИ); в разделе V.1 используются только заявки типа 1.</summary>
    public int TypeApplication { get; set; }

    /// <summary>Идентификатор сорта (К_СОРТА).</summary>
    public int VarietyId { get; set; }

    /// <summary>Исключаемый субъект (Н_СУБ_СТАРЫЙ), может отсутствовать при добавлении.</summary>
    public int? OldSubjectId { get; set; }

    /// <summary>Тип субъекта (ТИП_СУБЪЕКТА); значение 5 в раздел V.1 не попадает.</summary>
    public int TypeSubject { get; set; }

    /// <summary>Причина изменения (П_ИЗМЕНЕНИЯ): 1 — замена, 2 — добавление, 3 — удаление.</summary>
    public int CauseChange { get; set; }

    /// <summary>Дата изменения состава (ДАТА_ИЗМЕНЕНИЯ).</summary>
    public DateTime DateChanged { get; set; }

    /// <summary>Включаемый субъект (Н_СУБ_НОВЫЙ), может отсутствовать при удалении.</summary>
    public int? NewSubjectId { get; set; }
}
