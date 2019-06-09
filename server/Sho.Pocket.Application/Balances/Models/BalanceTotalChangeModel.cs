using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceTotalChangeModel
    {
        public BalanceTotalChangeModel()
        {
        }

        public BalanceTotalChangeModel(string currency, List<BalanceTotalModel> values)
        {
            Currency = currency;
            Values = values;
        }

        public string Currency { get; set; }

        public List<BalanceTotalModel> Values { get; set; }
    }
}
