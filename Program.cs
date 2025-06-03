using TodoApi.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataBaseContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapScalarApiReference();
    app.MapOpenApi();
    app.MapScalarApiReference();

    // app.UseSwaggerUi(options =>
    // {
    //     options.DocumentPath = "/openapi/v1.json";
    // });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// public void ConfigureServices(IServiceCollection services)
// {
//     services.AddDbContext<SchoolContext>(options =>
//         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//     services.AddDatabaseDeveloperPageExceptionFilter();

//     services.AddControllersWithViews();
// }