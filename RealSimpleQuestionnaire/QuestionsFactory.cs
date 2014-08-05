using System;
using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class QuestionsFactory
    {
        private readonly IDateTime _dateTime;
        private static readonly List<QuestionFactory> Questions;

        static QuestionsFactory()
        {
            Questions = typeof (QuestionsFactory)
                .Assembly.GetTypes()
                .Where(t => typeof (QuestionFactory).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (QuestionFactory) Activator.CreateInstance(t))
                .ToList();
        }

        public QuestionsFactory():this(new SystemDateTime())
        {
        }

        public QuestionsFactory(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public IEnumerable<Question> QuestionsFor(IEnumerable<Answer> answers)
        {
            List<Question> questions = Questions
                .Where(q => q.CanBeAsked(answers, _dateTime))
                .SelectMany(q => q.RepeatAsNeeded(answers))
                .ToList();

            return questions;
        }
    }
}