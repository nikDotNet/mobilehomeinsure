using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MobileHoome.Insure.ExtService.Helpers
{
    public static class Extensions
    {
        public static XElement ToXml(this object obj)
        {
            //XmlSerializer s = new XmlSerializer(obj.GetType());
            //using (StringWriter writer = new StringWriter())
            //{
            //    s.Serialize(writer, obj);
            //    return writer.ToString();
            //}


            //var ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            //var sw = new StringWriter();
            //using (var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, ConformanceLevel = ConformanceLevel.Auto }))
            //{
            //    XmlSerializer s = new XmlSerializer(obj.GetType());
            //    s.Serialize(xmlWriter, obj, ns);
            //    return sw.ToString();
            //}



            XmlSerializer xs = new XmlSerializer(obj.GetType());
            XDocument d = new XDocument();
            using (XmlWriter xw = d.CreateWriter())
                xs.Serialize(xw, obj);
            d.Root.RemoveAttributes();
            return d.Root;
        }

        public static T FromXml<T>(this string data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
    }
}
