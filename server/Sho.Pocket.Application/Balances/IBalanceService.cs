using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceService
    {
        Task<BalancesViewModel> GetAll(DateTime effectiveDate);

        Task<BalanceViewModel> GetById(Guid id);

        Task<Balance> Add(BalanceCreateModel createModel);

        Task<IEnumerable<BalanceViewModel>> AddEffectiveBalancesTemplate();

        Task<Balance> Update(Guid id, BalanceUpdateModel updateModel);

        Task Delete(Guid Id);

        Task<IEnumerable<BalanceTotalModel>> GetCurrentTotalBalance();

        Task<IEnumerable<DateTime>> GetEffectiveDates();

        Task ApplyExchangeRate(ExchangeRateModel model);

        Task<IEnumerable<BalanceTotalModel>> GetCurrencyTotals(string currencyName, int count);

        Task<byte[]> ExportBalancesToCsv();
    }
}
