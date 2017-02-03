﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class QuestionBlock : MenuItem {
        public override int Id { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public List<Question> Questions { get; set; }
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.QuestionBlock;
        public override string DetailText => Questions != null ? $"{Questions.Count} Fragen" : string.Empty;
    }
}