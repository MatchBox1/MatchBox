using System;
using System.Net.Mail;

namespace MatchBox
{
    public class MailAction
    {
        private static string s_mail_from = "xxx@xxx.xxx";
        private static string s_mail_user = "xxx@xxx.xxx";
        private static string s_mail_password = "xxx";
        private static string s_smtp_host = "00.00.00.00";

        private static int n_smtp_port = 25;

        public static void Send_Mail(string s_mail_to, string s_header, string s_body, ref string s_error)
        {
            try
            {
                MailMessage o_mail_message = new MailMessage();

                o_mail_message.From = new MailAddress(s_mail_from);
                o_mail_message.To.Add(s_mail_to);
                o_mail_message.Subject = s_header;
                o_mail_message.Body = s_body;
                o_mail_message.IsBodyHtml = true;

                SmtpClient o_smtp_client = new SmtpClient(s_smtp_host, n_smtp_port);

                o_smtp_client.Credentials = new System.Net.NetworkCredential(s_mail_user, s_mail_password);

                o_smtp_client.Send(o_mail_message);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                return;
            }
        }
    }
}