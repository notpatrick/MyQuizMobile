using MyQuizMobile.Helpers;
using MyQuizMobile.Interfaces;
using Newtonsoft.Json;
using System;

namespace MyQuizMobile.Model
{
    public class BaseDataObject : ObservableObject, IBaseDataObject
    {
        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Id for item
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Azure created at time stamp
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
