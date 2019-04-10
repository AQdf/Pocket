using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class BalanceFeatureContext : FeatureContextBase
    {
        public Dictionary<Guid, Asset> Assets { get; set; } = new Dictionary<Guid, Asset>();

        public Dictionary<string, Currency> Currencies { get; set; } = new Dictionary<string, Currency>();

        public Dictionary<Guid, Balance> Balances { get; set; } = new Dictionary<Guid, Balance>();

        private readonly IBalanceService _balanceService;

        public BalanceFeatureContext() : base()
        {
            _balanceService = _serviceProvider.GetRequiredService<IBalanceService>();
        }

        public BalanceViewModel GetBalance(Guid id)
        {
            return _balanceService.GetById(id);
        }

        public List<BalanceViewModel> GetAllBalances(DateTime effectiveDate)
        {
            BalancesViewModel storageBalances = _balanceService.GetAll(effectiveDate);

            List<BalanceViewModel> contextBalances = storageBalances.Items.Where(b => Balances.ContainsKey(b.Id.Value)).ToList();

            return contextBalances;
        }

        public Balance AddBalance(BalanceCreateModel createModel)
        {
            Balance newBalance = _balanceService.Add(createModel);

            Balances.Add(newBalance.Id, newBalance);

            return newBalance;
        }

        public Balance UpdateBalance(Guid id, BalanceUpdateModel updateModel)
        {
            Balance updateBalance = _balanceService.Update(id, updateModel);

            Balances[id] = updateBalance;

            return updateBalance;
        }

        public void DeleteBalance(Guid id)
        {
            _balanceService.Delete(id);

            Balances.Remove(id);
        }

        public List<BalanceViewModel> AddEffectiveBalances()
        {
            List<BalanceViewModel> balances = _balanceService.AddEffectiveBalancesTemplate();

            return balances;
        }
    }
}
