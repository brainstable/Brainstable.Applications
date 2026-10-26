namespace Brainstable.Applications.Dto;

/// <summary>
/// Род/вид растения (таблица РОД) — средний уровень группировки в разделах бюллетеня.
/// </summary>
public record KindDto
{
    /// <summary>Номер рода (Н_РОДА).</summary>
    public int Id { get; set; }

    /// <summary>Русское название рода (И_РОДА).</summary>
    public string Name { get; set; }

    /// <summary>Латинское название рода (И_РОДА_ЛАТ).</summary>
    public string LatineName { get; set; }

    /// <summary>Номер блока реестра, к которому относится род (Н_БЛОКА_РЕЕСТРА).</summary>
    public int BlockId { get; set; }

    /// <summary>Номер блока классов (Н_БЛОКА_КЛАССОВ), может отсутствовать.</summary>
    public int? ClassId { get; set; }
}
