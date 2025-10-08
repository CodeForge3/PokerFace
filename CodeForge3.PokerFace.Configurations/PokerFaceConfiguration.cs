using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CodeForge3.PokerFace.Configurations;

/// <summary>
/// The application configuration.
/// </summary>
public static class PokerFaceConfiguration
{
    #region Constants
    
    /// <summary>
    /// The name of the configuration file.
    /// </summary>
    private const string AppSettings = "appsettings.json";
    
    /// <summary>
    /// The name of the configuration file for the environment.
    /// </summary>
    private const string AppSettingsEnvironment = "appsettings.{0}.json";
    
    /// <summary>
    /// The name of the environment variable that contains the ASP.NET Core environment.
    /// </summary>
    private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
    
    /// <summary>
    /// The message returned when the assembly could not be found.
    /// </summary>
    private const string AssemblyNotFoundMessage = "The 'CodeForge3.PokerFace.Configurations' " +
        "assembly could not be found.";
    
    /// <summary>
    /// The name of the current Yolo model in the configuration file.
    /// </summary>
    private const string CurrentYoloModelName = "CurrentModels:Yolo";
    
    /// <summary>
    /// The message returned when a key is not found in the configuration file.
    /// </summary>
    private const string KeyNotFoundMessage = "The key '{0}' could not be found in the configuration file.";
    
    #endregion
    
    #region Constructor
    
    /// <summary>
    /// Initializes the <see cref="PokerFaceConfiguration" /> class.
    /// </summary>
    /// <exception cref="DirectoryNotFoundException">
    /// If the assembly directory could not be found.
    /// </exception>
    static PokerFaceConfiguration()
    {
        Assembly currentAssembly = typeof(PokerFaceConfiguration).Assembly;
        string assemblyDirectory = Path.GetDirectoryName(currentAssembly.Location)
            ?? throw new DirectoryNotFoundException(AssemblyNotFoundMessage);
        
        string? environment = Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
        
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.SetBasePath(assemblyDirectory);
        
        configurationBuilder.AddJsonFile(
            AppSettings,
            optional: false,
            reloadOnChange: true
        );
        
        if (environment != null)
        {
            configurationBuilder.AddJsonFile(
                string.Format(AppSettingsEnvironment, environment),
                optional: false,
                reloadOnChange: true
            );
        }
        
        ConfigurationRoot = configurationBuilder.Build();
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// The application root configuration.
    /// </summary>
    public static IConfigurationRoot ConfigurationRoot { get; }
    
    /// <summary>
    /// The current Yolo model.
    /// </summary>
    /// <exception cref="KeyNotFoundException">
    /// If the key could not be found in the configuration file.
    /// </exception>
    public static string CurrentYoloModel => ConfigurationRoot[CurrentYoloModelName]
        ?? throw CreateKeyNotFoundException(CurrentYoloModelName);
    
    #endregion
    
    #region CreateKeyNotFoundException
    
    /// <summary>
    /// Creates a new <see cref="KeyNotFoundException" /> instance.
    /// </summary>
    /// <param name="key">The name of the missing key.</param>
    /// <returns>The constructed <see cref="KeyNotFoundException" /> instance.</returns>
    private static KeyNotFoundException CreateKeyNotFoundException(string key)
    {
        string message = string.Format(KeyNotFoundMessage, key);
        return new KeyNotFoundException(message);
    }
    
    #endregion
}
