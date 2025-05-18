#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace DrivingExamBackend.Models
{

    public class Answer : Entity<int>
    {
        protected Answer() { }
        public Answer(Question question, string text, bool isCorrect)
        {
            Question = question;
            Text = text;
            IsCorrect = isCorrect;
        }

        public Question Question { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.