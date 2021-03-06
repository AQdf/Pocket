﻿using System;
using Newtonsoft.Json;
using Sho.Pocket.Core.Common.Converters;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.ExchangeRates.Models
{
    public class ExchangeRateModel
    {
        public ExchangeRateModel()
        {
        }

        public ExchangeRateModel(ExchangeRate exchangeRate)
        {
            Id = exchangeRate.Id;
            EffectiveDate = exchangeRate.EffectiveDate;
            BaseCurrency = exchangeRate.BaseCurrency;
            CounterCurrency = exchangeRate.CounterCurrency;
            Buy = exchangeRate.BuyRate;
            Sell = exchangeRate.SellRate;
            Provider = exchangeRate.Provider;
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal Buy { get; set; }

        public decimal Sell { get; set; }

        public string Provider { get; set; }
    }
}
