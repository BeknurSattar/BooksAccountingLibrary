using System;
using System.Collections.Generic;
using System.IO;

namespace BooksAccountingLibrary
{
    // Определение класса Book
    class Book
    {
        // Свойства, представляющие детали книги
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        // Конструктор для инициализации объекта Book
        public Book(string title, string author, int year, string genre)
        {
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
        }

        // Переопределение метода ToString для предоставления строкового представления книги
        public override string ToString()
        {
            return $"Название: {Title}, Автор: {Author}, Год выпуска: {Year}, Жанр: {Genre}";
        }
    }

    // Определение класса программы
    class Program
    {
        // Список для хранения объектов Book, представляющих библиотеку
        static List<Book> library = new List<Book>();

        // Имя файла для сохранения и загрузки данных библиотеки
        static string fileName = "library.txt";

        // Метод для добавления книги в библиотеку
        static void AddBook()
        {
            do
            {
                // Ввод пользователем деталей книги
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
                        // Получение текущего года
                        int currentYear = DateTime.Now.Year;

                        if (year <= currentYear)
                        {
                            break; // Выход из цикла при успешном вводе года
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

                // Создание объекта Book и добавление его в библиотеку
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
                // Удаление книги по указанному индексу
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
                // Отображение каждой книги в библиотеке с ее индексом
                for (int i = 0; i < library.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {library[i]}");
                }
            }
        }

        // Метод для сохранения библиотеки в файл
        static void SaveLibrary()
        {
            // Использование StreamWriter для записи деталей книг в файл
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
                // Чтение всех строк из файла
                string[] lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    // Разделение каждой строки на части с использованием запятой
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        // Разбор и создание объекта Book из частей
                        string title = parts[0];
                        string author = parts[1];
                        if (int.TryParse(parts[2], out int year))
                        {
                            string genre = parts[3];
                            Book book = new Book(title, author, year, genre);
                            // Добавление книги в библиотеку
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
            // Загрузка данных библиотеки из файла
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
                        // Перед выходом спрашиваем пользователя, хочет ли он сохранить изменения
                        Console.WriteLine("Хотите сохранить изменения перед выходом? (да/нет)");
                        string saveChoice = Console.ReadLine().Trim();
                        if (saveChoice.Equals("да", StringComparison.OrdinalIgnoreCase))
                        {
                            SaveLibrary();
                        }
                        // Выход из программы
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