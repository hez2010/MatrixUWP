#nullable enable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    public class ChoiceAssignmentQuestion : INotifyPropertyChanged
    {
        private List<ChoiceAssignmentElement>? choices;

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("choices")]
        public List<ChoiceAssignmentElement>? Choices
        {
            get => choices;
            set
            {
                choices = value;
                if (choices is null) return;
                foreach (var i in choices)
                {
                    i.Question = this;
                }
            }
        }
        [JsonProperty("grading")]
        public ChoiceAssignmentGrading Grading { get; set; } = new ChoiceAssignmentGrading();
        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; } = "";
        [JsonProperty("caseStatus")]
        public string CaseStatus { get; set; } = "";
        [JsonProperty("choice_type")]
        public string ChoiceType { get; set; } = "";
        public bool MultiChoice => ChoiceType == "multi";
        [JsonProperty("description")]
        public string Description { get; set; } = "";
        [JsonProperty("explanation")]
        public string Explanation { get; set; } = "";
        [JsonProperty("standard_answer")]
        public List<int>? StandardAnswer { get; set; }
        public string StandardAnswerDisplayText
        {
            get
            {
                if (StandardAnswer is null || StandardAnswer.Count == 0) return "";
                var sb = new StringBuilder();
                sb.Append("标准答案：");
                foreach (var i in StandardAnswer)
                {
                    sb.AppendFormat("{0}, ", (char)(i + 'A'));
                }
                var str = sb.ToString();
                return str.Substring(0, str.Length - 2);
            }
        }

        public Action ResetSelection => () =>
        {
            if (choices is null) return;
            foreach (var i in choices)
            {
                i.IsChecked = false;
            }
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}