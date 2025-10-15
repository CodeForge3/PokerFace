using CodeForge3.PokerFace.Configurations;
using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.MachineLearning.Interfaces;
using CodeForge3.PokerFace.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace CodeForge3.PokerFace.Services.Implementations;

/// <summary>
/// The implementation of the <see cref="IPokerAppService" />.
/// </summary>
public sealed class PokerAppService
    : IPokerAppService
{
    #region Fields
    
    /// <summary>
    /// The field containing the logger for the class.
    /// </summary>
    private readonly ILogger<PokerAppService> _logger;
    
    /// <summary>
    /// The field containing the Yolo image detection handler.
    /// </summary>
    private readonly IYoloDetectionHandler _yoloDetectionHandler;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PokerAppService" /> class.
    /// </summary>
    /// <param name="logger">The logger for the class.</param>
    /// <param name="yoloDetectionHandler">The Yolo image detection handler.</param>
    public PokerAppService(ILogger<PokerAppService> logger, IYoloDetectionHandler yoloDetectionHandler)
    {
        _logger = logger;
        _yoloDetectionHandler = yoloDetectionHandler;
    }
    
    #endregion
    
    #region PredictCards
    
    /// <inheritdoc />
    public async Task<IReadOnlyList<CardPrediction>> PredictCardsAsync(IBrowserFile? file)
    {
        _logger.LogDebug("Starting card prediction.");
        
        if (file == null)
        {
            _logger.LogWarning("The file is null.");
            throw new ArgumentNullException(nameof(file));
        }
        
        await using Stream stream = file.OpenReadStream(PokerFaceConfiguration.MaxUploadFileSize);
        using MemoryStream ms = new();
        await stream.CopyToAsync(ms);
        byte[] imageBytes = ms.ToArray();
        
        IReadOnlyList<CardPrediction> predictions = await _yoloDetectionHandler.DetectAsync(imageBytes);
        
        _logger.LogInformation("Card prediction completed.");
        return predictions;
    }
    
    #endregion
}
