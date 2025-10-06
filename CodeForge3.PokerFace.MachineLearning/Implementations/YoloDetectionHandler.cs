using System.Reflection;
using CodeForge3.PokerFace.MachineLearning.Interfaces;
using Compunet.YoloSharp;
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
    private readonly YoloPredictor _yoloPredictor;
    
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
    /// <param name="modelName">The name of the model to use.</param>
    public YoloDetectionHandler(ILogger<YoloDetectionHandler> logger, string modelName)
    {
        string path = Path.Combine(AssemblyDirectory.Value, ModelsFolder, modelName);
        _logger = logger;
        _yoloPredictor = new(path);
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
    
    #region Dispose
    
    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }
        
        _yoloPredictor.Dispose();
        
        _isDisposed = true;
    }
    
    #endregion
}
