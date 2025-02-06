namespace TradingEngineServer.Security;

public sealed class Security
{
    public Security(string symbol, string name, string assetType)
    {
        Symbol = symbol;
        Name = name;
        AssetType = assetType;
    }

    public string Symbol { get; init; }
    public string Name { get; init; }
    public string AssetType { get; init; }
}
