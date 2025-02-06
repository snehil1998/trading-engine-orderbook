using System;

namespace TradingEngineServer.Orders;

// should sort order in Bid limit in decending order
public sealed class BidPriceLevelComparer: IComparer<PriceLevel>
{
    public static BidPriceLevelComparer Comparer = new();
    public int Compare(PriceLevel? x, PriceLevel? y)
    {
        if (x?.Price == y?.Price) return 0;
        if (x?.Price > y?.Price) return -1;
        return 1;
    }
}

// should sort order in Ask limit in ascending order
public sealed class AskPriceLevelComparer: IComparer<PriceLevel>
{
    public static AskPriceLevelComparer Comparer = new();

    public int Compare(PriceLevel? x, PriceLevel? y)
    {
        if (x?.Price == y?.Price) return 0;
        if (x?.Price < y?.Price) return -1;
        return 1;
    }
}
