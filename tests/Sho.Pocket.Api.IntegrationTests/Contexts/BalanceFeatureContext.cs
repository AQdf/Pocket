using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<BalanceViewModel> GetBalance(Guid id)
        {
            return await _balanceService.GetById(id);
        }

        public async Task<List<BalanceViewModel>> GetAllBalances(DateTime effectiveDate)
        {
            BalancesViewModel storageBalances = await _balanceService.GetAll(effectiveDate);

            List<BalanceViewModel> contextBalances = storageBalances.Items.Where(b => Balances.ContainsKey(b.Id.Value)).ToList();

            return contextBalances;
        }

        public async Task<Balance> AddBalance(BalanceCreateModel createModel)
        {
            Balance newBalance = await _balanceService.Add(createModel);

            Balances.Add(newBalance.Id, newBalance);

            return newBalance;
        }

        public async Task<Balance> UpdateBalance(Guid id, BalanceUpdateModel updateModel)
        {
            Balance updateBalance = await _balanceService.Update(id, updateModel);

            Balances[id] = updateBalance;

            return updateBalance;
        }

        public async Task DeleteBalance(Guid id)
        {
            await _balanceService.Delete(id);

            Balances.Remove(id);
        }

        public async Task<List<BalanceViewModel>> AddEffectiveBalances()
        {
            IEnumerable<BalanceViewModel> balances = await _balanceService.AddEffectiveBalancesTemplate();

            return balances.ToList();
        }
    }
}
