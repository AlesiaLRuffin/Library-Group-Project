﻿
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LibraryProject1
{
    public class FileService
    {
        //creates file
        public static void CreateFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        public static void WriteBookToCSV(List<Book> books, string fileName)
        {
            using StreamWriter file = new StreamWriter(fileName);
            file.WriteLine("Author, Title, Status, DueDate");
            foreach (var book in books)
            {
                file.WriteLine($"{book.Author}, {book.Title}, {book.Status}, {book.DueDate}");
            }
        }

        //splits up the values of the list objects so they can be used in other methods
        public static List<Book> ConvertCSVToArray(string filename)
        {
            // makes list of books
            List<Book> books = new List<Book>();
            //uses streamreader to read from file, using statment then closes streamreader
            using (StreamReader file = new StreamReader(filename))
            {
                //sets up line which would be an instance of an object of book and while there is an object on the line,it splits up the proporties 
                //and gives them an array value for each proporty in the object
                string line;
                var firstline = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {
                    string[] dataArray = line.Split(",");
                    books.Add(new Book()
                    {
                        BookID = dataArray[0],
                        Title = dataArray[1],
                        Author = dataArray[2],
                        Status = bool.Parse(dataArray[3]),
                        DueDate = DateTime.Parse(dataArray[4])
                    });
                }
                // returns objects with their proporties as an array
                return books;
            }

        }

        //made enum for search type and put it in arguments of method so we can use one method to search by
        //author,title, or status
        public static List<Book> SearchByType(string keyword, string fileName, SearchTypeEnum searchType)
        {
            //gets books from file and puts it into list of books
            List<Book> books = ConvertCSVToArray(fileName);
            var filteredBooks = books.FindAll(book =>
            {
                // setsup string to convert proporty into string when searching
                string proporty = searchType.ToString();
                // searches through the proporties of each book and looks for the keyword ("title", "author", "status")
                // based on the searchType enum
                if (book.GetType().GetProperty(proporty).GetValue(book, null).ToString() == keyword)
                {
                    return true;
                }
                return false;

            });
            //returns the list of matched books
            return filteredBooks;

        }


        public static void AddBook(Book bookToAdd, string fileName)
        {
            List<Book> books = ConvertCSVToArray(fileName);
            books.Add(bookToAdd);
            WriteBookToCSV(books, fileName);
        }

        //method that prints the book file when given file name
        public static void PrintBooksFile(string fileName)
        {
            var books = ConvertCSVToArray(fileName);
            PrintBooks(books);
        }

        //prints a list of books based on search preference if you make a fileservice.searchbytype as a variable and plug it 
        //in to the list argument
        public static void PrintBooks(List<Book> books)
        {
            books.ForEach(book =>
            {
                foreach (var proporty in typeof(Book).GetProperties())
                {
                    Console.Write(proporty.GetValue(book) + ", ");
                }
                Console.WriteLine();

            });
        }


        ////public static List<Book> SearchByType(string keyword, string fileName, SearchTypeEnum searchType)
        //{
        //    //gets books from file and puts it into list of books
        //    List<Book> books = ConvertCSVToArray(fileName);
        //    var filteredBooks = books.FindAll(book =>
        //    {
        //        // setsup string to convert proporty into string when searching
        //        string proporty = searchType.ToString();
        //        // searches through the proporties of each book and looks for the keyword ("title", "author", "status")
        //        // based on the searchType enum
        //        if (book.GetType().GetProperty(proporty).GetValue(book, null).ToString() == keyword)
        //        {
        //            return true;
        //        }
        //        return false;
        //    });
        //    //returns the list of matched books
        //    return filteredBooks;
        //}
        public static void CheckingOutBook(string userSelection, List<Book> bookList)
        {
            DateTime checkoutDay = DateTime.Now;
            DateTime bookDueDate = checkoutDay.AddDays(14);
            foreach (Book b in bookList)
            {
                if (b.Title.Equals(userSelection) || b.Author.Equals(userSelection))
                {
                    if (b.Status.Equals(false))
                    {
                        Console.WriteLine("This book is currently checked out.");
                    }
                    else
                    {
                        Console.WriteLine($"{userSelection} is being checked out on : {checkoutDay:MM/dd/yyyy}.");
                        Console.WriteLine($"{userSelection} , will be due back on : {bookDueDate:MM/dd/yyyy}.");
                        b.Status.Equals(false);
                    }
                }
            }
        }
        //this method checked to see if the b.status.equals(false) worked or not 
        public static void CheckTheStatus(string userSelection, List<Book> bookList)
        {
            foreach (Book b in bookList)
            {
                Console.WriteLine(b.Status.Equals(userSelection));
            }
        }
    }
}
