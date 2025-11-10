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
    
    /// <summary>
    /// Gets the list of available models.
    /// </summary>
    /// <returns>The list of available models.</returns>
    IReadOnlyList<string> GetModelList();
    
    /// <summary>
    /// Selects the model to use.
    /// </summary>
    /// <param name="modelName">The name of the model to use.</param>
    /// <exception cref="ArgumentException">
    /// If the model cannot be found with the specified name.
    /// </exception>
    void SelectModel(string modelName);
}
