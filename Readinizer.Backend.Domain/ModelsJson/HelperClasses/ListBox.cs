using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class ListBox
    {
        public ListBox()
        {
            Value = new Value();
            State = "Undefined";
        }
        [JsonProperty("Value")]
        public Value Value { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }
    }
}
