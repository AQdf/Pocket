using Sho.Pocket.Application.ExchangeRates.Models;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Application.ExchangeRates
{
    public interface IExchangeRateService
    {
        List<ExchangeRateModel> AddDefaultExchangeRates(DateTime effectiveDate);

        ExchangeRateModel AlterExchangeRate(ExchangeRateModel model);
    }
}
