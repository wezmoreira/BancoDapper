using System;
using Microsoft.Data.SqlClient;
using Dapper;
using BancoDapper.Models;
using System.Data;

const string connectionString = "server=localhost,1433;database=balta;TrustServerCertificate=True;User ID=sa;Password=1q2w3e4r@#$";





using (var connection = new SqlConnection(connectionString))
{
    //ListCategories(connection);
    //CreateManyCategoy(connection);
    //UpdateCategoy(connection);
    //DeleteCategory(connection);
    //Console.WriteLine("*******************");
    //ListCategories(connection);
    //ExecuteProcedure(connection);
    //ExecuteReadProcedure(connection);
    //ExecuteScalar(connection);
    //OneToOne(connection);
    //OneToMany(connection);
    //QueryMultiple(connection);
    //SelectIn(connection);
    //Like(connection, "backend");
    Transaction(connection);
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

static void DeleteCategory(SqlConnection connection)
{
    var deleteQuery = "DELETE [Category] WHERE [Id]=@id";
    var rows = connection.Execute(deleteQuery, new
    {
        id = new Guid("5969dee2-ce2f-42d4-8b28-c5f46024e168"),
    });

    Console.WriteLine($"{rows} registros excluídos");
}

static void GetCategory(SqlConnection connection)
{
    var category = connection
        .QueryFirstOrDefault<Category>(
            "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
            new
            {
                id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
            });
    Console.WriteLine($"{category.Id} - {category.Title}");

}

static void CreateManyCategoy(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços AWS";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;

    var category2 = new Category();
    category2.Id = Guid.NewGuid();
    category2.Title = "Categoria Nova";
    category2.Url = "Categoria Nova";
    category2.Description = "Categoria Nova";
    category2.Order = 9;
    category2.Summary = "Categoria";
    category2.Featured = true;




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

    var rows = connection.Execute(insertSql, new[]
    {
        new
    {
        category.Id,
        category.Title,
        category.Url,
        category.Description,
        category.Order,
        category.Summary,
        category.Featured

    },
        new
    {
        category2.Id,
        category2.Title,
        category2.Url,
        category2.Description,
        category2.Order,
        category2.Summary,
        category2.Featured

    }
    });

    Console.WriteLine($"{rows} Linhas inseridas");
}

static void ExecuteProcedure(SqlConnection connection)
{
    var procedure = "[spDeleteStudent]";

    var parms = new { StudentId = "5192f822-cc82-489f-8e29-48156bc1414d" };
    var affectedRows = connection.Execute(procedure, parms, commandType: CommandType.StoredProcedure);

    Console.WriteLine($"{affectedRows} - linhas afetadas");
}

static void ExecuteReadProcedure(SqlConnection connection)
{
    var procedure = "[spGetCoursesByCategory]";

    var parms = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
    var courses = connection.Query(procedure, parms, commandType: CommandType.StoredProcedure);

    foreach(var item in courses)
    {
        Console.WriteLine(item.Title);
    }
}

static void ExecuteScalar(SqlConnection connection)
{
    var category = new Category();
    category.Title = "Amazon AWS";
    category.Url = "amazon";
    category.Description = "Categoria destinada a serviços AWS";
    category.Order = 8;
    category.Summary = "AWS Cloud";
    category.Featured = false;



    var insertSql = @"INSERT INTO 
        [CATEGORY] 
        OUTPUT inserted.[Id]
        VALUES(
            NEWID(), 
            @Title, 
            @Url, 
            @Summary, 
            @Order, 
            @Description, 
            @Featured)
        SELECT SCOPE_IDENTITY()";

    var id = connection.ExecuteScalar<Guid>(insertSql, new
    {
        category.Title,
        category.Url,
        category.Description,
        category.Order,
        category.Summary,
        category.Featured
    });

    Console.WriteLine($"A categoria inserida foi: {id}");
}

static void ReadView(SqlConnection connection)
{
    var sql = "SELECT * FROM [vwCourses]";
    var courses = connection.Query("SELECT [Id], [Title] FROM [Category]");
    foreach (var item in courses)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void OneToOne(SqlConnection connection)
{
    var sql = @"
            select 
                * 
            from 
                [CareerItem] 
            inner join 
                [Course] on [CareerItem].[CourseId] = [Course].[Id]
            ";

    var item = connection.Query<CareerItem, Course, CareerItem>(sql, 
    (careerItem, course)=>
    {
        careerItem.Course = course;
        return careerItem;
    }, splitOn: "Id");

    foreach(var items in item)
    {
        Console.WriteLine($"{items.Title} ************** Curso: {items.Course.Title}");
        Console.WriteLine();
    }
}

static void OneToMany(SqlConnection connection)
{
    var sql = @"
            select 
                [Career].[Id],
                [Career].[Title],
                [CareerItem].[CareerId],
                [CareerItem].[Title]
            from
                [Career]
            inner join 
                [CareerItem] on [CareerItem].[CareerId] = [Career].[Id]
            order BY
                [Career].[Title]
            ";

    var careers = new List<Career>();

    var items = connection.Query<Career, CareerItem, Career>(sql,
    (career, item) =>
    {
        var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
        if (car == null)
        {
            car = career;
            car.Items.Add(item);
            careers.Add(car);
        }
        else
        {
            car.Items.Add(item);
        }
        return career;
    }, splitOn: "CareerId");

    foreach (var career in careers)
    {
        Console.WriteLine($"{career.Title}");
        foreach(var item in career.Items)
        {
            Console.WriteLine($" - {item.Title}");
        }
        Console.WriteLine();
    }
}

static void QueryMultiple(SqlConnection connection)
{
    var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

    using (var multi = connection.QueryMultiple(query))
    {
        var categories = multi.Read<Category>();
        var courses = multi.Read<Course>();

        foreach(var item in categories)
        {
            Console.WriteLine($"-- {item.Title}");
            Console.WriteLine();
        }

        Console.WriteLine("****************************************************************************");
        Console.WriteLine();

        foreach(var item in courses)
        {
            Console.WriteLine($"-- {item.Title}");
            Console.WriteLine();
        }
    }
}

static void SelectIn(SqlConnection connection)
{
    var query = @"select top 10
                    * 
                from 
                    [Career] 
                where 
                    [Id] in @Id";

    var items = connection.Query<Career>(query, new
    {
        Id = new[]
        {
            "01ae8a85-b4e8-4194-a0f1-1c6190af54cb", "e6730d1c-6870-4df3-ae68-438624e04c72"
        }
    });

    foreach (var item in items)
    {
        Console.WriteLine(item.Title);
    }
}

static void Like(SqlConnection connection, string term)
{
    var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

    var items = connection.Query<Course>(query, new
    {
        //exp = "%backend%"
        exp = $"%{term}%"
    });

    foreach (var item in items)
    {
        Console.WriteLine(item.Title);
    }
}

static void Transaction(SqlConnection connection)
{
    var category = new Category();
    category.Id = Guid.NewGuid();
    category.Title = "Minha categoria que não";
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

    connection.Open();
    
    using(var transaction = connection.BeginTransaction())
        {
        var rows = connection.Execute(insertSql, new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Description,
            category.Order,
            category.Summary,
            category.Featured
        }, transaction);
        transaction.Rollback();

        Console.WriteLine($"{rows} Linhas inseridas");

    }

}