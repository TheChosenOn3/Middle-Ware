using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConnectionHandler
{
    class Logger
    {
        public static MongoClient StartLogger()
        {
            MongoClient client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost"),
                ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        using (StreamWriter file = new StreamWriter(@"C:\Logs\MongoLog.txt", true))
                        {
                            if (!(e.CommandName == "buildInfo" || e.CommandName == "isMaster"))
                            {
                                file.WriteLine(DateTime.Now + "\t" + $"{e.CommandName} - {e.Command.ToJson()}");
                                file.Close();
                            }
                        }
                    });
                }
            });
            return client;
        }
    }
}
