using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class SitesQuestion : Question
    {
        public SitesQuestion()
        {
            Id = 3;
            Text = "What sites where affected?";
        }

        public class Factory : QuestionFactory<SitesQuestion>
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers)
            {
                return answers.Any(a => a.QuestionId == 2);
            }
        }
    }
}