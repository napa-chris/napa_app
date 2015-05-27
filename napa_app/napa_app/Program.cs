using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlchemyAPI;
using napa_app.sdk;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;

namespace napa_app
{
    class Program
    {
        public async static Task SaveResults(string json)
        {
            var client = new MongoClient("mongodb://napa_user:Password0@ds031982.mongolab.com:31982/heroku_app37054022");
            var db = client.GetDatabase("heroku_app37054022");
            var collection = db.GetCollection<BsonDocument>("results");
            BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(json);
            var status = document.GetElement("status");
            if (status.Value != "ERROR")
            {
                var results = document.GetElement("result").Value.AsBsonDocument.GetElement("docs").Value.AsBsonArray.Select(a => a.AsBsonDocument);
                await collection.InsertManyAsync(results);
                Console.WriteLine("Saved");
            }
        }

        public async static Task GetDocs()
        {
            var client = new MongoClient("mongodb://napa_user:Password0@ds031982.mongolab.com:31982/heroku_app37054022");
            var db = client.GetDatabase("heroku_app37054022");
            var collection = db.GetCollection<BsonDocument>("results");
            var filter = Builders<BsonDocument>.Filter;
            filter.ElemMatch("source.enriched.url.taxonomy", Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Gt("score", 0.5), Builders<BsonDocument>.Filter.Regex("label", "*.science.*")));
            using (var cursor = await collection.Find(filter.ToBsonDocument()).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Console.WriteLine(doc);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            AlchemyAPI.AlchemyAPI alchemyObj = new AlchemyAPI.AlchemyAPI();
            alchemyObj.SetAPIKey("6b5aa8499e9a98215e57cf2ef6a0456654be777b");
            //alchemyObj.SetAPIKey("4c32fab89d526ce5e132a82213f68daf67c5d266");

            //AlchemyAPI_NewsParams prms = new AlchemyAPI_NewsParams();
            //prms.setOutputMode(AlchemyAPI_BaseParams.OutputMode.JSON);
            //prms.StartDate = "now-1d";
            //prms.EndDate = "now";
            //prms.Count = 10;
            //prms.setEntities(new Entity(EntityType.Person, "Elon Musk"));
            //prms.setReturn(ReturnOutputData.Url, ReturnOutputData.Title, ReturnOutputData.Taxonomy);
            //string json = alchemyObj.GetNews(prms);

            //var task = SaveResults(json);
            //task.Wait();

            var task = GetDocs();
            task.Wait();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
