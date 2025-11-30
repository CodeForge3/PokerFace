using SixLabors.ImageSharp;

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
    /// <param name="bounds">The bounding box of the prediction.</param>
    public CardPrediction(Card card, float probability, Rectangle bounds)
    {
        Card = card;
        Probability = probability;
        Bounds = bounds;
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// The card predicted.
    /// </summary>
    public Card Card { get; }
    
    /// <summary>
    /// The probability of the prediction being correct.
    /// </summary>
    public float Probability { get; }
    
    /// <summary>
    /// The bounding box of the prediction.
    /// </summary>
    public Rectangle Bounds { get; }
    
    #endregion
    
    #region ToString
    
    /// <inheritdoc />
    public override string ToString() => $"{Card} ({Probability:P0})";
    
    #endregion
}
