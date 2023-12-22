using System;
using BooksAccountingLibrary.BookLib;
using Serilog;

namespace BooksAccountingLibrary
{
    class Program
    {
        private const string ShowLibraryAction = "1";
        private const string AddBookAction = "2";
        private const string RemoveBookAction = "3";
        private const string SaveLibraryAction = "4";
        private const string ExitAction = "5";

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt")
                .CreateLogger();

            try
            {
                LibraryManager.LoadLibrary();

                while (true)
                {
                    LibraryManager.DisplayMenu();
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case ShowLibraryAction:
                            LibraryManager.ShowLibrary();
                            break;
                        case AddBookAction:
                            LibraryManager.AddBook();
                            break;
                        case RemoveBookAction:
                            LibraryManager.RemoveBook();
                            break;
                        case SaveLibraryAction:
                            LibraryManager.SaveLibrary();
                            Console.WriteLine("Нажмите Enter чтобы продолжить.");
                            Console.ReadLine();
                            break;
                        case ExitAction:
                            Console.WriteLine("Хотите сохранить изменения перед выходом? (да/нет)");
                            string saveChoice = Console.ReadLine().Trim();
                            if (saveChoice.Equals("да", StringComparison.OrdinalIgnoreCase))
                            {
                                LibraryManager.SaveLibrary();
                            }
                            Console.WriteLine("Нажмите Enter для выхода.");
                            Console.ReadLine();
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                            break;
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка: {Message}", ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
