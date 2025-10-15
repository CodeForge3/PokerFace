using CodeForge3.PokerFace.Enums;

namespace CodeForge3.PokerFace.Entities;

/// <summary>
/// Represents a regular playing card.
/// </summary>
/// <param name="Rank">The rank of the card.</param>
/// <param name="Suit">The suit of the card.</param>
public readonly record struct Card(ECardRank Rank, ECardSuit Suit)
{
    #region ToString
    
    /// <inheritdoc />
    public override string ToString() => $"{Rank} of {Suit}";
    
    #endregion
}
