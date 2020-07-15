using Egoal.Domain.Services;

namespace Egoal.Trades
{
    public class TradeDomainService : DomainService, ITradeDomainService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeDomainService(
            ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }
    }
}
