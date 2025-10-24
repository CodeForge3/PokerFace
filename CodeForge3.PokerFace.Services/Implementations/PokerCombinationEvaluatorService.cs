using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.Enums;
using CodeForge3.PokerFace.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CodeForge3.PokerFace.Services.Implementations;

/// <summary>
/// The implementation of the <see cref="IPokerCombinationEvaluatorService"/>.
/// </summary>
public class PokerCombinationEvaluatorService : IPokerCombinationEvaluatorService
{
    #region Fields

    /// <summary>
    /// The field containing the logger for the class.
    /// </summary>
    private readonly ILogger<PokerCombinationEvaluatorService> _logger;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PokerCombinationEvaluatorService"/> class.
    /// </summary>
    /// <param name="logger">The logger for the class.</param>
    public PokerCombinationEvaluatorService(ILogger<PokerCombinationEvaluatorService> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Evaluate

    /// inheritdoc />
    public string EvaluateCombination(IReadOnlyList<Card> cards)
    {
        if (cards.Count != 5)
            throw new ArgumentException("Prediction must have exactly 5 cards.");


        var rankCounts = Enum.GetValues<ECardRank>().ToDictionary(r => r, _ => 0);
        var suitCounts = Enum.GetValues<ECardSuit>().ToDictionary(s => s, _ => 0);
        int rankMask = 0;

        foreach (var card in cards)
        {
            rankCounts[card.Rank]++;
            suitCounts[card.Suit]++;
            rankMask |= 1 << ((int)card.Rank - 2); // -2 is because of the Enum Rank Two, which is the first element starts at 2.
        }


        bool flush = suitCounts.Values.Any(c => c == 5);


        var straightBitMask = 0b11111;
        bool straight = Enumerable.Range(0, 9).Any(shift => ((rankMask >> shift) & straightBitMask) == straightBitMask);

        var lowAceStraightBitMask = 0b1000000001111; // (A-2-3-4-5)
        if (!straight && (rankMask & lowAceStraightBitMask) == lowAceStraightBitMask)
            straight = true;


        int four = rankCounts.Values.Count(c => c == 4);
        int three = rankCounts.Values.Count(c => c == 3);
        int pairs = rankCounts.Values.Count(c => c == 2);


        if (straight && flush)
        {
            var royalFlushBitMask = 0b1111100000000;
            if ((rankMask & royalFlushBitMask) == royalFlushBitMask)
                return "Royal Flush";

            return "Straight Flush";
        }

        if (four == 1) { return "Four of a kind"; }

        if (three == 1 && pairs == 1) { return "Full House"; }

        if (flush) { return "Flush"; }

        if (straight) { return "Straight"; }

        if (three == 1) { return "Three of a kind"; }

        if (pairs == 2) { return "Two Pair"; }

        if (pairs == 1) { return "Pair"; }

        return "High Card";
    }

    #endregion
}

