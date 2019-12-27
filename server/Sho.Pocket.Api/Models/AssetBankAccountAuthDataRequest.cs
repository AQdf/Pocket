using System;

namespace Sho.Pocket.Api.Models
{
    public class AssetBankAccountAuthDataRequest
    {
        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string Token { get; set; }

        public string BankClientId { get; set; }

        public string CardNumber { get; set; }
    }
}
