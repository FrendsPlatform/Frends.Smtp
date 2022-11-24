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
    /// <example>jane.doe@somedomain.com;john.doe@somedomain.com</example>
    [DefaultValue("jane.doe@somedomain.com;john.doe@somedomain.com")]
    public string To { get; set; }

    /// <summary>
    /// Cc recipient addresses separated by ',' or ';'
    /// </summary>
    /// <example>jane.doe@somedomain.com;john.doe@somedomain.com</example>
    public string Cc { get; set; }

    /// <summary>
    /// Bcc recipient addresses separated by ',' or ';'
    /// </summary>
    /// <example>jane.doe@somedomain.com;john.doe@somedomain.com</example>
    public string Bcc { get; set; }

    /// <summary>
    /// Sender address.
    /// </summary>
    /// <example>josh.doe@somedomain.com</example>
    [DefaultValue("josh.doe@somedomain.com")]
    public string From { get; set; }

    /// <summary>
    /// Name of the sender.
    /// </summary>
    /// <example>Foo Bar</example>
    public string SenderName { get; set; }

    /// <summary>
    /// Email message's subject.
    /// </summary>
    /// <example>Hello</example>
    public string Subject { get; set; }

    /// <summary>
    /// Body of the message.
    /// </summary>
    /// <example>You've got mail</example>
    public string Message { get; set; }

    /// <summary>
    /// Set this true if the message is HTML.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool IsMessageHtml { get; set; }

    /// <summary>
    /// Encoding of message body and subject. Use following table's name column for other options. https://msdn.microsoft.com/en-us/library/system.text.encoding(v=vs.110).aspx#Anchor_5 
    /// </summary>
    /// <example>utf-8</example>
    [DefaultValue("utf-8")]
    public string MessageEncoding { get; set; }
}