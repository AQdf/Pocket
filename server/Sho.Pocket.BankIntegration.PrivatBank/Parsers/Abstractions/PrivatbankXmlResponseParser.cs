using System.Xml;

namespace Sho.BankIntegration.Privatbank.Parsers.Abstractions
{
    internal abstract class PrivatbankXmlResponseParser<TResult> where TResult : class
    {
        public abstract TResult Parse(string xml);

        public string GetNodeText(XmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath)?.InnerText;
        }

        public string GetAttributeText(XmlNode node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName)?.InnerText;
        }
    }
}
