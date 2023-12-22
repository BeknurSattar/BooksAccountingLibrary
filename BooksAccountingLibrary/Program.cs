using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using BooksAccountingLibrary.Book;
using Serilog;
using Serilog.Debugging;


namespace BooksAccountingLibrary
{
    class Program
    {
        // Главный метод, точка входа в программу
        static void Main(string[] args)
        {

            try

            {
                LibraryManager.LoadLibrary();

                while (true)
                {
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File("log.txt")
                        .CreateLogger();

                    Log.Information("Informational message");
                    Log.Warning("Warning message");
                    Log.Error("Error message");
                    Log.Fatal("Critical message");

                    Log.CloseAndFlush();
                    // Отображение основного меню и получение ввода пользователя
                    LibraryManager.DisplayMenu();
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            LibraryManager.ShowLibrary();
                            break;
                        case "2":
                            LibraryManager.AddBook();
                            break;
                        case "3":
                            LibraryManager.RemoveBook();
                            break;
                        case "4":
                            LibraryManager.SaveLibrary();
                            Console.WriteLine("Нажмите Enter чтобы продолжить.");
                            break;
                        case "5":
                            Console.WriteLine("Хотите сохранить изменения перед выходом? (да/нет)");
                            string saveChoice = Console.ReadLine().Trim();
                            if (saveChoice.Equals("да", StringComparison.OrdinalIgnoreCase))
                            {
                                LibraryManager.SaveLibrary();
                            }
                            Console.WriteLine("Нажмите Enter для выхода.");
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
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
