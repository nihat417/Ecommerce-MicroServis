using Ecommerce.Orders.WebAPI.Context;
using Ecommerce.Orders.WebAPI.Dtos;
using Ecommerce.Orders.WebAPI.Entites;
using Ecommerce.Orders.WebAPI.Models;
using Ecommerce.Orders.WebAPI.Options;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

OrderClassMap.Register();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

app.MapGet("/api/Order/getAllOrder", async ([FromServices] MongoDbContext context,IConfiguration configuration) =>
{
    var items = context.GetCollection<Order>("Orders");
    var orders = await items.Find(item => true).ToListAsync();
    
    List<OrderDto> orderdtos = new();
    Result<List<ProductDto>>? products = new();

    HttpClient httpclient = new HttpClient();
    string productsEndpoint = $"http://{ configuration.GetSection("HttpRequest:Products").Value}/api/Product/GetAllProducts";
    var message = await httpclient.GetAsync(productsEndpoint);

    if (!message.IsSuccessStatusCode)
    {
        return new OperationResult
        {
            Success = false,
            Message = "Failed to retrieve products from external service."
        };
    }

    products= await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();

    foreach (var order in orders)
    {
        OrderDto orderDto = new()
        {
            Id = order.Id,
            ProductId = order.ProductId,
            Price = order.Price,
            Quantity = order.Quantity,
            ProductName = products!.Data!.First(p =>p.Id == order.ProductId).Name,
            CreatedDate = order.CreatedDate,
        };
        orderdtos.Add(orderDto);
    }
    return new OperationResult
    {
        Success = true,
        Data = new {Products = orderdtos}
    };
});

app.MapPost("/api/Order/createOrder", async ([FromServices] MongoDbContext context, [FromBody] List<CreateOrderDto> request) =>
{
    var items = context.GetCollection<Order>("Orders");
    List<Order> orders = new();

    foreach (var item in request)
    {
        Order order = new()
        {
            ProductId = item.ProductId,
            Price = item.Price,
            Quantity = item.Quantity,
            CreatedDate = DateTime.Now,
        };
        orders.Add(order);
    }
    await items.InsertManyAsync(orders);

    return new OperationResult
    {
        Success = true,
        Message = "Order created sucsesfully"
    };
});



app.Run();
