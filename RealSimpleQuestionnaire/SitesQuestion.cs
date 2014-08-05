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
            Sites = new[] {"s1", "s2", "s3", "s4", "s51"};
        }

        public IEnumerable<string> Sites { get; set; }

        public class Factory : QuestionFactory<SitesQuestion>
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers, IDateTime dateTime)
            {
                return answers.Any(a => a.QuestionId == 2);
            }
        }
    }
}