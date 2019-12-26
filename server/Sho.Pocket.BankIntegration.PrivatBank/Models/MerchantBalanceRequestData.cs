using System.Xml.Serialization;

namespace Sho.Pocket.BankIntegration.Privatbank.Models
{
    public class MerchantBalanceRequestData
    {
        public MerchantBalanceRequestData()
        {
        }

        public MerchantBalanceRequestData(MerchantPayment payment, string oper = "cmt", int wait = 0, int test = 0)
        {
            Oper = oper;
            Wait = wait;
            Test = test;
            Payment = payment;
        }

        /// <summary>
        /// Example: cmt
        /// </summary>
        [XmlElement("oper", IsNullable = false)]
        public string Oper { get; set; }

        /// <summary>
        /// Maximum waiting time to submit payment (in seconds).
        /// Value range: 1 - 90. If payment is not handled in specified time, response will contain state = 1, message = "payment added to the queue".
        /// It means that payment is queued but is not handled and result is unidentified.
        /// The details of this transaction can be found in merchant extract.
        /// </summary>
        [XmlElement("wait", IsNullable = false)]
        public int Wait { get; set; }

        /// <summary>
        /// Flag that identifies if this is a test payment.
        /// 0 - payment will be submitted immediately.
        /// 1 - payment will be tested for correctness, but won't be submitted.
        /// </summary>
        [XmlElement("test", IsNullable = false)]
        public int Test { get; set; }

        [XmlElement("payment", IsNullable = false)]
        public MerchantPayment Payment { get; set; }
    }
}