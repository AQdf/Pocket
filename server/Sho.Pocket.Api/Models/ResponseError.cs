using Newtonsoft.Json;

namespace Sho.Pocket.Api.Models
{
    public class ResponseError
    {
        public ResponseError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
