﻿using System;
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
/// SMTP Task.
/// </summary>
public class SMTP
{
    /// <summary>
    /// Frends Task for sending emails with optional attachments using SMTP protocol.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.SMTP.SendEmail)
    /// <param name="message">Input parameters</param>
    /// <param name="attachments">Attachment parameters</param>
    /// <param name="SMTPSettings">Options parameters</param>
    /// <param name="cancellationToken">Token generated by Frends to stop this task.</param>
    /// </summary>
    /// <returns> Object { bool EmailSent, string StatusString } </returns>
    public static Output SendEmail([PropertyTab] Input message, [PropertyTab] Attachments attachments, [PropertyTab] Options SMTPSettings, CancellationToken cancellationToken)
    {
        using var client = InitializeSmtpClient(SMTPSettings);
        using var mail = InitializeMailMessage(message);
        if (attachments != null)
        {
            switch (attachments.AttachmentType)
            {
                case AttachmentType.FileAttachment:
                    ICollection<string> allAttachmentFilePaths = GetAttachmentFiles(attachments.FilePath);

                    if (attachments.ThrowExceptionIfAttachmentNotFound && allAttachmentFilePaths.Count == 0)
                        throw new FileNotFoundException(string.Format("The given filepath \"{0}\" had no matching files", attachments.FilePath), attachments.FilePath);

                    if (allAttachmentFilePaths.Count == 0 && !attachments.SendIfNoAttachmentsFound)
                        return new Output(false, string.Format("No attachments found matching path \"{0}\". No email sent.", attachments.FilePath));

                    foreach (var fp in allAttachmentFilePaths)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        mail.Attachments.Add(new Attachment(fp));
                    }
                    break;
                case AttachmentType.AttachmentFromString:
                    foreach (var attachment in attachments.StringAttachment)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        mail.Attachments.Add(Attachment.CreateAttachmentFromString(attachment.FileContent, attachment.FileName));
                    }
                    break;
            }
        }
        client.Send(mail);
        return new Output(true, string.Format("Email sent to: {0}", mail.To.ToString()));
    }

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

    private static MailMessage InitializeMailMessage(Input input)
    {
        //split recipients, either by comma or semicolon
        var separators = new[] { ',', ';' };

        string[] recipients = input.To.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] ccRecipients = input.Cc.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] bccRecipients = input.Bcc.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
            mail.To.Add(recipientAddress);

        //Add CC recipients
        foreach (var ccRecipient in ccRecipients)
            mail.CC.Add(ccRecipient);

        //Add BCC recipients
        foreach (var bccRecipient in bccRecipients)
            mail.Bcc.Add(bccRecipient);

        //Set message encoding
        Encoding encoding = Encoding.GetEncoding(input.MessageEncoding);

        mail.BodyEncoding = encoding;
        mail.SubjectEncoding = encoding;

        return mail;
    }

    private static ICollection<string> GetAttachmentFiles(string filePath)
    {
        string folder = Path.GetDirectoryName(filePath);
        string fileMask = Path.GetFileName(filePath) != "" ? Path.GetFileName(filePath) : "*";

        string[] filePaths = Directory.GetFiles(folder, fileMask);

        return filePaths;
    }
}