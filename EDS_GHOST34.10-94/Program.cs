using EDS_GHOST34._10_94;

class Program
{
    public static EDSignature signature;

    static async Task Main(string[] args)
    {
        Commands.Print("ЭЦП ГОСТ34.10-94", ConsoleColor.Yellow);
        Commands.Print("sign - создать подпись", ConsoleColor.DarkGray);
        Commands.Print("verify - проверить подпись\n", ConsoleColor.DarkGray);


        signature = new EDSignature();
        while (true)
        {
            Console.Write("\n> ");
            // Генерация параметров сигнатуры
            var cmd = Console.ReadLine();
            Commands.Parse(cmd);
        }
    }
}