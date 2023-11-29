using System.ComponentModel;
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
    [DefaultValue("smtp.somedomain.com")]
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
    /// <exmaple>true</exmaple>
    [DefaultValue("false")]
    public bool UseSsl { get; set; }

    /// <summary>
    /// Set this true if you want to use windows authentication to authenticate to SMTP server.
    /// </summary>
    /// <example>false</example>
    [DefaultValue("true")]
    public bool UseWindowsAuthentication { get; set; }

    /// <summary>
    /// Use this username to log in to the SMTP server
    /// </summary>
    /// <example>testuser</example>
    [DefaultValue("")]
    [UIHint(nameof(UseWindowsAuthentication), "", false)]
    public string UserName { get; set; }

    /// <summary>
    /// Use this password to log in to the SMTP server
    /// </summary>
    /// <example>Password123</example>
    [PasswordPropertyText(true)]
    [DefaultValue("")]
    [UIHint(nameof(UseWindowsAuthentication), "", false)]
    public string Password { get; set; }
}
