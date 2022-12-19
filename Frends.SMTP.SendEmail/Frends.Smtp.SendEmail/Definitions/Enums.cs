namespace Frends.Smtp.SendEmail.Definitions;

/// <summary>
/// Attachment type enums.
/// </summary>
public enum AttachmentType
{
    /// <summary>
    /// Select this if the attachment is a file.
    /// </summary>
    FileAttachment,

    /// <summary>
    /// Select this if the attachment file should be created from a string.
    /// </summary>
    AttachmentFromString
}