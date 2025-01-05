namespace TradingEngineServer.Logging;

public interface ILogger
{
    public void Debug(string module, string message);
    public void Debug(string module, Exception exception);
    public void Information(string module, string message);
    public void Information(string module, Exception exception);
    public void Warning(string module, string message);
    public void Warning(string module, Exception exception);
    public void Error(string module, string message);
    public void Error(string module, Exception exception); 
}
