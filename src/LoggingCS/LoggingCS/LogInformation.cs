using System;
using System.Reflection.Metadata;

namespace TradingEngineServer.Logging;

public record LogInformation(LogLevel LogLevel, string Module, string Message, DateTime Date, int ThreadId, string? ThreadName);
