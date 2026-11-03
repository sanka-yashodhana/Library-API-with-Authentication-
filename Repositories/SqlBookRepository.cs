//using Microsoft.Data.SqlClient;
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LibraryApi.Models;

//namespace LibraryApi.Repositories
//{
//    public class SqlBookRepository : IBookRepository
//    {
//        private readonly string _connectionString;

//        public SqlBookRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("LibraryDb") ?? throw new InvalidOperationException("Library connection string was not configured. ");
//        }

//        //Add a Book
//        public void Add(Book book)
//        {
//            string sql = @" INSERT INTO Books (Id, Title, Author, IsAvailable)
//                        VALUES (@Id, @Title, @Author, @IsAvailable)";

//            using SqlConnection connection = new SqlConnection(_connectionString);
//            using SqlCommand command = new SqlCommand(sql, connection);

//            command.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;
//            command.Parameters.Add("@Title", SqlDbType.NChar, 150).Value = book.Title;
//            command.Parameters.Add("@Author", SqlDbType.NChar, 100).Value = book.Author;
//            command.Parameters.Add("@IsAvailable", SqlDbType.Bit, 150).Value = book.IsAvailable;

//            connection.Open();

//            command.ExecuteNonQuery();
//        }

//        //List all books
//        public List<Book> GetAll()
//        {
//            List<Book> books = new List<Book>();

//            string sql = @"SELECT Id, Title, Author, IsAvailable FROM BOOKS ORDER BY Id";

//            using SqlConnection connection = new SqlConnection(_connectionString);
//            using SqlCommand command = new SqlCommand(sql, connection);

//            connection.Open();

//            using SqlDataReader reader = command.ExecuteReader();

//            while (reader.Read())
//            {
//                Book book = new Book
//                {
//                    Id = reader.GetInt32(0),
//                    Title = reader.GetString(1),
//                    Author = reader.GetString(2),
//                    IsAvailable = reader.GetBoolean(3)
//                };

//                books.Add(book);
//            }
//            return books;
//        }

//        public Book? GetById(int id)
//        {
//            string sql = @"SELECT Id, Title, Author, IsAvailable
//                           FROM Books
//                           WHERE Id = @Id";

//            using SqlConnection connection = new SqlConnection(_connectionString);
//            using SqlCommand command = new SqlCommand(sql, connection);

//            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

//            connection.Open();

//            using SqlDataReader reader = command.ExecuteReader();

//            if (reader.Read())
//            {
//                return new Book
//                {
//                    Id = reader.GetInt32(0),
//                    Title = reader.GetString(1),
//                    Author = reader.GetString(2),
//                    IsAvailable = reader.GetBoolean(3)
//                };
//            }
//            return null;
//        }

//        public bool Update(Book book)
//        {
//            string sql = @"Update Books
//                           SET Title = @Title,
//                               Author = @Author,
//                               IsAvailable = @IsAvailable
//                           WHERE Id = @Id";

//            using SqlConnection connection = new SqlConnection(_connectionString);
//            using SqlCommand command = new SqlCommand(sql, connection);

//            command.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;
//            command.Parameters.Add("@Title", SqlDbType.NVarChar, 150).Value = book.Title;
//            command.Parameters.Add("@Author", SqlDbType.NVarChar).Value = book.Author;
//            command.Parameters.Add("@IsAvailable", SqlDbType.Bit).Value = book.IsAvailable;

//            connection.Open();

//            int rowsAffected = command.ExecuteNonQuery();

//            return rowsAffected > 0;
//        }

//        public bool Delete(int id)
//        {
//            string sql = "DELETE FROM Books WHERE Id = @Id";

//            using SqlConnection connection = new SqlConnection(_connectionString);
//            using SqlCommand command = new SqlCommand(sql, connection);

//            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

//            connection.Open();

//            int rowsAffected = command.ExecuteNonQuery();

//            return rowsAffected > 0;
//        }
//    }
//}

