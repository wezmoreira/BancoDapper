using System;
using Microsoft.Data.SqlClient;
using Dapper;
using BancoDapper.Models;

const string connectionString = "server=localhost,1433;database=balta;TrustServerCertificate=True;User ID=sa;Password=1q2w3e4r@#$";





using (var connection = new SqlConnection(connectionString))
{
    ListCategories(connection);
    //CreateCategoy(connection);
    UpdateCategoy(connection);
    Console.WriteLine("*******************");
    ListCategories(connection);
}


static void ListCategories(SqlConnection connection) {
    var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
    foreach (var item in categories)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void CreateCategoy(SqlConnection connection)
{
    var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;



    var insertSql = @"INSERT INTO 
        [CATEGORY] 
        VALUES(
        @Id, 
        @Title, 
        @Url, 
        @Summary, 
        @Order, 
        @Description, 
        @Featured)";

    var rows = connection.Execute(insertSql, new
    {
        category.Id,
        category.Title,
        category.Url,
        category.Description,
        category.Order,
        category.Summary,
        category.Featured
    });

    Console.WriteLine($"{rows} Linhas inseridas");
}

static void UpdateCategoy(SqlConnection connection)
{
    var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@Id";
    var rows = connection.Execute(updateQuery, new
    {
        Id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
        Title = "Frontend 2022"
    });

    Console.WriteLine($"{rows} Registros atualizadas");
}