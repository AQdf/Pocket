using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceService
    {
        BalancesViewModel GetAll(DateTime effectiveDate);

        BalanceViewModel GetById(Guid id);

        Balance Add(BalanceCreateModel createModel);

        List<BalanceViewModel> AddEffectiveBalancesTemplate();

        Balance Update(Guid id, BalanceUpdateModel updateModel);

        void Delete(Guid Id);

        IEnumerable<BalanceTotalModel> GetCurrentTotalBalance();

        IEnumerable<DateTime> GetEffectiveDates();

        void ApplyExchangeRate(ExchangeRateModel model);

        IEnumerable<BalanceTotalModel> GetCurrencyTotals(string currencyName, int count);

        byte[] ExportBalancesToCsv();
    }
}
