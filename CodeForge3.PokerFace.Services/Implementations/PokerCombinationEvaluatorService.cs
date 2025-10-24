using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.Enums;
using CodeForge3.PokerFace.Services.Interfaces;

namespace CodeForge3.PokerFace.Services.Implementations;

/// <summary>
/// The implementation of the <see cref="IPokerCombinationEvaluatorService"/>.
/// </summary>
public class PokerCombinationEvaluatorService : IPokerCombinationEvaluatorService
{
    public string EvaluateCombination(IReadOnlyList<CardPrediction> cardPredictions)
    {
        if (cardPredictions.Count != 5)
            throw new ArgumentException("Prediction must have exactly 5 cards.");

        List<Card> cards = new List<Card>();
        for (int i = 0; i < cardPredictions.Count; i++)
        {
            cards[i] = cardPredictions[i].Card;
        }

        int[] rankCount = new int[13];
        int[] suitCount = new int[4];
        int rankMask = 0;

        foreach (var card in cards)
        {
            rankCount[(int)card.Rank]++;
            suitCount[(int)card.Suit]++;
            rankMask |= 1 << (int)card.Rank;
        }

        bool flush = suitCount.Any(c => c == 5);
        bool straight = false;
        int straightHigh = -1;

        // Straight check: normal straights
        for (int i = 0; i <= 8; i++)
        {
            int mask = 0b11111 << i;
            if ((rankMask & mask) == mask)
            {
                straight = true;
                straightHigh = i + 4;
                break;
            }
        }
        // Low-Ace straight (A-2-3-4-5)
        if (!straight && (rankMask & 0b1000000001111) == 0b1000000001111)
        {
            straight = true;
            straightHigh = 3; // 5 is high card
        }

        int four = rankCount.Count(c => c == 4);
        int three = rankCount.Count(c => c == 3);
        int pairs = rankCount.Count(c => c == 2);

        if (straight && flush)
        {
            var a = ECardCombination.RoyalFlush.GetType().ToString();
            if (straightHigh == (int)ECardRank.Ace)
                return ECardCombination.RoyalFlush.GetType().ToString();
            return ECardCombination.StraightFlush.GetType().ToString();
        }
        if (four == 1) return ECardCombination.FourOfAKind.GetType().ToString();
        if (three == 1 && pairs == 1) return ECardCombination.FullHouse.GetType().ToString();
        if (flush) return ECardCombination.Flush.GetType().ToString();
        if (straight) return ECardCombination.Straight.GetType().ToString();
        if (three == 1) return ECardCombination.ThreeOfAKind.GetType().ToString();
        if (pairs == 2) return ECardCombination.TwoPair.GetType().ToString();
        if (pairs == 1) return ECardCombination.Pair.GetType().ToString();

        return ECardCombination.HighCard.GetType().ToString();
    }
}

