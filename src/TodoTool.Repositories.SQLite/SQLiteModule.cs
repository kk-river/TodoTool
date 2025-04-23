using System.Data;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoTool.Modularity;
using TodoTool.Repositories;

namespace TodoTool;

public class SQLiteModule : IModule
{
    public void Configure(IHostApplicationBuilder builder)
    {
        SQLiteConnectionStringBuilder sqliteConnectionStringBuilder = new()
        {
            //DataSource = ":memory:" //オンメモリ
            DataSource = Path.Combine(Environment.CurrentDirectory, "TodoTool.db") //ファイル
        };

        builder.Services.AddSingleton(sqliteConnectionStringBuilder);
        builder.Services.AddScoped<IDbConnection>(static provider =>
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = provider.GetRequiredService<SQLiteConnectionStringBuilder>();
            SQLiteConnection connection = new(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        });

        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();

        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
    }
}

/// <summary>
/// SQLite does not support DateTimeOffset, so we need to convert it to a string.
/// </summary>
file class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override DateTimeOffset Parse(object value)
    {
        if (DateTimeOffset.TryParse(value.ToString(), out DateTimeOffset result))
        {
            return result;
        }

        throw new ArgumentException($"Invalid DateTimeOffset value: {value}");
    }

    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
        parameter.Value = value.ToString("u");
    }
}
