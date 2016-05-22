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
            Random rnd = new Random();
            List<string> fNamesList = System.IO.File.ReadAllLines("names.txt").ToList();
            List<string> lNamesList = System.IO.File.ReadAllLines("surnames.txt").ToList();
            List<string> companies = new List<string>() {"Comarch","Google","Ericpol","BMW","JP","Kamis","Tesco","Fiat","Wedel","Coca Cola", "Pepsico", "Sabre", "Sabatier", "Ikea", "Adidas", "Nike", "Rebook", "Zelmer", "Amino", "Amica", "Sony", "Microsoft", "KrzychuSoftware"};
            

            for (int i = 0; i < 10; i++ )
            {
                string firstName = fNamesList[rnd.Next(fNamesList.Count)];
                string lastName = lNamesList[rnd.Next(lNamesList.Count)];
                User usr = new User(firstName,lastName, "user_" + lastName + "_" + i.ToString().PadLeft(2,'0'), "password", firstName + "." + lastName + "@.invoiceSystem.pl", UserStatus.User, rnd.NextDouble() < 0.3 ? true : false, false);
                UserDAL.UserAdd(usr);
            }

            for (int i = 0; i < 7000; i++ )
            {
                Partner part = new Partner(fNamesList[rnd.Next(fNamesList.Count)], lNamesList[rnd.Next(lNamesList.Count)], companies[rnd.Next(companies.Count)], 10000000 + i, "Address " + i.ToString());
                PartnerDAL.PartnerAdd(part);
            }

            for (int i = 0; i < 4000; i++)
            {
                string number = InvoiceDAL.GetInvoiceNumber();
                List<Product> prods = new List<Product>();

                for (int j = 0; j < rnd.Next(20)+1; j++)
                {
                    prods.Add(new Product("Produkt" + j.ToString(), rnd.Next(100), (float)rnd.NextDouble() * 1000, ((float)rnd.Next(23)) / 100));
                }
                Invoice inv = new Invoice(number, DateTime.Now, "Faktura za towary " + i.ToString(), prods, ((float)rnd.Next(20)) / 100);
                InvoiceDAL.InvoiceAdd(inv, 10000000 + rnd.Next(7000), 10000000 + rnd.Next(7000));
            }
        }
    }
}
