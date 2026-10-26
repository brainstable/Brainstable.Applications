namespace Brainstable.Applications.Dto;

/// <summary>
/// Связь сорта с автором: одна строка таблицы СОРТ_АВТОР.
/// Авторы выводятся в колонке «Заявитель/Автор» с префиксом «б:».
/// </summary>
public record VarietyAutorDto
{
    /// <summary>Идентификатор сорта (К_СОРТА).</summary>
    public int VarietyId { get; set; }

    /// <summary>Идентификатор субъекта-автора (Н_СУБЪЕКТА).</summary>
    public int AutorId { get; set; }
}
