using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace DataAccessLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            string xml =
@"<?xml version=""1.0""?>
<goods xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <product>
    <name>Product 1</name>
    <amount>1</amount>
    <unitPrice>100</unitPrice>
    <tax>23</tax>
    <netValue>77.443</netValue>
    <grossValue>100</grossValue>
  </product>
  <product>
    <name>Product 2</name>
    <amount>1</amount>
    <unitPrice>100</unitPrice>
    <tax>23</tax>
    <netValue>77</netValue>
    <grossValue>100</grossValue>
  </product>
  <product>
    <name>Product 3</name>
    <amount>1</amount>
    <unitPrice>100</unitPrice>
    <tax>23</tax>
    <netValue>77</netValue>
    <grossValue>100</grossValue>
  </product>
</goods>";

            /* serializer = new XmlSerializer(typeof(Goods));
            Goods car = null;
            using (var reader = new StringReader(xml))
            {
                car = (Goods)serializer.Deserialize(reader);

                
                foreach(Product p in car.ProductList )
                {
                    Console.WriteLine(p.Name);
                }
            }

            XmlSerializer ser = new XmlSerializer(typeof(Goods));

            StringBuilder sb = new StringBuilder();
            using (StringWriter fs = new StringWriter(sb))
            {
                ser.Serialize(fs, car);
                Console.WriteLine(sb.ToString());
            }*/

            DateTime dt = DateTime.Now;

            Console.WriteLine(dt);

            Console.Read();
        }
    }
}
