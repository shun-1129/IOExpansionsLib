using TestConsoleApp.Models;
using static IOExpansionsLib.IO.JsonUtility;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main ( string[] args )
        {
            Appsettings? appsettings = ReadJsonFile<Appsettings> ( "appsettings.json" , "Appsettings" );

            int hoge = 0;
        }
    }
}
