using System;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel
{
    public class SingleTopic : MenuItem
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }

        [JsonIgnore]
        public bool UmfrageDone { get; set; } = false;
    }
}
