namespace Brainstable.Applications.Dto;

/// <summary>
/// Блок реестра (таблица БЛОК_РЕЕСТРА) — верхний уровень группировки
/// в разделах бюллетеня (например, зерновые, овощные и т. п.).
/// </summary>
public record BlockDto
{
    /// <summary>Номер блока реестра (Н_БЛОКА_РЕЕСТРА).</summary>
    public int Id { get; set; }

    /// <summary>Название блока реестра (И_БЛОКА_РЕЕСТРА).</summary>
    public string Name { get; set; }

    /// <summary>Порядковый номер блока: задаёт порядок вывода блоков в документе (Н_ПОРЯДКОВЫЙ).</summary>
    public int OrderNumber { get; set; }
}
