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
            }, @"full file path or folder eg: orc eng c:/orc.jpg eg: orc vie c:/folderimg");

            while (true)
            {
                var cmd = Console.ReadLine();
                cmdHandler.Do(cmd);
            }
        }
    }
}
