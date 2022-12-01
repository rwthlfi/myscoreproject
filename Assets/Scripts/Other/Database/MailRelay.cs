using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web;
using UnityEngine;

public static class MailRelay
{

    public enum configMailKey { user, host, port, code }

    /// <summary>
    /// to send email to the give recepient
    /// </summary>
    /// <param name="_recepient">the email of the recepient</param>
    /// <param name="_subject">email subject</param>
    /// <param name="_content">the content of the email</param>
    public static bool SendEmail(List<string>_configKey, string _recepient, string _subject, string _content)
    {
        string to = _recepient;
        string from = _configKey[(int)configMailKey.user];
        MailMessage message = new MailMessage(from, to);
        message.Subject = _subject;
        message.Body = _content;

        SmtpClient client = new SmtpClient()
        {
            Host = _configKey[(int)configMailKey.host],
            Port = ConverterFunction.StringToInt(_configKey[(int)configMailKey.port]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential()
            {
                UserName = from,
                Password = _configKey[(int)configMailKey.code]
            }
        };

        try
        {
            client.Send(message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                ex.ToString());

            return false;
        }
    }


    /// <summary>
    /// To sent email without any pass this one we use the MyScore Email for it.
    /// </summary>
    /// <param name="_recepient"></param>
    /// <param name="_subject"></param>
    /// <param name="_content"></param>
    public static bool SendEmail_NoPass(List<string> _configKey, string _recepient, string _subject, string _content)
    {

        string from = _configKey[(int)configMailKey.user];
        string host = _configKey[(int)configMailKey.host];
        int port = ConverterFunction.StringToInt(_configKey[(int)configMailKey.port]);

        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient();
        mail.To.Add(_recepient);
        mail.From = new MailAddress(from);
        mail.Subject = _subject;
        mail.IsBodyHtml = true;
        mail.Body = _content;

        SmtpServer.Host = host;
        SmtpServer.Port = port;
        SmtpServer.EnableSsl = true;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        try
        {
            SmtpServer.Send(mail);
            Debug.Log("Email is sent!");
            return true;
        }


        catch (Exception ex)
        {
            Debug.Log("Exception Message: " + ex.Message);
            if (ex.InnerException != null)
                Debug.Log("Exception Inner:   " + ex.InnerException);
            return false;
        }
    }

}
