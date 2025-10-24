using CodeForge3.PokerFace.Entities;

namespace CodeForge3.PokerFace.Services.Interfaces;

/// <summary>
/// Defines the service of the combination evaluator.
/// </summary>
public interface ICardCombinationEvaluatorService
{
    /// <summary>
    /// Evaluate the given cards as a poker card combination.
    /// </summary>
    /// <param name="cards">List of the cards.</param>
    /// <returns>String representation of the strongest card combination of the cards.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If not exactly five cards given. />.
    /// </exception>
    string EvaluateCombination(IReadOnlyList<Card> cards);
}
