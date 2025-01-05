namespace TradingEngineServer.Logging.LoggerConfiguration;

public class LoggingConfiguration
{
    public LoggerType LoggerType { get; set; }
    public required TextLoggerConfiguration TextLoggerConfiguration { get; set; }
    // public DatabaseLoggerConfiguration? databaseLoggerConfiguration { get; set; }
}

public class TextLoggerConfiguration
{
    public required string Directory { get; set; }
    public required string FileName { get; set; }
    public required string FileExtension { get; set; }

}

// public class DatabaseLoggerConfiguration
// {

// }
