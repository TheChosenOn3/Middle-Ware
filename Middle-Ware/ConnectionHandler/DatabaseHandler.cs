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

        public static void UpdateDocument(T obj, List<DBFilterClass<T>>filterList)
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
            int counter = 0;
            FilterDefinition<T> query1=null;
            foreach (DBFilterClass<T> item in filterList)
            {
                var FieldName = (item.Field.Body as MemberExpression).Member.Name;
                var PrimeKeyValue = item.FieldValues(obj);
                if (counter == 0)
                {
                    query1 = (item.condition == FilterCondition.equals) ? Primarybuilder.Eq(FieldName, PrimeKeyValue) :  Primarybuilder.Ne(FieldName, PrimeKeyValue);//;
                    counter++;
                }
                else {
                    query1 =  (item.condition == FilterCondition.equals) ? query1 & Primarybuilder.Eq(FieldName, PrimeKeyValue) : query1 & Primarybuilder.Ne(FieldName, PrimeKeyValue);//;
                }
            }
           
           

            //////
            UpdateDefinition<T> query = null;
            var UpdateBuilder = Builders<T>.Update;
             counter = 1;

            foreach (KeyValuePair<string, string> item in fieldVal)
            {
                query = (counter == 1) ? UpdateBuilder.Set(item.Key, item.Value) : query.Set(item.Key, item.Value);
                counter++;
            }

            col.FindOneAndUpdate<T>(query1, query);
        }

        public static void UpdateDocument(T obj, DBFilterClass<T> filterList)
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
            int counter = 0;
            FilterDefinition<T> query1 = null;
           var FieldName = (filterList.Field.Body as MemberExpression).Member.Name;
           var PrimeKeyValue = filterList.FieldValues(obj);
                    counter++;
                query1 = (filterList.condition == FilterCondition.equals) ? Primarybuilder.Eq(FieldName, PrimeKeyValue) :  Primarybuilder.Ne(FieldName, PrimeKeyValue);//;
            



            //////
            UpdateDefinition<T> query = null;
            var UpdateBuilder = Builders<T>.Update;
            counter = 1;

            foreach (KeyValuePair<string, string> item in fieldVal)
            {
                query = (counter == 1) ? UpdateBuilder.Set(item.Key, item.Value) : query.Set(item.Key, item.Value);
                counter++;
            }

            col.FindOneAndUpdate<T>(query1, query);
        }

        public static void DeleteRow(T obj, Expression<Func<T, object>> PrimaryKey, Func<T, object> PrimaryKeyVal)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            string collectionName = Convert.ToString(obj.GetType());
            int indexCounter = collectionName.IndexOf('.');
            string ClassValue = "tbl" + collectionName.Substring(indexCounter + 1, collectionName.Length - indexCounter - 1);
            var col = db.GetCollection<T>(ClassValue);
            var propertyValue = PrimaryKeyVal(obj);
            var builder = Builders<T>.Filter;
            var field = (PrimaryKey.Body as MemberExpression).Member.Name;
            var query = builder.Eq(field, propertyValue);
            col.FindOneAndDelete(query);

        }




    }
}
