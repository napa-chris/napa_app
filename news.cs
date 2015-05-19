using System;
using AlchemyAPI;

namespace News {
    class Program {
        static public void Main(String[] args) {
            AlchemyAPI.AlchemyAPI alchemyObj = new AlchemyAPI.AlchemyAPI();
            alchemyObj.SetAPIKey("6b5aa8499e9a98215e57cf2ef6a0456654be777b");
            //alchemyObj.SetAPIKey("4c32fab89d526ce5e132a82213f68daf67c5d266");

            AlchemyAPI_NewsParams prms = new AlchemyAPI_NewsParams();
            prms.setStartDate(30, 'd');
            prms.setEndDate();
            //string[] taxonomy = { "books" };
            string[] entityText = { "elon+musk" };
            //prms.addTaxonomy(taxonomy);
            prms.addEntityText(entityText);
            prms.setCount(10);
            string xml = alchemyObj.GetNews(prms);
            
            Console.WriteLine(xml);
            Console.ReadLine();
        }
    }
}