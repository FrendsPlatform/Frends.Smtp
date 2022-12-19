using Frends.SMTP.SendEmail.Definitions;
using NUnit.Framework;
using System.IO;

namespace Frends.SMTP.SendEmail.Tests;

/// <summary>
/// NOTE: To run these unit tests, you need an SMTP test server.
/// 
/// docker run -p 3000:80 -p 2525:25 -d rnwood/smtp4dev:v3
///  
/// Management console can be found from http://localhost:3000/
/// 
/// </summary>
[TestFixture]
public class SendEmailTests
{
    private const string USERNAME = "test";
    private const string PASSWORD = "test";
    private const string SMTPADDRESS = "localhost";
    private const string TOEMAILADDRESS = "test@test.com";
    private const string FROMEMAILADDRESS = "user@user.com";
    private const int PORT = 2525;
    private const string TEMP_ATTACHMENT_SOURCE = "emailtestattachments";
    private const string TEST_FILE_NAME = "testattachment.txt";
    private const string TEST_FILE_NOT_EXISTING = "doesntexist.txt";
    private string _localAttachmentFolder;
    private string _filepath;
    private const bool USEWINDOWSAUTHENTICATION = false;

    private Input _input;
    private Options _options;

    [SetUp]
    public void EmailTestSetup()
    {
        _localAttachmentFolder = Path.Combine(Path.GetTempPath(), TEMP_ATTACHMENT_SOURCE);
        if (!Directory.Exists(_localAttachmentFolder)) Directory.CreateDirectory(_localAttachmentFolder);
        _filepath = Path.Combine(_localAttachmentFolder, TEST_FILE_NAME);
        if (!File.Exists(_filepath)) using (File.Create(_filepath)) { }
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
        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8",
            Subject = "Email test - PlainText"
        };

        _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = false,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

        var result = SMTP.SendEmail(_input, null, _options, default);
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void SendEmailWithFileAttachment()
    {
        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8",
            Subject = "Email test - FileAttachment",
        };

        _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = false,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

        var attachment = new Attachments
        {
            FilePath = _filepath,
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = true,
            StringAttachment = null
        };

        var result = SMTP.SendEmail(_input, attachment, _options, default);
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void SendEmailWithStringAttachment()
    {
        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8",
            Subject = "Email test - AttachmentFromString",
        };

        var attachment = new Attachments()
        {
            AttachmentType = AttachmentType.AttachmentFromString,
            StringAttachment = new[] { new AttachmentFromString() { FileContent = "teststring ä ö", FileName = "testfilefromstring.txt" } }
        };

        var _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = false,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

        var result = SMTP.SendEmail(_input, attachment, _options, new System.Threading.CancellationToken());
        Assert.IsTrue(result.EmailSent);
    }

    [Test]
    public void TrySendingEmailWithNoFileAttachmentFound()
    {
        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8",
            Subject = "Email test",
        };

        var attachment = new Attachments
        {
            FilePath = Path.Combine(_localAttachmentFolder, TEST_FILE_NOT_EXISTING),
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = false
        };

        _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = false,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

        var result = SMTP.SendEmail(_input, attachment, _options, new System.Threading.CancellationToken());
        Assert.IsFalse(result.EmailSent);
    }

    [Test]
    public void TrySendingEmailWithNoFileAttachmentFoundException()
    {
        _input = new Input()
        {
            From = FROMEMAILADDRESS,
            To = TOEMAILADDRESS,
            Cc = "",
            Bcc = "",
            Message = "testmsg",
            IsMessageHtml = false,
            SenderName = "EmailTestSender",
            MessageEncoding = "utf-8",
            Subject = "Email test",
        };

        var attachment = new Attachments
        {
            FilePath = Path.Combine(_localAttachmentFolder, TEST_FILE_NOT_EXISTING),
            SendIfNoAttachmentsFound = false,
            ThrowExceptionIfAttachmentNotFound = true
        };

        _options = new Options()
        {
            UserName = USERNAME,
            Password = PASSWORD,
            SMTPServer = SMTPADDRESS,
            Port = PORT,
            UseSsl = false,
            UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
        };

        Assert.Throws<FileNotFoundException>(() => SMTP.SendEmail(_input, attachment, _options, new System.Threading.CancellationToken()));
    }
}