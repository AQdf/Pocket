using Sho.Pocket.BankIntegration.Privatbank.Models;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Sho.Pocket.BankIntegration.Privatbank.Utils
{
    internal static class MerchantBalanceRequestExtensions
    {
        public static string ToXml(this MerchantBalanceRequest request)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MerchantBalanceRequest));
            var xml = string.Empty;

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, request);
                    xml = sw.ToString();
                }
            }

            return xml;
        }
    }
}
