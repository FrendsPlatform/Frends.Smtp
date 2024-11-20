﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.SMTP.SendEmail.Definitions;

/// <summary>
/// SMTP Options parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// SMTP server address.
    /// </summary>
    /// <example>smtp.somedomain.com</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("smtp.somedomain.com")]
    public string SMTPServer { get; set; }

    /// <summary>
    /// SMTP server port.
    /// </summary>
    /// <example>25</example>
    [DefaultValue("25")]
    public int Port { get; set; }

    /// <summary>
    /// Choose the SecureSocketOption to use, default is Auto
    /// </summary>
    /// <example>SecureSocketOption.None</example>
    [DefaultValue(SecureSocketOption.Auto)]
    public SecureSocketOption SecureSocket { get; set; }

    /// <summary>
    /// WARNING: Setting AcceptAllCerts to true disables SSL/TLS certificate validation.
    /// This should only be used in development/test environments with self-signed certificates.
    /// Using this option in production environments poses significant security risks.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool AcceptAllCerts { get; set; }

    /// <summary>
    /// Set this true if SMTP server expectes OAuth token.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool UseOAuth2 { get; set; }

    /// <summary>
    /// Token to be used when using OAuth2.
    /// </summary>
    /// <example>cec4ce4f98e4f68e4vc89v1489v4987s4erv8794...</example>
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(UseOAuth2), "", true)]
    [PasswordPropertyText]
    public string Token { get; set; }

    /// <summary>
    /// Use this username to log in to the SMTP server
    /// </summary>
    /// <example>testuser</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string UserName { get; set; }

    /// <summary>
    /// Use this password to log in to the SMTP server
    /// </summary>
    /// <example>Password123</example>
    [PasswordPropertyText(true)]
    [DefaultValue("")]
    [UIHint(nameof(UseOAuth2), "", false)]
    public string Password { get; set; }
}
