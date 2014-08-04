using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }
    }

    public class QuestionsFactory
    {
        private readonly List<Question> _questions;

        public QuestionsFactory()
        {
            _questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new DateQuestion(),
                new SitesQuestion(),
                new AffectedSiteQuestion(),
                new SiteFatalitiesQuestion()
            };
        }

        public IEnumerable<Question> QuestionsFor(IEnumerable<Answer> answers)
        {
            IEnumerable<Question> result = _questions
                .Where(q => q.CanBeAsked(answers))
                .SelectMany(q => q.RepeatAsNeeded(answers))
                .ToList();

            return result;
        }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        internal virtual bool CanBeAsked(IEnumerable<Answer> answers)
        {
            return true;
        }

        public virtual IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
        {
            return new[] { this };
        }
    }

    public class DateQuestion : Question
    {
        public DateQuestion()
        {
            Id = 2;
            Text = "What was the event date?";
        }

        internal override bool CanBeAsked(IEnumerable<Answer> answers)
        {
            return answers.Any(a => a.QuestionId == 1);
        }
    }

    public class SitesQuestion : Question
    {
        public SitesQuestion()
        {
            Id = 3;
            Text = "What sites where affected?";
        }

        internal override bool CanBeAsked(IEnumerable<Answer> answers)
        {
            return answers.Any(a => a.QuestionId == 2);
        }
    }

    public class AffectedSiteQuestion : Question
    {
        public AffectedSiteQuestion()
        {
            Id = 4;
            Text = "How was site '{0}' affected?";
        }

        public AffectedSiteQuestion(string site)
            : this()
        {
            Text = string.Format(Text, site);
        }

        internal override bool CanBeAsked(IEnumerable<Answer> answers)
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

    public class SiteFatalitiesQuestion : Question
    {
        public SiteFatalitiesQuestion()
        {
            Id = 5;
            Text = "Where there fatalities on site '{0}'?";
        }

        public SiteFatalitiesQuestion(string site)
            : this()
        {
            Text = string.Format(Text, site);
        }

        internal override bool CanBeAsked(IEnumerable<Answer> answers)
        {
            return answers.Any(a => a.QuestionId == 4 && (Severity)a.Result == Severity.High);
        }

        public override IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
        {
            var questions = from a in answers
                            where a.QuestionId == 4 && (Severity)a.Result == Severity.High
                            let siteAnswer = a as SiteAnswer
                            select new SiteFatalitiesQuestion(siteAnswer.Site);

            return questions;
        }
    }

    public class Answer
    {
        public int QuestionId { get; set; }
        public object Result { get; set; }
    }

    public class SiteAnswer : Answer
    {
        public string Site { get; set; }
    }

    public enum Severity
    {
        Low,
        Medium,
        High
    }
}