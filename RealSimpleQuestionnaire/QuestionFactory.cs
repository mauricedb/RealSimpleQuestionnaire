using System.Collections.Generic;

namespace RealSimpleQuestionnaire
{
    public abstract class QuestionFactory<T> : QuestionFactory where T : Question, new()
    {
        public override IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
        {
            return new[] {new T()};
        }
    }

    public abstract class QuestionFactory
    {
        internal virtual bool CanBeAsked(IEnumerable<Answer> answers, IDateTime dateTime)
        {
            return true;
        }

        public abstract IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers);
    }
}