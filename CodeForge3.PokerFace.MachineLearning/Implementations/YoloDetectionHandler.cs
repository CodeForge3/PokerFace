using System.Reflection;
using CodeForge3.PokerFace.Configurations;
using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.Enums;
using CodeForge3.PokerFace.MachineLearning.Interfaces;
using Compunet.YoloSharp;
using Compunet.YoloSharp.Data;
using Microsoft.Extensions.Logging;

namespace CodeForge3.PokerFace.MachineLearning.Implementations;

/// <summary>
/// The implementation of the <see cref="IYoloDetectionHandler" />.
/// </summary>
public sealed class YoloDetectionHandler
    : IYoloDetectionHandler
{
    #region Constants
    
    /// <summary>
    /// The message returned when the assembly could not be found.
    /// </summary>
    private const string AssemblyNotFoundMessage = "The 'CodeForge3.PokerFace.MachineLearning' " +
        "assembly could not be found.";
    
    /// <summary>
    /// The message returned when the label format is not correct.
    /// </summary>
    private const string InvalidLabelFormatMessage = "The '{0}' label format is invalid.";
    
    /// <summary>
    /// The pattern used to match model files.
    /// </summary>
    private const string ModelFilePattern = "*.onnx";
    
    /// <summary>
    /// The name of the folder containing the models.
    /// </summary>
    private const string ModelsFolder = "Models";
    
    #endregion
    
    #region Fields
    
    /// <summary>
    /// The field containing the assembly directory.
    /// </summary>
    private static readonly Lazy<string> AssemblyDirectory = new(InitAssemblyDirectory);
    
    /// <summary>
    /// The field containing the logger for the class.
    /// </summary>
    private readonly ILogger<YoloDetectionHandler> _logger;
    
    /// <summary>
    /// The field containing the Yolo predictor.
    /// </summary>
    private YoloPredictor? _yoloPredictor;
    
    /// <summary>
    /// The field containing the flag indicating whether
    /// the handler has been disposed of.
    /// </summary>
    private bool _isDisposed;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="YoloDetectionHandler" /> class,
    /// using the specified model name.
    /// </summary>
    /// <param name="logger">The logger for the class.</param>
    public YoloDetectionHandler(ILogger<YoloDetectionHandler> logger)
    {
        _logger = logger;
        _yoloPredictor = null;
        _isDisposed = false;
    }
    
    #endregion
    
    #region InitAssemblyDirectory
    
    /// <summary>
    /// Initializes the assembly directory.
    /// </summary>
    /// <returns>The current assembly directory.</returns>
    /// <exception cref="DirectoryNotFoundException">
    /// If the assembly directory could not be found.
    /// </exception>
    private static string InitAssemblyDirectory()
    {
        Assembly currentAssembly = typeof(YoloDetectionHandler).Assembly;
        string assemblyDirectory = Path.GetDirectoryName(currentAssembly.Location)
            ?? throw new DirectoryNotFoundException(AssemblyNotFoundMessage);
        return assemblyDirectory;
    }
    
    #endregion
    
    #region ParseLabel
    
    /// <summary>
    /// Converts the label to a <see cref="Card" /> object.
    /// </summary>
    /// <param name="label">The label to convert.</param>
    /// <returns>The <see cref="Card" /> representation of the label.</returns>
    /// <exception cref="ArgumentException">
    /// If the label is not in the correct format.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the label cannot be parsed into a <see cref="Card" />.
    /// </exception>
    private static Card ParseLabel(string label)
    {
        if (string.IsNullOrWhiteSpace(label) || label.Length < 2)
        {
            throw new ArgumentException(
                string.Format(InvalidLabelFormatMessage, label),
                nameof(label)
            );
        }
        
        string rankPart = label[..^1];
        char suitChar = label[^1];
        
        ECardRank rank = rankPart switch
        {
            "2" => ECardRank.Two,
            "3" => ECardRank.Three,
            "4" => ECardRank.Four,
            "5" => ECardRank.Five,
            "6" => ECardRank.Six,
            "7" => ECardRank.Seven,
            "8" => ECardRank.Eight,
            "9" => ECardRank.Nine,
            "10" => ECardRank.Ten,
            "J" => ECardRank.Jack,
            "Q" => ECardRank.Queen,
            "K" => ECardRank.King,
            "A" => ECardRank.Ace,
            _ => throw new ArgumentOutOfRangeException(nameof(label))
        };
        
        ECardSuit suit = suitChar switch
        {
            'c' => ECardSuit.Clubs,
            'd' => ECardSuit.Diamonds,
            'h' => ECardSuit.Hearts,
            's' => ECardSuit.Spades,
            _ => throw new ArgumentOutOfRangeException(nameof(label))
        };
        
        return new(rank, suit);
    }
    
    #endregion
    
    #region Detect
    
    /// <inheritdoc />
    public async Task<IReadOnlyList<CardPrediction>> DetectAsync(byte[] imageBytes)
    {
        _logger.LogDebug("Starting image detection with Yolo.");
        
        if (_yoloPredictor is null)
        {
            SelectModel(PokerFaceConfiguration.CurrentYoloModel);
        }
        
        YoloResult<Detection> yoloResult = await _yoloPredictor!.DetectAsync(imageBytes);
        
        List<CardPrediction> predictions = yoloResult
            .Select(d => new CardPrediction(
                ParseLabel(d.Name.Name),
                d.Confidence,
                d.Bounds
            ))
            .ToList();
        
        _logger.LogInformation("Detected {Count} cards.", predictions.Count);
        return predictions;
    }
    
    #endregion
    
    #region Dispose
    
    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }
        
        _yoloPredictor?.Dispose();
        
        _isDisposed = true;
    }
    
    #endregion
    
    #region GetModelList
    
    /// <inheritdoc />
    public IReadOnlyList<string> GetModelList()
    {
        _logger.LogDebug("Starting model list retrieval.");
        
        string path = Path.Combine(AssemblyDirectory.Value, ModelsFolder);
        
        string[] modelNames = Directory.GetFiles(path, ModelFilePattern);
        
        List<string> names = modelNames.Select(Path.GetFileName).ToList()!;
        
        _logger.LogInformation("{Count} models found.", names.Count);
        return names;
    }
    
    #endregion
    
    #region SelectModel
    
    /// <inheritdoc />
    public void SelectModel(string modelName)
    {
        _logger.LogDebug("Starting model selection.");
        
        string path = Path.Combine(AssemblyDirectory.Value, ModelsFolder, modelName);
        
        if (!File.Exists(path))
        {
            throw new ArgumentException("The model file does not exist.");
        }
        
        _yoloPredictor?.Dispose();
        _yoloPredictor = new(path);
        
        _logger.LogInformation("{ModelName} model selected.", modelName);
    }
    
    #endregion
}
