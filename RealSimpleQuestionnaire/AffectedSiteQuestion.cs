using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class AffectedSiteQuestion : Question
    {
        public AffectedSiteQuestion(string site)
        {
            Id = 4;
            Text = string.Format("How was site '{0}' affected?", site);
        }

        public class Factory : QuestionFactory
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers, IDateTime dateTime)
            {
                return answers.Any(a => a.QuestionId == 3);
            }

            public override IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
            {
                var questions = from a in answers
                    where a.QuestionId == 3
                    let sites = a.Result as IEnumerable<string>
                    from site in sites
                    select new AffectedSiteQuestion(site);

                return questions;
            }
        }
    }
}