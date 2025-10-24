namespace CodeForge3.PokerFace.Enums;

/// <summary>
/// The enumeration of the possible card combinations.
/// </summary>
public enum ECardCombination
{
    /// <summary>
    /// Basic card combination.
    /// </summary>
    HighCard,

    /// <summary>
    /// Card combination of two matching ranks.
    /// </summary>
    Pair,

    /// <summary>
    /// Card combination of double matching ranks. (Two pairs.)
    /// </summary>
    TwoPair,

    /// <summary>
    /// Card combination of three matching ranks.
    /// </summary>
    ThreeOfAKind,

    /// <summary>
    /// Card combination of five consecutive ranks.
    /// </summary>
    Straight,

    /// <summary>
    /// Card combination of five identical suits.
    /// </summary>
    Flush,

    /// <summary>
    /// Card combination of two and three matching ranks. (A pair and a three of a kind.)
    /// </summary>
    FullHouse,

    /// <summary>
    /// Card combination of four matching ranks. 
    /// </summary>
    FourOfAKind,

    /// <summary>
    /// Card combination of five consecutive ranks with five identical suits. (Straight and flush.)
    /// </summary>
    StraightFlush,

    /// <summary>
    /// Card combination of 10, Jack, Queen, King and Ace with identical suits.
    /// </summary>
    RoyalFlush
}