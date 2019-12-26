using System.Xml.Serialization;

namespace Sho.Pocket.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Request content to get account balance. Example:
    ///     <?xml version="1.0" encoding="UTF-8"?>
    ///     <request version="1.0">
    ///         <merchant>
    ///             <id>75482</id>
    ///             <signature>ab871c9601cf28920c4c0ff63041ea585da9de89</signature>
    ///         </merchant>
    ///         <data>
    ///             <oper>cmt</oper>
    ///             <wait>0</wait>
    ///             <test>0</test>
    ///             <payment id = "" >
    ///                 <prop name="cardnum" value="5168742060221193" />
    ///                 <prop name = "country" value="UA" />
    ///             </payment>
    ///         </data>
    ///     </request>
    /// </summary>
    [XmlRoot("request")]
    public class MerchantBalanceRequest
    {
        public MerchantBalanceRequest()
        {
        }

        public MerchantBalanceRequest(Merchant merchant, MerchantBalanceRequestData data)
        {
            Merchant = merchant;
            Data = data;
        }

        [XmlElement("merchant", IsNullable = false)]
        public Merchant Merchant { get; set; }

        [XmlElement("data", IsNullable = false)]
        public MerchantBalanceRequestData Data { get; set; }
    }
}
