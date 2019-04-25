using Sho.Pocket.Application.ExchangeRates.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRateModel>> AddDefaultExchangeRates(DateTime effectiveDate);

        Task<ExchangeRateModel> AlterExchangeRate(ExchangeRateModel model);
    }
}
