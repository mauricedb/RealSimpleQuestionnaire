using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class SiteFatalitiesQuestion : Question
    {
        public SiteFatalitiesQuestion(string site)
        {
            Id = 5;
            Text = string.Format("Where there fatalities on site '{0}'?", site);
        }

        public class Factory : QuestionFactory
        {
            internal override bool CanBeAsked(IEnumerable<Answer> answers)
            {
                return answers.Any(a => a.QuestionId == 4 && (Severity) a.Result == Severity.High);
            }

            public override IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
            {
                IEnumerable<SiteFatalitiesQuestion> questions = from a in answers
                    where a.QuestionId == 4 && (Severity) a.Result == Severity.High
                    let siteAnswer = a as SiteAnswer
                    select new SiteFatalitiesQuestion(siteAnswer.Site);

                return questions;
            }
        }
    }
}