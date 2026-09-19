namespace TestConsoleApp
{
    internal class Program
    {
        static void Main ( string[] args )
        {
            Console.WriteLine ( "Hello, World!" );
            Console.WriteLine ();

            string env = Environment.GetEnvironmentVariable ( "SYSoft" ) ?? "未設定";
            Console.WriteLine ( $"SYSoft: {env}" );
        }
    }
}
