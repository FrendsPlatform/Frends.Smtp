using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Smtp.SendEmail.Definitions;

/// <summary>
/// Attachment parameters.
/// </summary>
public class Attachments
{
    /// <summary>
    /// Chooses if the attachment file is created from a string or copied from disk.
    /// </summary>
    /// <example>AttachmentType.FileAttachment)</example>
    [DefaultValue(AttachmentType.FileAttachment)]
    public AttachmentType AttachmentType { get; set; }

    /// <summary>
    /// String to create attachment file.
    /// </summary>
    /// <example>{ FileContent = "foo", FileName = "bar.txt" }</example>
    [UIHint(nameof(AttachmentType), "", AttachmentType.AttachmentFromString)]
    public AttachmentFromString[] StringAttachment { get; set; }

    /// <summary>
    /// Attachment file's path. Uses Directory.GetFiles(string, string) as a pattern matching technique. See https://msdn.microsoft.com/en-us/library/wz42302f(v=vs.110).aspx.
    /// Exception: If the path ends in a directory, all files in that folder are added as attachments.
    /// </summary>
    /// <example>c:\temp</example>
    [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
    public string FilePath { get; set; }

    /// <summary>
    /// If set true and no files match the given path, an exception is thrown.
    /// </summary>
    /// <example>false</example>
    [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
    public bool ThrowExceptionIfAttachmentNotFound { get; set; }

    /// <summary>
    /// If set true and no files match the given path, email will be sent nevertheless.
    /// </summary>
    /// <example>false</example>
    [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
    public bool SendIfNoAttachmentsFound { get; set; }
}

/// <summary>
/// AttachmentFromString values
/// </summary>
public class AttachmentFromString
{
    /// <summary>
    /// Content of the attachment file
    /// </summary>
    /// <example>Foo</example>
    public string FileContent { get; set; }

    /// <summary>
    /// Name of the attachment file
    /// </summary>
    /// <example>Bar</example>
    public string FileName { get; set; }
}