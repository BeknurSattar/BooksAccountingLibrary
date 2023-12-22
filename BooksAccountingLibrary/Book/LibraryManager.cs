using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace BooksAccountingLibrary.Book
{
    // Класс для работы с файлом
    public class LibraryManager
    {
        public const string FileNameKey = "FileName";
        public static string fileName = ConfigurationManager.AppSettings[FileNameKey];

        static List<Book> library = new List<Book>();

        private const string ShowLibraryAction = "1";
        private const string AddBookAction = "2";
        private const string RemoveBookAction = "3";
        private const string SaveLibraryAction = "4";
        private const string ExitAction = "5";

        // Метод для отображения основного меню
        public static void DisplayMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Показать библиотеку");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Сохранить библиотеку");
            Console.WriteLine("5. Выйти");
        }

        // Метод для добавления книги в библиотеку
        public static void AddBook()
        {
            do
            {
                Console.WriteLine("Введите название книги:");
                string title = ReadNonEmptyString();

                Console.WriteLine("Введите автора книги:");
                string author = ReadNonEmptyString();
                while (IsNumeric(author))
                {
                    Console.WriteLine("Автор не может состоять из чисел. Пожалуйста, введите корректное значение для автора:");
                    author = ReadNonEmptyString();
                }

                int year;
                do
                {
                    Console.WriteLine("Введите год выпуска:");
                    if (int.TryParse(ReadNonEmptyString(), out year))
                    {
                        int currentYear = DateTime.Now.Year;

                        if (year <= currentYear)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Год выпуска не может быть больше текущего года.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат года.");
                    }
                } while (true);

                Console.WriteLine("Введите жанр:");
                string genre = ReadNonEmptyString();
                while (IsNumeric(genre))
                {
                    Console.WriteLine("Жанр не может состоять из чисел. Пожалуйста, введите корректное значение для жанра:");
                    genre = ReadNonEmptyString();
                }

                Book book = new Book(title, author, year, genre);
                library.Add(book);
                Console.WriteLine("Книга успешно добавлена.");

                Console.WriteLine("Хотите добавить еще одну книгу? (да/нет)");
            } while (Console.ReadLine().Trim().Equals("да", StringComparison.OrdinalIgnoreCase));
        }

        // Метод ReadNonEmptyString используется для чтения строки с консоли, гарантируя, что введенная строка не является пустой или состоит только из пробелов.
        public static string ReadNonEmptyString()
        {
            string input;
            do
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Пожалуйста, введите непустую строку.");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        // Метод IsNumeric используется для проверки, является ли введенная строка числом.
        public static bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        // Метод для удаления книги из библиотеки
        public static void RemoveBook()
        {
            Console.WriteLine("Введите номер книги для удаления:");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= library.Count)
            {
                library.RemoveAt(index - 1);
                Console.WriteLine("Книга успешно удалена.");
            }
            else
            {
                Console.WriteLine("Неверный номер книги.");
            }
        }

        // Метод для отображения текущей библиотеки
        public static void ShowLibrary()
        {
            if (library.Count == 0)
            {
                Console.WriteLine("Библиотека пуста.");
            }
            else
            {
                for (int i = 0; i < library.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {library[i]}");
                }
            }
        }

        // Метод для загрузки из файла
        public static void LoadLibrary()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string[] lines = File.ReadAllLines(fileName);
                    foreach (var line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string title = parts[0];
                            string author = parts[1];
                            if (int.TryParse(parts[2], out int year))
                            {
                                string genre = parts[3];
                                Book book = new Book(title, author, year, genre);
                                library.Add(book);
                            }
                            else
                            {
                                Console.WriteLine($"Неверный формат года в строке: {line}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Неверный формат строки: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке библиотеки: {ex.Message}");
            }
        }

        // Метод для сохранение в файл
        public static void SaveLibrary()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var book in library)
                    {
                        writer.WriteLine($"{book.Title},{book.Author},{book.Year},{book.Genre}");
                    }
                }
                Console.WriteLine("Библиотека сохранена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении библиотеки: {ex.Message}");
            }
        }
    }
}
