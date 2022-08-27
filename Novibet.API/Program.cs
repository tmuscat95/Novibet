using Microsoft.EntityFrameworkCore;
using Novibet.API.Data;
using Novibet.API.Services;
using Novibet.API.Services.Interfaces;
using Novibet.Library;
using Novibet.Library.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IPDetailsDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IPDetails")));
builder.Services.AddScoped<IIPInfoProvider,IPInfoProvider>();
builder.Services.AddScoped<IServiceIPDetails,ServiceIPDetails>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IPDetailsDBContext>();
    context.Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
