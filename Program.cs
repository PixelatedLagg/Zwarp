using LightCSV;

namespace Zwarp
{
    public class Program
    {
        public static async Task Main()
        {
            string MenuOptions = System.IO.File.ReadAllText("options.txt");
            AppDomain.CurrentDomain.ProcessExit += File.OnExit;
            if (!System.IO.File.Exists("00inilock"))
            {
                Input("Welcome to Zwarp!\nSet Master Password: ");
                string pSet = Console.ReadLine() ?? "";
                while (pSet == "")
                {
                    await Error("Invalid Master Password!");
                    Input("Welcome to Zwarp!\nSet Master Password: ");
                    pSet = Console.ReadLine() ?? "";
                }
                Data.Password = pSet;
                await Success("Set Master Password.");
            }
            else
            {
                File.Init();
            }
            Input("Zwarp V0.1\nPassword: ");
            string password = Console.ReadLine() ?? "";
            while (password != Data.Password)
            {
                await Error("Incorrect Master Password!");
                Input("Zwarp V0.1\nMaster Password: ");
                password = Console.ReadLine() ?? "";
            }
            Console.Clear();
            while (true)
            {
                Input($"{MenuOptions}\n\nSelect Option: ");
                switch (Console.ReadKey().KeyChar - '0')
                {
                    case 1:
                        Console.Clear();
                        if (Data.Services.Count == 0)
                        {
                            await Error("No Services To View!");
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Blue;
                        for (int i = 1; i - 1 < Data.Services.Count; i++)
                        {
                            Console.WriteLine($"{i} : {Data.Services.ElementAt(i - 1).Key}");
                        }
                        Console.Write("\nSelect Option: ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        int option = Console.ReadKey().KeyChar - '0';
                        while (option > Data.Services.Count || option == 0)
                        {
                            await Error("Invalid Option!");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            for (int i = 1; i - 1 < Data.Services.Count; i++)
                            {
                                Console.WriteLine($"{i} : {Data.Services.ElementAt(i - 1).Key}");
                            }
                            Console.Write("\nSelect Option: ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            option = Console.ReadKey().KeyChar - '0';
                        }
                        Console.Clear();
                        TextCopy.ClipboardService.SetText(Data.Services.ElementAt(option - 1).Value);
                        await Success("Copied Service Password To Clipboard.");
                        break;
                    case 2:
                        Console.Clear();
                        Input("Service Name: ");
                        string sName = Console.ReadLine() ?? "";
                        while (sName == "")
                        {
                            await Error("Invalid Service Name!");
                            Input("Service Name: ");
                            sName = Console.ReadLine() ?? "";
                        }
                        while (Data.Services.ContainsKey(sName))
                        {
                            await Error("Service Name In Use!");
                            Input("Service Name: ");
                            sName = Console.ReadLine() ?? "";
                        }
                        Input("Service Password: ");
                        string sPassword = Console.ReadLine() ?? "";
                        while (sPassword == "")
                        {
                            await Error("Invalid Service Password!");
                            Input("Service Password: ");
                            sPassword = Console.ReadLine() ?? "";
                        }
                        Data.Services.Add(sName, sPassword);
                        await Success("Added Service.");
                        break;
                    case 3:
                        Console.Clear();
                        Input("Service Name: ");
                        string srName = Console.ReadLine() ?? "";
                        while (srName == "")
                        {
                            await Error("Invalid Service Name!");
                            Input("Service Name: ");
                            srName = Console.ReadLine() ?? "";
                        }
                        while (!Data.Services.ContainsKey(srName))
                        {
                            await Error("Service Name Not In Use!");
                            Input("Service Name: ");
                            srName = Console.ReadLine() ?? "";
                        }
                        Data.Services.Remove(srName);
                        await Success("Removed Service.");
                        break;
                    case 4:
                        Console.Clear();
                        Input("Are You Sure? (Y/N) ");
                        char cOption = Console.ReadKey().KeyChar;
                        if (cOption == 'y' || cOption == 'Y')
                        {
                            Data.Services.Clear();
                            await Success("Cleared All Services.");
                        }
                        else
                        {
                            Console.Clear();
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Input("New Master Password: ");
                        string mPassword = Console.ReadLine() ?? "";
                        while (mPassword == "")
                        {
                            await Error("Invalid Master Password!");
                            Input("New Master Password: ");
                            mPassword = Console.ReadLine() ?? "";
                        }
                        Data.Password = mPassword;
                        await Success("Changed Master Password.");
                        break;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Zwarp is a password manager created by Pixelated_Lagg.\n\nGithub: https://github.com/PixelatedLagg\nDiscord: Pixelated_Lagg#8321");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 7:
                        Console.Clear();
                        Input("Enter the path of your CSV file: ");
                        string csvPath = Console.ReadLine() ?? "";
                        Input("Enter the column name for the names of each service: ");
                        string csvName = Console.ReadLine() ?? "";
                        Input("enter the column name for the passwords of each service: ");
                        string csvPassword = Console.ReadLine() ?? "";
                        CSVObject csv = CSVObject.ParseFromFile(csvPath);
                        //read entries
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
        private static async Task Success(string text)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
            await Task.Delay(1500);
            Console.Clear();
        }
        private static async Task Error(string text)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
            await Task.Delay(1500);
            Console.Clear();
        }
        private static void Input(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}