using Frends.SMTP.SendEmail.Definitions;
using NUnit.Framework;
using System;
using System.IO;

namespace Frends.SMTP.SendEmail.Tests
{
    /// <summary>
    /// NOTE: To run these unit tests, you need an SMTP test server. Fill in the properties below with your values.
    /// </summary>
    [TestFixture]
    public class SendEmailTests
    {

        // ****************************************** FILL THESE ******************************************************
        private const string USERNAME = "apikey";
        private const string PASSWORD = "password";
        private const string SMTPADDRESS = "smtp.sendgrid.net";
        private const string TOEMAILADDRESS = "jefim.borissov@hiq.fi";
        private const string FROMEMAILADDRESS = "jefim.borissov@hiq.fi";
        private const int PORT = 587;
        private const bool USESSL = true;
        private const bool USEWINDOWSAUTHENTICATION = false;
        // ************************************************************************************************************

        private const string TEMP_ATTACHMENT_SOURCE = "emailtestattachments";
        private const string TEST_FILE_NAME = "testattachment.txt";
        private const string TEST_FILE_NOT_EXISTING = "doesntexist.txt";
        private readonly string passwordFromEnvironment = Environment.GetEnvironmentVariable("SMTP_PASSWORD");


        private string _localAttachmentFolder;
        private string _filepath;

        [SetUp]
        public void EmailTestSetup()
        {
            _localAttachmentFolder = Path.Combine(Path.GetTempPath(), TEMP_ATTACHMENT_SOURCE);

            if (!Directory.Exists(_localAttachmentFolder))
                Directory.CreateDirectory(_localAttachmentFolder);

            _filepath = Path.Combine(_localAttachmentFolder, TEST_FILE_NAME);
            
            var file = File.Create(_filepath);
            file.Close();
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
            var input = new Input()
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

            var _options = new Options()
            {
                UserName = USERNAME,
                Password = string.IsNullOrWhiteSpace(passwordFromEnvironment) ? PASSWORD : passwordFromEnvironment,
                SMTPServer = SMTPADDRESS,
                Port = PORT,
                UseSsl = USESSL,
                UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
            };

            var result = SMTP.SendEmail(input, null, _options, new System.Threading.CancellationToken());
            Assert.IsTrue(result.EmailSent);
        }

        [Test]
        public void SendEmailWithFileAttachment()
        {
            var input = new Input()
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

            var _options = new Options()
            {
                UserName = USERNAME,
                Password = string.IsNullOrWhiteSpace(passwordFromEnvironment) ? PASSWORD : passwordFromEnvironment,
                SMTPServer = SMTPADDRESS,
                Port = PORT,
                UseSsl = USESSL,
                UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
            };

            var attachment = new Attachments
            {
                FilePath = _filepath,
                SendIfNoAttachmentsFound = false,
                ThrowExceptionIfAttachmentNotFound = true,
                StringAttachment = null
            };

            var result = SMTP.SendEmail(input, attachment, _options, new System.Threading.CancellationToken());
            Assert.IsTrue(result.EmailSent);
        }

        [Test]
        public void SendEmailWithStringAttachment()
        {
            var input = new Input()
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
                Password = string.IsNullOrWhiteSpace(passwordFromEnvironment) ? PASSWORD : passwordFromEnvironment,
                SMTPServer = SMTPADDRESS,
                Port = PORT,
                UseSsl = USESSL,
                UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
            };

            var result = SMTP.SendEmail(input, attachment, _options, new System.Threading.CancellationToken());
            Assert.IsTrue(result.EmailSent);
        }

        [Test]
        public void TrySendingEmailWithNoFileAttachmentFound()
        {
            var input = new Input()
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

            var _options = new Options()
            {
                UserName = USERNAME,
                Password = string.IsNullOrWhiteSpace(passwordFromEnvironment) ? PASSWORD : passwordFromEnvironment,
                SMTPServer = SMTPADDRESS,
                Port = PORT,
                UseSsl = USESSL,
                UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
            };

            var result = SMTP.SendEmail(input, attachment, _options, new System.Threading.CancellationToken());
            Assert.IsFalse(result.EmailSent);
        }

        [Test]
        public void TrySendingEmailWithNoFileAttachmentFoundException()
        {
            var input = new Input()
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

            var _options = new Options()
            {
                UserName = USERNAME,
                Password = string.IsNullOrWhiteSpace(passwordFromEnvironment) ? PASSWORD : passwordFromEnvironment,
                SMTPServer = SMTPADDRESS,
                Port = PORT,
                UseSsl = USESSL,
                UseWindowsAuthentication = USEWINDOWSAUTHENTICATION,
            };

            Assert.Throws<FileNotFoundException>(() => SMTP.SendEmail(input, attachment, _options, new System.Threading.CancellationToken()));
        }
    }
}
