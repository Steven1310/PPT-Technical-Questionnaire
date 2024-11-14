using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7125); // HTTP
    serverOptions.ListenAnyIP(7126, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add services to the container.
var sqliteDBSettings = builder.Configuration.GetSection("SqliteDBSettings").Get<SqliteDBSettings>();
if (sqliteDBSettings == null || string.IsNullOrEmpty(sqliteDBSettings.DB_URI) || string.IsNullOrEmpty(sqliteDBSettings.DatabaseName))
{
    throw new InvalidOperationException("SQLite configuration is missing or invalid.");
}
builder.Services.Configure<SqliteDBSettings>(builder.Configuration.GetSection("SqliteDBSettingsDev"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlite($"Data Source={sqliteDBSettings.DB_URI}{sqliteDBSettings.DatabaseName}"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IImageRepository, SqliteDBImageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

