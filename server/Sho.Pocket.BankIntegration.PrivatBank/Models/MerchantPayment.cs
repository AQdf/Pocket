using System.Xml.Serialization;

namespace Sho.Pocket.BankIntegration.Privatbank.Models
{
    [XmlType("payment")]
    public class MerchantPayment
    {
        public MerchantPayment()
        {
        }

        public MerchantPayment(string cardnum, string country = "UA")
        {
            Cardnum = cardnum;
            Country = country;
        }

        /// <summary>
        /// Unique identifier of the payment assigned by payments partner.
        /// Serves to unambiguously identify operations in Privat24 and payments partner.
        /// </summary>
        [XmlAttribute("id")]
        public string PaymentId { get; set; }

        /// <summary>
        ///  Card number
        /// </summary>
        [XmlElement("cardnum", IsNullable = false)]
        public string Cardnum { get; set; }

        /// <summary>
        ///  Country
        /// </summary>
        [XmlElement("country", IsNullable = false)]
        public string Country { get; set; }
    }
}
