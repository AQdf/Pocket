using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.BankIntegration.Privatbank.Abstractions;
using Sho.Pocket.Core.BankIntegration.Models;
using System.Net.Http;
using System.Text;
using Sho.Pocket.BankIntegration.Privatbank.Common;
using System;
using System.Xml;

namespace Sho.Pocket.BankIntegration.Privatbank.Services
{
    public class PrivatbankAccountService : IPrivatbankAccountService
    {
        private const string _bankName = "Privatbank";

        private readonly string _bankApiUrl = "https://api.privatbank.ua/p24api/";

        public async Task<List<BankAccountBalance>> GetClientAccountsInfoAsync(BankClientData clientData)
        {
            string requestUri = _bankApiUrl + "balance";
            List<BankAccountBalance> result;

            using (HttpClient client = new HttpClient())
            {
                string request = BuildRequestData(clientData);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = new StringContent(request, Encoding.UTF8, "text/xml")
                };

                HttpResponseMessage response = await client.SendAsync(message);
                string content = await response.Content.ReadAsStringAsync();
                result = ParseClientAccountsInfo(content);
            }

            return result;
        }

        public Task<string> GetClientAccountExctractAsync(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Make sure that the contents of dataContent and the corresponding substring in xml match up to a byte.
        /// </summary>
        /// <param name="clientData"></param>
        /// <returns></returns>
        private string BuildRequestData(BankClientData clientData)
        {
            string dataTagContent = $"<oper>cmt</oper><wait>0</wait><test>0</test><payment id=\"\"><prop name=\"cardnum\" value=\"{clientData.CardNumber}\" /><prop name=\"country\" value=\"UA\" /></payment>";
            string plainSignature = $"{dataTagContent}{clientData.Token}";
            string signature = plainSignature.GetHashMd5().GetHashSha1();
            string request = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><request version=\"1.0\"><merchant><id>{clientData.BankClientId}</id><signature>{signature}</signature></merchant><data>{dataTagContent}</data></request>";

            return request;
        }

        /// <summary>Parses Privatbank merchant card balance response</summary>
        /// <param name="content">
        ///     <?xml version=\"1.0\" encoding=\"UTF-8\"?>
        ///     <response version =\"1.0\">
        ///         <merchant>
        ///             <id>153774</id>
        ///             <signature>1249ef10df822f755ab723c4aaba8e624bebda7f</signature>
        ///         </merchant>
        ///         <data>
        ///             <oper>cmt</oper>
        ///             <info>
        ///                 <cardbalance>
        ///                     <card>
        ///                         <account>4731219100649452980</account>
        ///                         <card_number>4731219100649452</card_number>
        ///                         <acc_name/>
        ///                         <acc_type/>
        ///                         <currency>UAH</currency>
        ///                         <card_type/>
        ///                         <main_card_number>4731219100649452980</main_card_number>
        ///                         <card_stat/>
        ///                         <src/>
        ///                     </card>
        ///                     <av_balance>32.25</av_balance>
        ///                     <bal_date>26.12.19 04:30</bal_date>
        ///                     <bal_dyn>null</bal_dyn>
        ///                     <balance>32.25</balance>
        ///                     <fin_limit>0.0</fin_limit>
        ///                     <trade_limit>0.0</trade_limit>
        ///                 </cardbalance>
        ///             </info>
        ///         </data>
        ///     </response>
        /// </param>
        /// <returns></returns>
        private List<BankAccountBalance> ParseClientAccountsInfo(string content)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(content);

            string accountId = document.SelectSingleNode("response/data/info/cardbalance/card/card_number").InnerText;
            string currency = document.SelectSingleNode("response/data/info/cardbalance/card/currency").InnerText;
            string accountName = document.SelectSingleNode("response/data/info/cardbalance/card/acc_name").InnerText;
            string balanceString = document.SelectSingleNode("response/data/info/cardbalance/balance").InnerText;
            bool parsed = decimal.TryParse(balanceString, out decimal value);

            List<BankAccountBalance> result = new List<BankAccountBalance>();

            if (!string.IsNullOrWhiteSpace(accountId) && !string.IsNullOrWhiteSpace(currency) && parsed)
            {
                accountName = !string.IsNullOrWhiteSpace(accountName) ? accountName : GetFriendlyAccountName(value, currency);
                BankAccountBalance accountBalance = new BankAccountBalance(_bankName, accountId, accountName, currency, value);
                result.Add(accountBalance);
            }

            return result;
        }

        private string GetFriendlyAccountName(decimal balance, string currency)
        {
            return $"{_bankName}: {balance} {currency}";
        }
    }
}
