using LibraryApi.Application.Service;
using LibraryApi.Infra.Context;
using LibraryApi.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(policy =>
{
   policy.AddPolicy("_myAllowSpecificOrigins", builder =>
    builder.WithOrigins("http://localhost:64409/")
     .SetIsOriginAllowed((host) => true) // this for using localhost address
     .AllowAnyMethod()
     .AllowAnyHeader()
     .AllowCredentials());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AttachmentService>();
builder.Services.AddScoped<BookOrderService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddLogging();

// var logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(builder.Configuration)
//     .Enrich.FromLogContext()
//     .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//         new MSSqlServerSinkOptions()
//     {
//         AutoCreateSqlDatabase = false,
//         AutoCreateSqlTable = false,
//         TableName = "Logs"
//     })
//     .CreateLogger();
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog(logger);


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// IConfiguration configuration = new ConfigurationBuilder()
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .Build();

// builder.Host.UseSerilog((context, config) =>
// {
//     config
//     .ReadFrom.Configuration(configuration)
//     .Enrich.FromLogContext();
// });

// var Logger  = new LoggerConfiguration()
//     .ReadFrom.Configuration(configuration)
//     .CreateLogger();

// builder.Logging.AddSerilog(Logger);
// // builder.Host.UseSerilog(Logger);
// builder.Host.UseSerilog((context, config) =>
//     config.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();