using System;
using System.Text;

namespace Bigdata.OrcTesseract
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineHandleManagement cmdHandler = new CommandLineHandleManagement();

            cmdHandler.Register("ocr", (param) =>
            {
                new TesseractEngineWrapper().Handle(param);
            }, @"ocr full file path or folder eg: ocr eng c:/orc.jpg eg: ocr vie c:/folderimg");

            while (true)
            {
                var cmd = Console.ReadLine();
                cmdHandler.Do(cmd);
            }
        }
    }
}
