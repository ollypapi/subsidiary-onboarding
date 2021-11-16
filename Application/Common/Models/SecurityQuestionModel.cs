using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Common.Models
{
    public class SecurityQuestionModel
    {
        public virtual long Id { get; set; }
        public string Question { get; set; }
    }
    public class QuestionModel:SecurityQuestionModel
    {
        [JsonIgnore]
        public override long Id { get; set; }
        public string Answer { get; set; }
    }
}
