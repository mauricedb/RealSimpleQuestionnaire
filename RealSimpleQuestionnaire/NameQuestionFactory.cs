using System.Collections.Generic;

namespace RealSimpleQuestionnaire
{
    public class NameQuestionFactory : QuestionFactory<Question>
    {
        public override IEnumerable<Question> RepeatAsNeeded(IEnumerable<Answer> answers)
        {
            return new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                }
            };
        }
    }
}