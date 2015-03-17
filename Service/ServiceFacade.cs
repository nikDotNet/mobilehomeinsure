using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileHome.Insure.DAL;
using MobileHome.Insure.DAL.EF;
using MobileHome.Insure.Model;
using System.Net.Mail;

namespace MobileHome.Insure.Service
{
    public class ServiceFacade : IServiceFacade
    {
        private readonly mhappraisalContext _context;
        public ServiceFacade()
        {
            _context = new mhappraisalContext();
        }

        
        public void saveState(State objState)
        {
        
        }

        public void sendMail(string sender, string senderMail, string subject, string message)
        {
            MailMessage messageObject = new MailMessage(senderMail, "info@mobilehome.insure", subject, message);
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;

            smtp.Credentials = new System.Net.NetworkCredential("info@mobilehome.insure", "MobileHome2015");
            smtp.Send(messageObject);

        }

    }
}
