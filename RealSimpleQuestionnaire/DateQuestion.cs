using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class DateQuestion : Question
    {
        public DateQuestion()
        {
            Id = 2;
            Text = "What was the event date?";
        }


        public class Factory : QuestionFactory<DateQuestion>
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers, IDateTime dateTime)
            {
                return answers.Any(a => a.QuestionId == 1);
            }
        }
    }
}