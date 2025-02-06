namespace TradingEngineServer.Input;

public interface IActionsFactory
{
    void AddSecurity(CancellationToken token);
    void AddOrder(string username, CancellationToken token);
    void CancelOrder(string username, CancellationToken token);
    void ModifyOrder(string username, CancellationToken token);
    void PrintOrderbook(CancellationToken token);
    void PrintOrders(CancellationToken token);
}
