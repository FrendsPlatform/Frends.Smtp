using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.SMTP.SendEmail
{
    public class Input
    {
        /// <summary>
        /// Recipient addresses separated by ',' or ';'
        /// </summary>
        /// <example>jane.doe@example.org</example>
        [DefaultValue("\"jane.doe@example.org\"")]
        public string To { get; set; }

        /// <summary>
        /// Cc recipient addresses separated by ',' or ';'
        /// </summary>
        /// <example>jane.doe@example.org</example>
        [DefaultValue("\"jane.doe@example.org\"")]
        public string Cc { get; set; }

        /// <summary>
        /// Bcc recipient addresses separated by ',' or ';'
        /// </summary>
        /// <example>jane.doe@example.org</example>
        [DefaultValue("\"jane.doe@example.org\"")]
        public string Bcc { get; set; }

        /// <summary>
        /// Sender address.
        /// </summary>
        /// <example>john.doe@example.org</example>
        [DefaultValue("\"john.doe@example.org\"")]
        public string From { get; set; }

        /// <summary>
        /// Name of the sender.
        /// </summary>
        /// <example>Frends</example>
        [DefaultValue("\"\"")]
        public string SenderName { get; set; }

        /// <summary>
        /// Email message's subject.
        /// </summary>
        /// <example>Hello Jane</example>
        [DefaultValue("\"Hello Jane\"")]
        public string Subject { get; set; }

        /// <summary>
        /// Body of the message.
        /// </summary>
        /// <example>You've got mail!</example>
        [DefaultValue("\"You've got mail!\"")]
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
        [DefaultValue("\"utf-8\"")]
        public string MessageEncoding { get; set; }

    }

    public class Options
    {
        /// <summary>
        /// SMTP server address.
        /// </summary>
        /// <example>smtp.somedomain.com</example>
        [DefaultValue("\"smtp.somedomain.com\"")]
        public string SMTPServer { get; set; }

        /// <summary>
        /// SMTP server port.
        /// </summary>
        /// <example>25</example>
        [DefaultValue("25")]
        public int Port { get; set; }

        /// <summary>
        /// Set this true if SMTP expects to be connected using SSL.
        /// </summary>
        /// <example>true</example>
        [DefaultValue("false")]
        public bool UseSsl { get; set; }

        /// <summary>
        /// Set this true if you want to use windows authentication to authenticate to SMTP server.
        /// </summary>
        /// <example>true</example>
        [DefaultValue("true")]
        public bool UseWindowsAuthentication { get; set; }

        /// <summary>
        /// Use this username to log in to the SMTP server
        /// </summary>
        /// <example>username</example>
        [DefaultValue("\"\"")]
        [UIHint(nameof(UseWindowsAuthentication), "", false)]
        public string UserName { get; set; }

        /// <summary>
        /// Use this password to log in to the SMTP server
        /// </summary>
        /// <example>Password123</example>
        [PasswordPropertyText(true)]
        [DefaultValue("\"\"")]
        [UIHint(nameof(UseWindowsAuthentication), "", false)]
        public string Password { get; set; }
    }

    public class Output
    {
        /// <summary>
        /// Value is true if email was sent.
        /// </summary>
        /// <example>true</example>
        public bool EmailSent { get; set; }
        /// <summary>
        /// Contains information about the task's result.
        /// </summary>
        /// <example>No attachments found matching path \"C:\\temp\\*.csv\". No email sent.</example>
        public string StatusString { get; set; }
    }

    public class Attachment
    {
        /// <summary>
        /// Chooses if the attachment file is created from a string or copied from disk.
        /// </summary>
        /// <example>FileAttachment</example>
        public AttachmentType AttachmentType { get; set; }

        [UIHint(nameof(AttachmentType), "", AttachmentType.AttachmentFromString)]
        public AttachmentFromString stringAttachment { get; set; }

        /// <summary>
        /// Attachment file's path. Uses Directory.GetFiles(string, string) as a pattern matching technique. See https://msdn.microsoft.com/en-us/library/wz42302f(v=vs.110).aspx.
        /// Exception: If the path ends in a directory, all files in that folder are added as attachments.
        /// </summary>
        /// <example>/my/attachment/dir</example>
        [DefaultValue("\"\"")]
        [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
        public string FilePath { get; set; }

        /// <summary>
        /// If set true and no files match the given path, an exception is thrown.
        /// </summary>
        /// <example>true</example>
        [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
        public bool ThrowExceptionIfAttachmentNotFound { get; set; }

        /// <summary>
        /// If set true and no files match the given path, email will be sent nevertheless.
        /// </summary>
        /// <example>true</example>
        [UIHint(nameof(AttachmentType), "", AttachmentType.FileAttachment)]
        public bool SendIfNoAttachmentsFound { get; set; }
    }
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

    public class AttachmentFromString
    {
        /// <summary>
        /// Content of the attachment file
        /// </summary>
        /// <example>My attachment content</example>
        [DefaultValue("\"\"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string FileContent { get; set; }

        /// <summary>
        /// Name of the attachment file
        /// </summary>
        /// <example>MyAttachment.txt</example>
        [DefaultValue("\"\"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string FileName { get; set; }
    }
}
