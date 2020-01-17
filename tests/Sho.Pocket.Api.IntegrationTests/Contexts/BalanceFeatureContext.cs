using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class BalanceFeatureContext : FeatureContextBase
    {
        public Dictionary<Guid, BalanceViewModel> Balances { get; set; } = new Dictionary<Guid, BalanceViewModel>();

        private readonly IBalanceService _balanceService;

        public BalanceFeatureContext() : base()
        {
            _balanceService = _serviceProvider.GetRequiredService<IBalanceService>();
        }

        public async Task<BalanceViewModel> GetBalance(Guid id)
        {
            return await _balanceService.GetUserBalanceAsync(User.Id, id);
        }

        public async Task<List<BalanceViewModel>> GetAllBalances(DateTime effectiveDate)
        {
            BalancesViewModel storageBalances = await _balanceService.GetUserEffectiveBalancesAsync(User.Id, effectiveDate);

            List<BalanceViewModel> contextBalances = storageBalances.Items.Where(b => Balances.ContainsKey(b.Id.Value)).ToList();

            return contextBalances;
        }

        public async Task<BalanceViewModel> AddBalance(BalanceCreateModel createModel)
        {
            BalanceViewModel newBalance = await _balanceService.AddBalanceAsync(User.Id, createModel);

            Balances.Add(newBalance.Id.Value, newBalance);

            return newBalance;
        }

        public async Task<BalanceViewModel> UpdateBalance(Guid id, BalanceUpdateModel updateModel)
        {
            BalanceViewModel updateBalance = await _balanceService.UpdateBalanceAsync(User.Id, id, updateModel);

            Balances[id] = updateBalance;

            return updateBalance;
        }

        public async Task DeleteBalance(Guid id)
        {
            await _balanceService.DeleteBalanceAsync(User.Id, id);

            Balances.Remove(id);
        }

        public async Task<List<BalanceViewModel>> AddEffectiveBalances()
        {
            List<BalanceViewModel> balances = await _balanceService.AddEffectiveBalancesTemplate(User.Id);

            return balances;
        }
    }
}
