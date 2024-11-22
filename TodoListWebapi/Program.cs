using Microsoft.EntityFrameworkCore;
using TodoListWebapi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//This is for Cors "very important"
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicys", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//User sql connection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer
    (builder.Configuration.GetConnectionString("SqlServerConnectionStr"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicys");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
