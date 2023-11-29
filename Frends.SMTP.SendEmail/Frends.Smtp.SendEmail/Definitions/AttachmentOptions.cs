namespace Frends.SMTP.SendEmail.Definitions;

/// <summary>
/// Parameters for attachments.
/// </summary>
public class AttachmentOptions
{
    /// <summary>
    /// Array of Attachments to be send with the message.
    /// </summary>
    /// <example>[ 
    ///     Attachment { AttachmentType = AttachmentType.FileAttachment, FilePath = C:\pathToTheFile\test.txt, SendIfNoAttachmentsFound = false, ThrowExceptionIfAttachmentNotFound = true },
    ///     Attachment { AttachmentType = AttachmentType.AttachmentFromString, AttachmentFromString { FileContent = This is test file, FileName = testfile.txt } } ]</example>
    public Attachment[] Attachments { get; set; }
}

