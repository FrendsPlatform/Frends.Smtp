using NUnit.Framework;
using System;
using System.IO;
using Frends.SMTP.SendEmail.Definitions;

namespace Frends.SMTP.SendEmail.Tests;

[TestFixture]
public class SendEmailTests
{
    // ****************************************** FILL THESE ******************************************************
    private static readonly string USERNAME = Environment.GetEnvironmentVariable("Frends_SMTP_Username");
    private static readonly string PASSWORD = Environment.GetEnvironmentVariable("Frends_SMTP_Password");
    private static readonly string SMTPADDRESS = Environment.GetEnvironmentVariable("Frends_SMTP_Address");
    private static readonly string TOEMAILADDRESS = Environment.GetEnvironmentVariable("Frends_SMTP_Email");
    private static readonly string FROMEMAILADDRESS = Environment.GetEnvironmentVariable("Frends_SMTP_Email");
    private const int PORT = 587;
    private const bool USESSL = true;
    private const bool USEWINDOWSAUTHENTICATION = false;
    // ************************************************************************************************************


    private const string TEMP_ATTACHMENT_SOURCE = "emailtestattachments";
    private const string TEST_FILE_NAME = "testattachment.txt";
    private const string TEST_FILE_NOT_EXISTING = "doesntexist.txt";

    private string _localAttachmentFolder;
    private string _filepath;
    private Input _input;
    private Input _input2;
    private Options _options;

    [SetUp]
    public void EmailTestSetup()
    {
        _localAttachmentFolder = Path.Combine(Path.GetTempPath(), TEMP_ATTACHMENT_SOURCE);

        if (!Directory.Exists(_localAttachmentFolder))
            Directory.CreateDirectory(_localAttachmentFolder);

        _filepath = Path.Combine(_localAttachmentFolder, TEST_FILE_NAME);

        if (!File.Exists(_filepath))
        {
            File.Create(_filepath).Dispose();
        }

        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8"
        };

        _input2 = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = null,
            Bcc = null,
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8"
        };

        _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = USESSL,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

    }
    [TearDown]
    public void EmailTestTearDown()
    {
        if (Directory.Exists(_localAttachmentFolder))
            Directory.Delete(_localAttachmentFolder, true);
    }

    [Test]
    public void SendEmailWithPlainText()
    {
        var input = _input;
        input.Subject = "Email test - PlainText";

        var result = SMTP.SendEmail(input, null, _options, default);
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void SendEmailWithFileAttachment()
    {
        var input = _input;
        input.Subject = "Email test - FileAttachment";

        var attachment = new Attachment
        {
            AttachmentType = AttachmentType.FileAttachment,
            FilePath = _filepath,
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = true
        };


        var Attachments = new AttachmentOptions { Attachments = new Attachment[] { attachment } };

        var result = SMTP.SendEmail(input, Attachments, _options, default);
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void SendEmailWithStringAttachment()
    {
        var input = _input;
        input.Subject = "Email test - AttachmentFromString";
        var fileAttachment = new AttachmentFromString() { FileContent = "teststring � �", FileName = "testfilefromstring.txt" };
        var attachment = new Attachment()
        {
            AttachmentType = AttachmentType.AttachmentFromString,
            StringAttachment = fileAttachment
        };
        var Attachments = new AttachmentOptions { Attachments = new Attachment[] { attachment } };

        var result = SMTP.SendEmail(input, Attachments, _options, default);
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void TrySendingEmailWithNoFileAttachmentFound()
    {
        var input = _input;
        input.Subject = "Email test";

        var attachment = new Attachment
        {
            FilePath = Path.Combine(_localAttachmentFolder, TEST_FILE_NOT_EXISTING),
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = false
        };


        var Attachments = new AttachmentOptions { Attachments = new Attachment[] { attachment } };

        var result = SMTP.SendEmail(input, Attachments, _options, default);
        Assert.IsFalse(result.EmailSent);
    }

    [Test]
    public void TrySendingEmailWithNoCcAndBcc()
    {
        var input = _input2;
        input.Subject = "Email test";

        var attachment = new Attachment
        {
            FilePath = Path.Combine(_localAttachmentFolder, TEST_FILE_NOT_EXISTING),
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = false
        };


        var Attachments = new AttachmentOptions { Attachments = new Attachment[] { attachment } };

        var result = SMTP.SendEmail(input, Attachments, _options, default);
        Assert.IsFalse(result.EmailSent);
    }

    [Test]
    public void TrySendingEmailWithNoFileAttachmentFoundException()
    {
        var input = _input;
        input.Subject = "Email test";

        var attachment = new Attachment
        {
            FilePath = Path.Combine(_localAttachmentFolder, TEST_FILE_NOT_EXISTING),
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = true
        };


        var Attachments = new AttachmentOptions { Attachments = new Attachment[] { attachment } };

        Assert.Throws<FileNotFoundException>(() => SMTP.SendEmail(input, Attachments, _options, default));

    }
}
