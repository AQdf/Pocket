using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceService
    {
        Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userOpenId, DateTime effectiveDate);

        Task<BalanceViewModel> GetUserBalanceAsync(Guid userOpenId, Guid id);

        Task<BalanceViewModel> AddBalanceAsync(Guid userOpenId, BalanceCreateModel createModel);

        Task<List<BalanceViewModel>> AddEffectiveBalancesTemplate(Guid userOpenId);

        Task<BalanceViewModel> UpdateBalanceAsync(Guid userOpenId, Guid id, BalanceUpdateModel updateModel);

        Task<bool> DeleteBalanceAsync(Guid userOpenId, Guid Id);

        Task<List<BalanceTotalModel>> GetCurrentTotalBalance(Guid userOpenId);

        Task<List<DateTime>> GetEffectiveDatesAsync(Guid userOpenId);

        Task ApplyExchangeRate(ExchangeRateModel model);

        Task<List<BalanceTotalModel>> GetCurrencyTotals(Guid userOpenId, string currencyName, int count);

        Task<byte[]> ExportUserBalancesToCsvAsync(Guid userOpenId);
    }
}
