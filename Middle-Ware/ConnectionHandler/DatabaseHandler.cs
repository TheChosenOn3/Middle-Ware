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
        const string DatabaseName = "Project500";
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
            string ClassValue = collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
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
        public static List<T> getFilteredObj(T obj, Expression<Func<T, object>> property, Func<T, object> func)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue = collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            var col = db.GetCollection<T>(ClassValue);
            var propertyValue = func(obj);
            var builder = Builders<T>.Filter;
            var field = (property.Body as MemberExpression).Member.Name;
            var query = builder.Eq(field, propertyValue);
            List<T> returnList = (List<T>)col.Find(query).ToListAsync().Result;
            return returnList;
        }

    }
}
