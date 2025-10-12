namespace CodeForge3.PokerFace.Entities;

/// <summary>
/// Represents a regular playing card prediction.
/// </summary>
public readonly struct CardPrediction
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CardPrediction" /> struct.
    /// </summary>
    /// <param name="card">The card predicted.</param>
    /// <param name="probability">The probability of the prediction being correct.</param>
    public CardPrediction(Card card, float probability)
    {
        Card = card;
        Probability = probability;
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// The card predicted.
    /// </summary>
    private Card Card { get; }
    
    /// <summary>
    /// The probability of the prediction being correct.
    /// </summary>
    private float Probability { get; }
    
    #endregion
    
    #region ToString
    
    /// <inheritdoc />
    public override string ToString() => $"{Card} ({Probability:P0})";
    
    #endregion
}
