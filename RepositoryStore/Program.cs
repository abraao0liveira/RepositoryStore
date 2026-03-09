using Microsoft.EntityFrameworkCore;
using RepositoryStore.Data;
using RepositoryStore.Models;
using RepositoryStore.Repositories;
using RepositoryStore.Repositories.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(connectionString);
});
// Add services to the container.
builder.Services.AddTransient<IProductRepository, ProductRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/v1/products", async (CancellationToken token, IProductRepository repository)
    => Results.Ok(await repository.GetAllAsync(0, 5, token)));

app.MapGet("/v1/products/{id}", async (CancellationToken token, IProductRepository repository, int id)
    => Results.Ok(await repository.GetByIdAsync(id, token)));

app.MapPost("/v1/products/{id}", async (CancellationToken token, IProductRepository repository, Product product)
    => Results.Ok(await repository.CreateAsync(product, token)));

app.MapPut("/v1/products", async (CancellationToken token, IProductRepository repository, Product product)
    => Results.Ok(await repository.UpdateAsync(product, token)));

app.MapDelete("/v1/products/{id}", async (CancellationToken token, IProductRepository repository, int id) =>
{
    var product = await repository.GetByIdAsync(id);
    return product == null ? Results.NotFound()
        : Results.Ok(await repository.DeleteAsync(product, token));
});

app.Run();
