using System.ComponentModel;

namespace Frends.SMTP.SendEmail.Definitions;

/// <summary>
/// Parameters for adding attachment from string.
/// </summary>
public class AttachmentFromString
{
    /// <summary>
    /// Content of the attachment file
    /// </summary>
    [DefaultValue("")]
    public string FileContent { get; set; }

    /// <summary>
    /// Name of the attachment file
    /// </summary>
    [DefaultValue("")]
    public string FileName { get; set; }
}

