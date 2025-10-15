using CodeForge3.PokerFace.Entities;

namespace CodeForge3.PokerFace.MachineLearning.Interfaces;

/// <summary>
/// Defines a Yolo image detection handler.
/// </summary>
public interface IYoloDetectionHandler
    : IDisposable
{
    /// <summary>
    /// Detects the cards in the specified image asynchronously.
    /// </summary>
    /// <param name="imageBytes">The bytes of the image.</param>
    /// <returns>The list of detected cards.</returns>
    Task<IReadOnlyList<CardPrediction>> DetectAsync(byte[] imageBytes);
}
