using System;
using System.Collections.Generic;
using System.Linq;

namespace RealSimpleQuestionnaire
{
    public class QuestionsFactory
    {
        private static readonly List<QuestionFactory> Questions;

        static QuestionsFactory()
        {
            Questions = typeof (QuestionsFactory)
                .Assembly.GetTypes()
                .Where(t => typeof (QuestionFactory).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (QuestionFactory) Activator.CreateInstance(t))
                .ToList();
        }

        public IEnumerable<Question> QuestionsFor(IEnumerable<Answer> answers)
        {
            List<Question> questions = Questions
                .Where(q => q.CanBeAsked(answers))
                .SelectMany(q => q.RepeatAsNeeded(answers))
                .ToList();

            return questions;
        }
    }
}