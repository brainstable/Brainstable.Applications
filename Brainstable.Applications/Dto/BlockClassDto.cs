namespace Brainstable.Applications.Dto;

/// <summary>
/// Блок классов (таблица БЛОК_КЛАССОВ) — укрупнённая группа культур,
/// по которой строится сводная таблица II.5.
/// </summary>
public class BlockClassDto
{
    /// <summary>Номер блока классов (Н_БЛОКА_КЛАССОВ).</summary>
    public int Id { get; set; }

    /// <summary>Название блока классов (И_БЛОКА_КЛАССОВ).</summary>
    public string Name { get; set; }
}
