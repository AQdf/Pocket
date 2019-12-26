using System.Xml.Serialization;

namespace Sho.Pocket.BankIntegration.Privatbank.Models
{
    public class Merchant
    {
        public Merchant()
        {
        }

        public Merchant(string id, string signature)
        {
            Id = id;
            Signature = signature;
        }

        /// <summary>
        /// Merchant ID registered in Privat24.
        /// </summary>
        [XmlElement("id", IsNullable = false)]
        public string Id { get; set; }

        /// <summary>
        /// Signature is built as sha1(md5(password)). password - personal merchant password after register.
        /// Look in merchant settings in Privat24 (example:3A90E5J0f6OUIfqN1Qu59gYrjDgDblfL).
        /// </summary>
        [XmlElement("signature", IsNullable = false)]
        public string Signature { get; set; }
    }
}
