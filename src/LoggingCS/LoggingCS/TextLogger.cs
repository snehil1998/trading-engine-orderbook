using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using TradingEngineServer.Logging.LoggerConfiguration;

namespace TradingEngineServer.Logging;

public class TextLogger : AbstractLogger, ITextLogger
{
    public TextLogger(IOptions<LoggingConfiguration> loggingConfiguration): base()
    {
        _loggingConfiguration = loggingConfiguration.Value ?? throw new ArgumentNullException(nameof(loggingConfiguration));
        if(_loggingConfiguration.LoggerType != LoggerType.Text)
        {
            throw new InvalidOperationException($"{nameof(TextLogger)} does not match loggerType {_loggingConfiguration.LoggerType}");
        }

        var now = DateTime.Now;
        var logDirectory = Path.Combine(_loggingConfiguration.TextLoggerConfiguration.Directory, $"{now:yyy-MM-dd}");
        var fileName = _loggingConfiguration.TextLoggerConfiguration.FileName;
        var baseLogName = Path.ChangeExtension(fileName, _loggingConfiguration.TextLoggerConfiguration.FileExtension);
        var filePath = Path.Combine(logDirectory, baseLogName);
        Directory.CreateDirectory(logDirectory);
        _ = Task.Run(() => LogAsync(filePath, _logQueue, cancellationTokenSource.Token));
    }
    protected override void Log(LogLevel logLevel, string module, string message)
    {
        _logQueue.Post(new LogInformation(logLevel, module, message, DateTime.Now, Thread.CurrentThread.ManagedThreadId,
            Thread.CurrentThread.Name));
    }

    private static async void LogAsync(string filePath, BufferBlock<LogInformation> queue, CancellationToken token)
    {
        using var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var streamWriter = new StreamWriter(fileStream) { AutoFlush = true };
        try{
            while (true)
            {
                var logInformation = await queue.ReceiveAsync(token);
                string formattedMessage = FormatLogInformation(logInformation);
                await streamWriter.WriteAsync(formattedMessage).ConfigureAwait(false);
            }
        }
        catch(OperationCanceledException)
        {
            
        }
    }

    private static string FormatLogInformation(LogInformation logInformation)
    {
        return $"[{logInformation.Date:yyy-MM-dd HH-mm-ss.fffffff}] [{logInformation.ThreadName}:{logInformation.ThreadId:000}] " +
        $"[{logInformation.LogLevel}] [{logInformation.Module}] {logInformation.Message}\n";
    }

    // sort of a destructor, called by GC when the object has to be collected to clear unmanaged resources
    ~TextLogger()
    {
        Dispose(false); // should only clear unmanaged resources
    }

    public void Dispose () 
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        lock(_lock)
        {
            if(_dispose) return;
            _dispose = true;
        }
        if (disposing)
        {
            // get rid of managed resources (managed objects)
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        // get rid of unmanaged resources, eg., database connection, file stream, etc.
    }

    private readonly LoggingConfiguration _loggingConfiguration;
    private readonly BufferBlock<LogInformation> _logQueue = new();
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private bool _dispose;
    private object _lock = new();
}
