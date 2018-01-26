using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace ConnectionHandler
{
    public class DatabaseHandler<T>
    {
        const string DatabaseName = "PaymentSwitchDB";
        static MongoClient client;
        static IMongoDatabase db;

        /// <summary>
        /// Generic Select statement for all objects
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static List<T> getDocumentContent(T Obj)
        {
            client = new MongoClient();
            db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(Obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue ="tbl"+ collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            IMongoCollection<T> col = db.GetCollection<T>(ClassValue);//
            List<T> returnList = (List<T>)col.Find(f => true).ToListAsync().Result;
            return returnList;
        }


        /// <summary>
        /// Generic select that applies a filter
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// Expression<Func<T, object>> property, Func<T, object> func
        public static List<T> getDocumentContent(T obj, Dictionary<Expression<Func<T, object>>, Func<T, object>> Filters)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue = "tbl" + collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            var col = db.GetCollection<T>(ClassValue);
            var builder = Builders<T>.Filter;
            FilterDefinition<T> query=null;
            int counter = 0;
            foreach (KeyValuePair<Expression<Func<T, object>>, Func<T, object>> item in Filters)
            {
                var propertyValue = item.Value(obj);

                var field = (item.Key.Body as MemberExpression).Member.Name;
                query = (counter == 0) ? builder.Eq(field, propertyValue):query & builder.Eq(field, propertyValue);
                counter++;
            }
           
            List<T> returnList = (List<T>)col.Find(query).ToListAsync().Result;
            return returnList;
        }

        public static void insertData(T obj)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue = "tbl"+collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            var col = db.GetCollection<T>(ClassValue);
            col.InsertOne(obj);
        }


       

    }
}
