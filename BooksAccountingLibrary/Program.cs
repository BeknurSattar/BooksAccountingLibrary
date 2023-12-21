using System;
using System.Collections.Generic;
using System.IO;

namespace BooksAccountingLibrary
{
    // Определение класса Book
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        public Book(string title, string author, int year, string genre)
        {
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
        }

        public override string ToString()
        {
            return $"Название: {Title}, Автор: {Author}, Год выпуска: {Year}, Жанр: {Genre}";
        }
    }

    // Определение класса программы
    class Program
    {
        static List<Book> library = new List<Book>();

        static string fileName = "library.txt";

        // Метод для добавления книги в библиотеку
        static void AddBook()
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
        static string ReadNonEmptyString()
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
        static bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        // Метод для удаления книги из библиотеки
        static void RemoveBook()
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
        static void ShowLibrary()
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

        // Метод для сохранения библиотеки в файл
        static void SaveLibrary()
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

        // Метод для загрузки библиотеки из файла
        static void LoadLibrary()
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

        // Метод для отображения основного меню
        static void DisplayMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Показать библиотеку");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Сохранить библиотеку");
            Console.WriteLine("5. Выйти");
        }

        // Главный метод, точка входа в программу
        static void Main(string[] args)
        {
            LoadLibrary();

            while (true)
            {

                // Отображение основного меню и получение ввода пользователя
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowLibrary();
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        RemoveBook();
                        break;
                    case "4":
                        SaveLibrary();
                        Console.WriteLine("Нажмите Enter чтобы продолжить.");
                        Console.ReadLine();
                        break;
                    case "5":
                        Console.WriteLine("Хотите сохранить изменения перед выходом? (да/нет)");
                        string saveChoice = Console.ReadLine().Trim();
                        if (saveChoice.Equals("да", StringComparison.OrdinalIgnoreCase))
                        {
                            SaveLibrary();
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
    }
}