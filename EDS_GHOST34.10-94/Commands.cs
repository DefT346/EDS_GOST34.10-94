using System.Numerics;
using System.Reflection;

namespace EDS_GHOST34._10_94
{

    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            return new List<T>(array)
                        .GetRange(offset, length)
                        .ToArray();
        }

        public static T[] SubArray<T>(this T[] array, int offset)
        {
            return new List<T>(array)
                        .GetRange(offset, array.Length - offset)
                        .ToArray();
        }
    }

    internal class Commands
    {
        internal static void Print(string mess, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mess);
            Console.ResetColor();
        }

        internal static void Parse(string input)
        {
            try
            {
                var data = input.Split(' ');
                var key = data[0].ToLower();
                var method = typeof(Commands).GetMethod(key, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                var body = data.SubArray(1);
                if (method == null) throw new Exception("Команда не найдена");
                method.Invoke(null, new object[] { body });
            }
            catch (Exception ex)
            {
                Print("\n" + ex.Message, ConsoleColor.Red);
            }
        }

        public static void Sign(params string[] args)
        {
            // Создание подписи
            Print("\nСоздание подписи", ConsoleColor.Yellow);

            var document = FileDialog.ReadFile(FileDialog.ShowDialog());

            var privkey = Program.signature.CreatePrivateKey(); Console.WriteLine($"\nСоздан приватный ключ: {privkey}");
            var pubkey = Program.signature.GetPublicKey(privkey); Console.WriteLine($"\nПубличный ключ: {pubkey}");

            var signdata = Program.signature.Sign(privkey, document);
            Print($"\nСоздана подпись:", ConsoleColor.Green);
            Console.WriteLine($"r: {signdata.r}\ns: {signdata.s}");
        }

        public static void Verify(params string[] args)
        {
            Print("\nПроверка подписи", ConsoleColor.Yellow);

            Console.Write("\nВведите публичный ключ: ");
            var pubkey = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine();
            SignData signdata = SignData.Read();

            // Проверка подписи
            var cdocument = FileDialog.ReadFile(FileDialog.ShowDialog());

            var v = EDSignature.Verify(Program.signature, signdata, pubkey, cdocument);

            if (v) Print("\nПодпись подтверждена", ConsoleColor.Green);
            else Print("\nПодпись не подтверждена", ConsoleColor.Red);
        }
    }
}
