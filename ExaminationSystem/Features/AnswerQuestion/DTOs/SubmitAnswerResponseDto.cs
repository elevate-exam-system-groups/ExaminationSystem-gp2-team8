namespace ExaminationSystem.Features.AnswerQuestion.DTOs
{
    public class SubmitAnswerResponseDto
    {
        public bool Saved { get; set; }
        public int QuestionId { get; set; }
        public int AttemptId { get; set; }

        /// <summary>Remaining seconds on the server-side timer at the moment of save.</summary>
        public int SecondsRemaining { get; set; }
    }
}
