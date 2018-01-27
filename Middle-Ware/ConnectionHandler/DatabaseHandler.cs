using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Linq;

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

        public static void UpdateDocument(T obj, Expression<Func<T, object>> PrimaryKey, Func<T, object> PrimaryKeyVal)
        {
            var fields = obj.GetType().GetProperties().Select(f => f.Name).ToList();
            Dictionary<string, string> fieldVal = new Dictionary<string, string>();

            foreach (string item in fields.Where(c => c.ToString() != "Id"))
            {
                var valueOfField = obj.GetType().GetProperty(item).GetValue(obj);
                if (valueOfField != null) fieldVal.Add(item, valueOfField.ToString());

            }
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue = "tbl" + collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            var col = db.GetCollection<T>(ClassValue);
            var Primarybuilder = Builders<T>.Filter;
            ///
            var FieldName = (PrimaryKey.Body as MemberExpression).Member.Name;
            var PrimeKeyValue = PrimaryKeyVal(obj);
            var query1 = Primarybuilder.Eq(FieldName, PrimeKeyValue);
            UpdateDefinition<T> query = null;
            var UpdateBuilder = Builders<T>.Update;
            int counter = 1;

            foreach (KeyValuePair<string, string> item in fieldVal)
            {
                query = (counter == 1) ? UpdateBuilder.Set(item.Key, item.Value) : query.Set(item.Key, item.Value);
                counter++;
            }

            col.FindOneAndUpdate<T>(query1, query);
        }






    }
}
