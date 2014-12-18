using System;
using System.Net.Mail;



    public class SendEmail
    {
        static bool SendEmail(string sTo, string sCC, string sBcc, string sFrom, string sFromDisplayName, string sSubject, string sBody, string sMailServer, string sPort, string sUserName, string sPassword, bool IsHtml, string attachmentFilename)
        {
            System.Net.NetworkCredential credentials;
            MailMessage mailMsg = null;
            MailAddress mailAddress;
            SmtpClient smtpClient = null;
            if (sPort == "")
            {
                sPort = "25";
            }
            try
            {
                mailMsg = new MailMessage();
                mailMsg.To.Add(sTo);
                if (sBcc != null && sBcc.Length > 0)
                {
                    mailMsg.Bcc.Add(sBcc);
                }
                if (sCC != null && sCC.Length > 0)
                {
                    mailMsg.CC.Add(sCC);
                }
                mailAddress = new MailAddress(sFrom, sFromDisplayName);
                mailMsg.From = mailAddress;
                mailMsg.Subject = sSubject;
                mailMsg.Body = sBody;
                if (IsHtml == true)
                {
                    mailMsg.IsBodyHtml = true;
                }
                else
                {
                    sBody = sBody.Replace("<br>", System.Environment.NewLine);
                    sBody = sBody.Replace("<Br>", System.Environment.NewLine);
                    sBody = sBody.Replace("<BR>", System.Environment.NewLine);
                    sBody = sBody.Replace("<BR />", System.Environment.NewLine);
                    sBody = sBody.Replace("<br />", System.Environment.NewLine);
                    sBody = sBody.Replace("<Br />", System.Environment.NewLine);
                    sBody = sBody.Replace("<br></br>", System.Environment.NewLine);
                    sBody = sBody.Replace("<Br></Br>", System.Environment.NewLine);
                    sBody = sBody.Replace("<BR></BR>", System.Environment.NewLine);
                    mailMsg.IsBodyHtml = false;
                }
                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mailMsg.Attachments.Add(attachment);
                }

                smtpClient = new SmtpClient(sMailServer, Convert.ToInt32(sPort));

                try
                {
                    if (sUserName.Length > 0)
                    {
                        credentials = new System.Net.NetworkCredential(sUserName, sPassword);
                        smtpClient.Credentials = credentials;
                    }
                }
                catch { }
            }
            catch { }
            try
            {
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
                return false;
            }
            finally
            {
                try
                {
                    mailMsg.Dispose();
                    smtpClient.Dispose();
                    credentials = null;
                }
                catch
                { }
            }
            return true;
        }

    }
    }
