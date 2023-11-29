namespace Frends.SMTP.SendEmail.Definitions
{
    /// <summary>
    /// Result class of the Task.
    /// </summary>
    public class Result
    {
        internal Result(bool emailSent, string status)
        {
            EmailSent = emailSent;
            StatusString = status;
        }

        /// <summary>
        /// Value is true if email was sent.
        /// </summary>
        /// <example>true</example>
        public bool EmailSent { get; private set; }

        /// <summary>
        /// Contains information about the task's result.
        /// </summary>
        /// <example></example>
        public string StatusString { get; private set; }
    }
}
