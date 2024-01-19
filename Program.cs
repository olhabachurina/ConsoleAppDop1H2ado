// See https://aka.ms/new-console-template for more information
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

class Program
{
    static void Main()
    {
        string connectionString = GetConnectionString();
        DatabaseManager dbManager = new DatabaseManager(connectionString);

        // Добавление книги
        // dbManager.AddBook("The Great Gatsby", 1, 1925, 10);

        // Добавление автора
        //dbManager.AddAuthor("F. Scott", "Fitzgerald");

        // Регистрация читателя
        //dbManager.RegisterReader("Anna", "Petrenko", "petrenko@ukr.net");

        // Выдача книги читателю
        //dbManager.LendBook(1, 1, DateTime.Now);

        // Просмотр списка книг
        // DataTable booksTable = dbManager.GetBooks();
        // Console.WriteLine("Books:");
        //PrintDataTable(booksTable);

        // Просмотр списка авторов
        // DataTable authorsTable = dbManager.GetAuthors();
        // Console.WriteLine("Authors:");
        // PrintDataTable(authorsTable);

        // Просмотр списка читателей
        // DataTable readersTable = dbManager.GetReaders();
        // Console.WriteLine("Readers:");
        // PrintDataTable(readersTable);

        // Возврат книги
        //dbManager.ReturnBook(1, DateTime.Now.AddDays(14));

        // Просмотр обновленного списка книг
       // DataTable booksTable = dbManager.GetBooks();
       //Console.WriteLine("Books:");
      //  PrintDataTable(booksTable);
    }
    // Вспомогательный метод для вывода таблицы в консоль
    static void PrintDataTable(DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write($"{col.ColumnName}: {row[col]} \t");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        IConfiguration configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"ConnectionString: {connectionString}");

        return connectionString;
    }
}

class DatabaseManager
{
    private readonly string connectionString;
    private object authorName;

    public DatabaseManager(string connectionString)
    {
        this.connectionString = connectionString;
    }


    // Общий метод для выполнения SQL-запросов
    private void ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        command.Parameters.AddRange(parameters);
        command.ExecuteNonQuery();
    }
    // Метод для добавления книги
    public void AddBook(string title, int authorId, int publicationYear, int copiesAvailable)
    {
        string query = "INSERT INTO Books (Title, AuthorId, PublicationYear, CopiesAvailable) " +
                       "VALUES (@Title, @AuthorId, @PublicationYear, @CopiesAvailable)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@Title", title),
            new SqlParameter("@AuthorId", authorId),
            new SqlParameter("@PublicationYear", publicationYear),
            new SqlParameter("@CopiesAvailable", copiesAvailable)
        };

        ExecuteNonQuery(query, parameters);
    }

    // Метод для добавления автора
    public void AddAuthor(string firstName, string lastName)
    {
        string query = "INSERT INTO Authors (FirstName, LastName) " +
                       "VALUES (@FirstName, @LastName)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@FirstName", firstName),
            new SqlParameter("@LastName", lastName)
        };

        ExecuteNonQuery(query, parameters);
    }

    public void RegisterReader(string firstName, string lastName, string contactInfo)
    {
        string query = "INSERT INTO Readers (FirstName, LastName, ContactInfo) " +
                       "VALUES (@FirstName, @LastName, @ContactInfo)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@FirstName", firstName),
            new SqlParameter("@LastName", lastName),
            new SqlParameter("@ContactInfo", contactInfo)
        };

        ExecuteNonQuery(query, parameters);
    }

    public void LendBook(int readerId, int bookId, DateTime loanDate)
    {
        string query = "INSERT INTO BookLoans (ReaderId, BookId, LoanDate) " +
                       "VALUES (@ReaderId, @BookId, @LoanDate)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ReaderId", readerId),
            new SqlParameter("@BookId", bookId),
            new SqlParameter("@LoanDate", loanDate)
        };

        ExecuteNonQuery(query, parameters);
    }

    public void ReturnBook(int loanId, DateTime returnDate)
    {
        string query = "UPDATE BookLoans SET ReturnDate = @ReturnDate WHERE LoanId = @LoanId";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ReturnDate", returnDate),
            new SqlParameter("@LoanId", loanId)
        };

        ExecuteNonQuery(query, parameters);
    }

    public DataTable GetBooks()
    {
        string query = "SELECT * FROM Books";
        return ExecuteQuery(query);
    }

    public DataTable GetAuthors()
    {
        string query = "SELECT * FROM Authors";
        return ExecuteQuery(query);
    }

    public DataTable GetReaders()
    {
        string query = "SELECT * FROM Readers";
        return ExecuteQuery(query);
    }

    private DataTable ExecuteQuery(string query)
    {
        using (var connection = new SqlConnection(connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            var adapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }

static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        IConfiguration configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"ConnectionString: {connectionString}");

        return connectionString;
    }
}
