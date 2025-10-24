using CodeForge3.PokerFace.Entities;

namespace CodeForge3.PokerFace.Services.Interfaces;

/// <summary>
/// Defines the poker hand evaluation of five cards service.
/// </summary>
public interface IPokerCombinationEvaluatorService
{
    /// <summary>
    /// Evaluate the given cards as a poker combination.
    /// </summary>
    /// <param name="cards">List of the cards.</param>
    /// <returns>The strongest poker combination of the cards.</returns>
    string EvaluateCombination(IReadOnlyList<CardPrediction> cardPredictions);
}
