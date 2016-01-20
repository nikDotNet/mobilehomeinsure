using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using System.Net.Mail;
using MobileHome.Insure.Service.Helper;
using MobileHome.Insure.Service.Helper.Constants;

namespace MobileHome.Insure.Service
{
    public class ServiceFacade : IServiceFacade
    {
        private readonly mhappraisalContext _context;
        private readonly mhRentalContext _rentalContext;
        
        public ServiceFacade()
        {
            _context = new mhappraisalContext();
            _rentalContext = new mhRentalContext();
        }

        
        public void saveState(State objState)
        {
        
        }

        private string getUnsubscribeLink(string emailId)
        {
            return "<a href=http://test.mobilehome.insure/Unsubscribe?user=" + CryptoHelper.Encrypt(emailId) + ">here</a>";
        }

        public void sendMail(string from, string to, string subject, string message, List<string> lstEmail = null, bool isOrderMail = false)
        {
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpHost"], int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]));
            smtp.EnableSsl = false;
             MailAddress fromMailAddress = null;
            if (!isOrderMail)
            {
                smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["InfoEmail"], System.Configuration.ConfigurationManager.AppSettings["InfoEmailPassword"]);
                fromMailAddress = new MailAddress(from, "MobileHome.Insure");
            }
            else
            {
                smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["OrdersEmail"], System.Configuration.ConfigurationManager.AppSettings["OrdersEmailPassword"]);
                fromMailAddress = new MailAddress(from, "Orders - MobileHome.Insure");
            }

            if (lstEmail == null)
                lstEmail = new List<string>();

            if(to!= "" && !lstEmail.Contains(to))
                lstEmail.Add(to);

           lstEmail = filterUnsubscribedEmails(lstEmail);

            foreach(var emailTo in lstEmail)
            {
                
                MailMessage messageObject = new MailMessage(fromMailAddress, new MailAddress(emailTo));
                messageObject.Subject = subject;
                messageObject.Body = message + " <br />" + ApplicationConstants.UnsubscribeText.Replace("here", getUnsubscribeLink(emailTo)); 
                
                messageObject.IsBodyHtml = true;
                smtp.Send(messageObject);
            }
        }

        public bool Unsubscribe(string encryptedText)
        {
            string customerEmail = CryptoHelper.Decrypt(encryptedText);
            var customerObjs = (from customer in _rentalContext.Customers
                               where customer.Email == customerEmail
                               select customer);
            foreach (var customerObj in customerObjs)
            {
                 customerObj.IsUnsubscribed = true;
                _rentalContext.Entry(customerObj).State = System.Data.Entity.EntityState.Modified;
               
            }
            try
            {
                if (customerObjs.Count() != 0)
                {
                    _rentalContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public List<string> filterUnsubscribedEmails(List<string> lstEmail)
        {
            var query = from customer in _rentalContext.Customers
                        join emailInList in lstEmail on customer.Email equals emailInList
                        where customer.IsUnsubscribed == true
                        select emailInList;
            return lstEmail.Except(query.Distinct()).ToList();
        }

    }
}
