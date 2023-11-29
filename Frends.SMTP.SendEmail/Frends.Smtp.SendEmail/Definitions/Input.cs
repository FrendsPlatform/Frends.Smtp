using System.ComponentModel;

namespace Frends.SMTP.SendEmail.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Recipient addresses separated by ',' or ';'
    /// </summary>
    /// <example>jane.doe@somedomain.com</example>
    [DefaultValue("jane.doe@somedomain.com")]
    public string To { get; set; }

    /// <summary>
    /// Cc recipient addresses separated by ',' or ';'
    /// </summary>
    /// <example>jane.doe@somedomain.com</example>
    [DefaultValue("jane.doe@somedomain.com")]
    public string Cc { get; set; }

    /// <summary>
    /// Bcc recipient addresses separated by ',' or ';'
    /// </summary>
    /// <example>jane.doe@somedomain.com</example>
    [DefaultValue("jane.doe@somedomain.com")]
    public string Bcc { get; set; }

    /// <summary>
    /// Sender address.
    /// </summary>
    /// <example>jane.doe@somedomain.com</example>
    [DefaultValue("john.doe@somedomain.com")]
    public string From { get; set; }

    /// <summary>
    /// Name of the sender.
    /// </summary>
    /// <example>Jane Doe</example>
    [DefaultValue("")]
    public string SenderName { get; set; }

    /// <summary>
    /// Email message's subject.
    /// </summary>
    /// <example>Hello Jane</example>
    [DefaultValue("Hello Jane")]
    public string Subject { get; set; }

    /// <summary>
    /// Body of the message.
    /// </summary>
    /// <exmaple>You've got mail!</exmaple>
    [DefaultValue("You've got mail!")]
    public string Message { get; set; }

    /// <summary>
    /// Set this true if the message is HTML.
    /// </summary>
    /// <example>true</example>
    [DefaultValue("false")]
    public bool IsMessageHtml { get; set; }

    /// <summary>
    /// Encoding of message body and subject. Use following table's name column for other options. https://msdn.microsoft.com/en-us/library/system.text.encoding(v=vs.110).aspx#Anchor_5 
    /// </summary>
    /// <example>utf-8</example>
    [DefaultValue("utf-8")]
    public string MessageEncoding { get; set; }

}

