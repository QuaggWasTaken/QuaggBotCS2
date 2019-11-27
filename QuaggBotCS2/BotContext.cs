using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace QuaggBotCS2
{
    [Serializable]
    public class BotContext
    {

        public List<Server> Servers { get; set; }


        private void SaveToDisk()
        {
            File.Delete("botContext.bin");
            Stream saveFileStream = File.Create("botContext.bin");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, this);
            saveFileStream.Close();
            Thread.Sleep(TimeSpan.FromMinutes(1));
        }

        public void LoopNewThread()
        {
            while (true)
            {
                Thread thread = new Thread(new ThreadStart(SaveToDisk));
                thread.Start();
            }
        }
    }
}
