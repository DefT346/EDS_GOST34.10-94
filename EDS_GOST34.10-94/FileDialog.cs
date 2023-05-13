

using System.Runtime.InteropServices;
using System.Text;


// From https://www.pinvoke.net/default.aspx/Structures/OPENFILENAME.html
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct OpenFileName
{
    public int lStructSize;
    public IntPtr hwndOwner;
    public IntPtr hInstance;
    public string lpstrFilter;
    public string lpstrCustomFilter;
    public int nMaxCustFilter;
    public int nFilterIndex;
    public string lpstrFile;
    public int nMaxFile;
    public string lpstrFileTitle;
    public int nMaxFileTitle;
    public string lpstrInitialDir;
    public string lpstrTitle;
    public int Flags;
    public short nFileOffset;
    public short nFileExtension;
    public string lpstrDefExt;
    public IntPtr lCustData;
    public IntPtr lpfnHook;
    public string lpTemplateName;
    public IntPtr pvReserved;
    public int dwReserved;
    public int flagsEx;
}

internal class FileDialog
{
    // From https://www.pinvoke.net/default.aspx/comdlg32/GetOpenFileName.html
    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName(ref OpenFileName ofn);

    public static string ShowDialog(string filter = "")
    {
        Console.WriteLine("\nВыберите документ:");

        var ofn = new OpenFileName();
        ofn.lStructSize = Marshal.SizeOf(ofn);
        // Define Filter for your extensions (Excel, ...)
        //ofn.lpstrFilter = "Excel Files (*.xlsx)\0*.xlsx\0All Files (*.*)\0*.*\0";
        ofn.lpstrFilter = filter;
        ofn.lpstrFile = new string(new char[256]);
        ofn.nMaxFile = ofn.lpstrFile.Length;
        ofn.lpstrFileTitle = new string(new char[64]);
        ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
        ofn.lpstrTitle = "Open File Dialog...";
        if (GetOpenFileName(ref ofn))
            return ofn.lpstrFile;
        return string.Empty;
    }

    public static byte[] ReadFile(string path)
    {
        // чтение из файла
        using (FileStream fstream = File.OpenRead(path))
        {
            var p = fstream.Name.Split('\\');
            Console.WriteLine($"\nОткрыт документ: {p.Last()}");
            // выделяем массив для считывания данных из файла
            byte[] buffer = new byte[fstream.Length];
            // считываем данные
            fstream.Read(buffer, 0, buffer.Length);
            // декодируем байты в строку
            return buffer;
        }
    }

    public static void WriteFile(string path, byte[] buffer)
    {
        // запись в файл
        using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
        {
            // запись массива байтов в файл
            fstream.WriteAsync(buffer, 0, buffer.Length);
            Console.WriteLine("Текст записан в файл");
        }
    }
}

