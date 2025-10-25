using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CodeForge3.PokerFace.Enums;

/// <summary>
/// The enumeration of the possible card combinations.
/// </summary>
public enum ECardCombination
{
    /// <summary>
    /// Basic card combination.
    /// </summary>
    [Display(Name = "High card")]
    HighCard,

    /// <summary>
    /// Card combination of two matching ranks.
    /// </summary>
    [Display(Name = "Pair")]
    Pair,

    /// <summary>
    /// Card combination of double matching ranks. (Two pair.)
    /// </summary>
    [Display(Name = "Two pair")]
    TwoPair,

    /// <summary>
    /// Card combination of three matching ranks.
    /// </summary>
    [Display(Name = "Three of a kind")]
    ThreeOfAKind,

    /// <summary>
    /// Card combination of five consecutive ranks.
    /// </summary>
    [Display(Name = "Straight")]
    Straight,

    /// <summary>
    /// Card combination of five identical suits.
    /// </summary>
    [Display(Name = "Flush")]
    Flush,

    /// <summary>
    /// Card combination of two and three matching ranks. (A pair and a three of a kind.)
    /// </summary>
    [Display(Name = "Full House")]
    FullHouse,

    /// <summary>
    /// Card combination of four matching ranks. 
    /// </summary>
    [Display(Name = "Four of a kind")]
    FourOfAKind,

    /// <summary>
    /// Card combination of five consecutive ranks with five identical suits. (Straight and flush.)
    /// </summary>
    [Display(Name = "Straight Flush")]
    StraightFlush,

    /// <summary>
    /// Card combination of 10, Jack, Queen, King and Ace with identical suits.
    /// </summary>
    [Display(Name = "Royal Flush")]
    RoyalFlush
}