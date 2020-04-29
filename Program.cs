using System;
using System.IO;
using System.Threading;

namespace QUETE_Event
{
    class Program
    {
        public event EventHandler<SendLogEventArgs> OnLogSend;
        static void Main(string[] args)
        {
            var program = new Program();
            var logger = new StandardOutputLogger();
            var loggerFile = new FileStreamOutputLogger();

            logger.Subscribe(program);
            loggerFile.Subscribe(program);
            var eventArgs1 = new SendLogEventArgs("I am the LogEvent published 1 ", DateTime.Now);
            Thread.Sleep(2000);
            var eventArgs2 = new SendLogEventArgs("I am the LogEvent published 2 ", DateTime.Now);

            if (program.OnLogSend != null)
            {
                program.OnLogSend(program, eventArgs1);
                program.OnLogSend(program, eventArgs2);
            }
        }
    }

    class StandardOutputLogger
    {
        public void Subscribe(Program program)
        {
            program.OnLogSend += HandleLogSend;
        }

        public void HandleLogSend(object sender, SendLogEventArgs args)
        {
            Write(args.Message, args.DateTime);
        }

        public void Write(String message, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            String formattedMessage = String.Format("{0} - {1}", dateTime, message);
            Console.WriteLine(formattedMessage);
        }
    }

    class FileStreamOutputLogger
    {
        public void Subscribe(Program program)
        {
            program.OnLogSend += HandleLogSend;
        }

        public void HandleLogSend(object sender, SendLogEventArgs args)
        {
            IsExistingFile(args.Message, args.DateTime);
        }

        public void IsExistingFile(String message, DateTime? dateTime = null)
        {
            if(!File.Exists("file1.txt"))
            {
                CreateFile("file1.txt");
                WriteFile("file1", message, dateTime);
            }
            else if (!File.Exists("file2.txt"))
            {
                CreateFile("file2.txt");
                WriteFile("file2", message, dateTime);
            }
        }
        public void WriteFile(string filePath, String message, DateTime? dateTime = null)
        {
            TextWriter writeFile = new StreamWriter(filePath + ".txt");
            writeFile.Write(message + dateTime);
            writeFile.Flush();
            writeFile.Close();
        }
        public static void CreateFile(string nameFile)
        {        
            FilePath newFile = new FilePath();
            newFile.Name = nameFile;
            FileStream currentFIle = File.Create(nameFile);
            currentFIle.Close();
        }
    }

    class SendLogEventArgs : EventArgs
    {
        public String Message;
        public DateTime DateTime;

        public SendLogEventArgs(String message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }
    }

    public class FilePath
    {
        public string Name { get; set; }

    }
}
