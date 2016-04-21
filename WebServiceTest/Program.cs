using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTest.AegisRental;
using System.Xml;

namespace WebServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceSoapClient sClient = new ServiceSoapClient();
            sClient.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\Google Drive\TFS-Nik\ColonialGroup\teammobilehomeinsure\mobilehomeinsure\WebServiceTest\SampleXML\xmlnodesample.xml");
            XmlNode xnode = doc.FirstChild;

            XmlNode result = sClient.QuotePolicy("62ba120e-839f-441a-a460-462cfa86a6b2", xnode, "AGMH", AstecProcessingMode.SubmitOverride);
            Console.ReadLine();
        }
    }
}
