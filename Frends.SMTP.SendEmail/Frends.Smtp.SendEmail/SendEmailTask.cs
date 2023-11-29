using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Frends.SMTP.SendEmail.Definitions;

namespace Frends.SMTP.SendEmail;
/// <summary>
/// Main class of the Task.
/// </summary>
public static class SMTP
{
    /// <summary>
    /// Sends email message with optional attachments.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.SMTP.SendEmail)
    /// </summary>
    /// <param name="message">Message parameters.</param>
    /// <param name="attachments">Parameters for adding attachments.</param>
    /// <param name="SMTPSettings">Connection parameters.</param>
    /// <param name="cancellationToken">Token given by Frends to terminate the Task.</param>
    /// <returns>Object { bool EmailSent, string StatusString }</returns>
    public static Result SendEmail([PropertyTab] Input message, [PropertyTab] AttachmentOptions attachments, [PropertyTab] Options SMTPSettings, CancellationToken cancellationToken)
    {
        using var client = InitializeSmtpClient(SMTPSettings);
        using var mail = InitializeMailMessage(message, cancellationToken);
        if (attachments != null)
            foreach (var attachment in attachments.Attachments)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (attachment.AttachmentType == AttachmentType.FileAttachment)
                {
                    ICollection<string> allAttachmentFilePaths = GetAttachmentFiles(attachment.FilePath);

                    if (attachment.ThrowExceptionIfAttachmentNotFound && allAttachmentFilePaths.Count == 0)
                        throw new FileNotFoundException($@"The given filepath ""attachment.FilePath"" had no matching files", attachment.FilePath);

                    if (allAttachmentFilePaths.Count == 0 && !attachment.SendIfNoAttachmentsFound)
                        return new Result(false, $@"No attachments found matching path ""{attachment.FilePath}"". No email sent.");

                    foreach (var fp in allAttachmentFilePaths)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        mail.Attachments.Add(new System.Net.Mail.Attachment(fp));
                    }
                }

                if (attachment.AttachmentType == AttachmentType.AttachmentFromString
                    && !string.IsNullOrEmpty(attachment.StringAttachment.FileContent))
                    mail.Attachments.Add(System.Net.Mail.Attachment.CreateAttachmentFromString
                        (attachment.StringAttachment.FileContent, attachment.StringAttachment.FileName));
            }

        client.Send(mail);

        return new Result(true, $"Email sent to: {mail.To}");
    }

    /// <summary>
    /// Initializes new SmtpClient with given parameters.
    /// </summary>
    private static SmtpClient InitializeSmtpClient(Options settings)
    {
        var smtpClient = new SmtpClient
        {
            Port = settings.Port,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = settings.UseWindowsAuthentication,
            EnableSsl = settings.UseSsl,
            Host = settings.SMTPServer
        };

        if (!settings.UseWindowsAuthentication && !string.IsNullOrEmpty(settings.UserName))
            smtpClient.Credentials = new NetworkCredential(settings.UserName, settings.Password);

        return smtpClient;
    }

    /// <summary>
    /// Initializes new MailMessage with given parameters. Uses default value 'true' for IsBodyHtml
    /// </summary>
    private static MailMessage InitializeMailMessage(Input input, CancellationToken cancellationToken)
    {
        //split recipients, either by comma or semicolon
        var separators = new[] { ',', ';' };

        string[] recipients = string.IsNullOrEmpty(input.To)
            ? Array.Empty<string>()
            : input.To.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] ccRecipients = string.IsNullOrEmpty(input.Cc)
            ? Array.Empty<string>()
            : input.Cc.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] bccRecipients = string.IsNullOrEmpty(input.Bcc)
            ? Array.Empty<string>()
            : input.Bcc.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        //Create mail object
        var mail = new MailMessage()
        {
            From = new MailAddress(input.From, input.SenderName),
            Subject = input.Subject,
            Body = input.Message,
            IsBodyHtml = input.IsMessageHtml
        };
        //Add recipients
        foreach (var recipientAddress in recipients)
        {
            cancellationToken.ThrowIfCancellationRequested();
            mail.To.Add(recipientAddress);
        }
        //Add CC recipients
        foreach (var ccRecipient in ccRecipients)
        {
            cancellationToken.ThrowIfCancellationRequested();
            mail.CC.Add(ccRecipient);
        }
        //Add BCC recipients
        foreach (var bccRecipient in bccRecipients)
        {
            cancellationToken.ThrowIfCancellationRequested();
            mail.Bcc.Add(bccRecipient);
        }
        //Set message encoding
        Encoding encoding = Encoding.GetEncoding(input.MessageEncoding);

        mail.BodyEncoding = encoding;
        mail.SubjectEncoding = encoding;

        return mail;
    }

    /// <summary>
    /// Gets all actual file names of attachments matching given file path
    /// </summary>
    private static ICollection<string> GetAttachmentFiles(string filePath)
    {
        string folder = Path.GetDirectoryName(filePath);
        string fileMask = Path.GetFileName(filePath) != "" ? Path.GetFileName(filePath) : "*";

        string[] filePaths = Directory.GetFiles(folder, fileMask);

        return filePaths;
    }
}
