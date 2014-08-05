using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealSimpleQuestionnaire;

namespace RealSimpleQuestionnaireSpecs
{
    [TestClass]
    public class QuestionsFactoryUnitTest
    {
        [TestMethod]
        public void ThereShouldBeStartQuestionAskingTheName()
        {
            var factory = new QuestionsFactory();
            IEnumerable<Answer> answers = Enumerable.Empty<Answer>();

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                }
            });
        }

        [TestMethod]
        public void AfterTheNameWeShouldAskForTheDate()
        {
            var factory = new QuestionsFactory();
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 1,
                    Result = "Maurice"
                }
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 2,
                    Text = "What was the event date?"
                }
            });
        }

        [TestMethod]
        public void AfterTheDateWeShouldAskForSites()
        {
            var dateTime = new Moq.Mock<IDateTime>();
            dateTime.SetupGet(x => x.UtcNow).Returns(16.July(2014));

            var factory = new QuestionsFactory(dateTime.Object);
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 1,
                    Result = "Maurice"
                },
                new Answer
                {
                    QuestionId = 2,
                    Result = 15.July(2014)
                }
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 2,
                    Text = "What was the event date?"
                },
                new Question
                {
                    Id = 3,
                    Text = "What sites where affected?"
                }
            });
        }

        [TestMethod]
        public void AfterTheDateWasOverDueWeShouldAskForTheReasonAndSites()
        {
            var dateTime = new Moq.Mock<IDateTime>();
            dateTime.SetupGet(x => x.UtcNow).Returns(25.July(2014));

            var factory = new QuestionsFactory(dateTime.Object);
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 1,
                    Result = "Maurice"
                },
                new Answer
                {
                    QuestionId = 2,
                    Result = 15.July(2014)
                }
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 2,
                    Text = "What was the event date?"
                },
                new Question
                {
                    Id = 3,
                    Text = "What sites where affected?"
                },
                new Question
                {
                    Id = 6,
                    Text = "Why was this not reported within a week?"
                }
            });
        }

        [TestMethod]
        public void AfterTheSitesWeShouldAskAboutEachSite()
        {
            var factory = new QuestionsFactory();
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 1,
                    Result = "Maurice"
                },
                new Answer
                {
                    QuestionId = 2,
                    Result = 4.August(2014)
                },
                new Answer
                {
                    QuestionId = 3,
                    Result = new[] {"s1", "s2", "s3"}
                },
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 2,
                    Text = "What was the event date?"
                },
                new Question
                {
                    Id = 3,
                    Text = "What sites where affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's1' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's2' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's3' affected?"
                }
            });
        }

        [TestMethod]
        public void ShortAfterTheSitesWeShouldAskAboutEachSite()
        {
            var factory = new QuestionsFactory();
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 3,
                    Result = new[] {"s1", "s2", "s3"}
                },
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's1' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's2' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's3' affected?"
                }
            });
        }

        [TestMethod]
        public void AfterTheSitesWeShouldAskAboutEachSiteAndSeverity()
        {
            var factory = new QuestionsFactory();
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = 3,
                    Result = new[] {"s1", "s2", "s3"}
                },
                new SiteAnswer
                {
                    QuestionId = 4,
                    Site = "s1",
                    Result = Severity.High // Should ask for fatalities
                },
                new SiteAnswer
                {
                    QuestionId = 4,
                    Site = "s2",
                    Result = Severity.Low // Should now ask for fatalities
                },
            };

            IEnumerable<Question> questions = factory.QuestionsFor(answers);

            questions.ShouldBeEquivalentTo(new[]
            {
                new Question
                {
                    Id = 1,
                    Text = "What is your name?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's1' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's2' affected?"
                },
                new Question
                {
                    Id = 4,
                    Text = "How was site 's3' affected?"
                },
                new Question
                {
                    Id = 5,
                    Text = "Where there fatalities on site 's1'?"
                }
            });
        }
    }
}