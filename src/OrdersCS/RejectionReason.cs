using System;

namespace TradingEngineServer.Rejects;

public enum RejectionReason
{
    Unknown,
    OrderNotFound,
    InstrumentNotFound,
    AttemptModifyingWrongSide
}
