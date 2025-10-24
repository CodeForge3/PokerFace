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
    /// <summary>
    /// The field containing the logger for the class.
    /// </summary>
    private readonly ILogger<PokerCombinationEvaluatorService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PokerCombinationEvaluatorService"/> class.
    /// </summary>
    /// <param name="logger">The logger for the class.</param>
    public PokerCombinationEvaluatorService(ILogger<PokerCombinationEvaluatorService> logger)
    {
        _logger = logger;
    }

    public string EvaluateCombination(IReadOnlyList<CardPrediction> cardPredictions)
    {
        if (cardPredictions.Count != 5)
            throw new ArgumentException("Prediction must have exactly 5 cards.");

        List<Card> cards = [];
        foreach (var cardPrediction in cardPredictions)
        {
            cards.Add(cardPrediction.Card);
            _logger.LogInformation($"-{cardPrediction.Card}-");
        }

        var rankCounts = Enum.GetValues<ECardRank>().ToDictionary(r => r, _ => 0);
        var suitCounts = Enum.GetValues<ECardSuit>().ToDictionary(s => s, _ => 0);
        int rankMask = 0;

        foreach (var card in cards)
        {
            rankCounts[card.Rank]++;
            suitCounts[card.Suit]++;
            rankMask |= 1 << ((int)card.Rank - 2); // -2 is because of the Enum Rank Two, which is the first element starts at 2.
        }

        bool flush = false;
        foreach (var (_, count) in suitCounts)
            if (count == 5)
                flush = true;

        bool straight = false;
        for (var shift = 0; shift <= 8; shift++)
        {
            var straightBitMask = 0b11111 << shift;
            if ((rankMask & straightBitMask) == straightBitMask)
            {
                straight = true;
                break;
            }
        }

        var lowAceStraightBitMask = 0b1000000001111; // (A-2-3-4-5)
        if (!straight && (rankMask & lowAceStraightBitMask) == lowAceStraightBitMask)
            straight = true;

        int four = 0;
        int three = 0;
        int pairs = 0;
        foreach (var (_, count) in rankCounts)
        {
            if (count == 4) { four++; }
            if (count == 3) { three++; }
            if (count == 2) { pairs++; }
        }


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
}

