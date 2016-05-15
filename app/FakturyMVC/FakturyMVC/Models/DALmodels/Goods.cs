using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace FakturyMVC.Models.DALmodels
{
    [XmlRoot("goods")]
    public class Goods
    {
        [XmlElement("product", typeof(Product))]
        public List<Product> ProductList { get; set; }

        public Goods(List<Product> productList)
        {
            ProductList = productList;
        }

        public Goods()
        {

        }

        public static Goods Deserialize(string xml)
        {
            Goods deserializedGoods = null;
            try
            {
                using (var reader = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Goods));
                    deserializedGoods = (Goods)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                deserializedGoods = new Goods();
            }

            return deserializedGoods;
        }

        public static string Serialize(Goods goods)
        {
            string serializedGoods = null;
            try
            {
                XmlSerializer writer = new XmlSerializer(typeof(Goods));
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    writer.Serialize(sw, goods);
                    serializedGoods = sb.ToString();
                }
            }
            catch (Exception)
            {
                serializedGoods = String.Empty;
            }

            return serializedGoods;
        }
    }
}