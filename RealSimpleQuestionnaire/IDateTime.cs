using System;

namespace RealSimpleQuestionnaire
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }

    public class SystemDateTime : IDateTime
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }

}