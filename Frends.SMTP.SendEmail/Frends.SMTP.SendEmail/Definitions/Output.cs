namespace Frends.Smtp.SendEmail.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Output
{
    /// <summary>
    /// Value is true if email was sent.
    /// </summary>
    /// <example>true</example>
    public bool EmailSent { get; private set; }

    /// <summary>
    /// Contains information about the task's result.
    /// </summary>
    /// <example>Email sent to: foo</example>
    public string StatusString { get; private set; }

    internal Output(bool emailSent, string statusString)
    {
        EmailSent = emailSent;
        StatusString = statusString;
    }
}