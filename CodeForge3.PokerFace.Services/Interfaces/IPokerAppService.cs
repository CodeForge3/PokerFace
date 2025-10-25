using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace CodeForge3.PokerFace.Services.Interfaces;

/// <summary>
/// Defines the main application service.
/// </summary>
public interface IPokerAppService
{
    /// <summary>
    /// Predicts the cards in the uploaded image.
    /// </summary>
    /// <param name="file">The uploaded image file.</param>
    /// <returns>The list of predicted cards.</returns>
    /// <exception cref="ArgumentNullException">
    /// If the uploaded image file is <see langword="null" />.
    /// </exception>
    Task<IReadOnlyList<CardPrediction>> PredictCardsAsync(IBrowserFile? file);

    /// <summary>
    /// Evaluate the given cards as a poker card combination.
    /// </summary>
    /// <param name="cards">List of the cards.</param>
    /// <returns>Enumeration of the strongest card combination of the cards.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If not exactly five cards given. />.
    /// </exception>
    ECardCombination EvaluateCombination(IReadOnlyList<Card> cards);
}
