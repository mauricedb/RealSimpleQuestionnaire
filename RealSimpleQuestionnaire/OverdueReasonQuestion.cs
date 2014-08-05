using System;
using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class OverdueReasonQuestion : Question
    {
        public OverdueReasonQuestion()
        {
            Id = 6;
            Text = "Why was this not reported within a week?";
        }


        public class Factory : QuestionFactory<OverdueReasonQuestion>
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers, IDateTime dateTime)
            {
                return answers.Any(a => a.QuestionId == 2 && (DateTime)a.Result < dateTime.UtcNow.AddDays(-7));
            }
        }
    }
}