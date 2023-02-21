using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});

// Add services to the container.
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


app.MapGet("/cachedefault", () =>
{
    var response = "Resposta gerada em:" + DateTime.Now.ToString();
    return response;
})
.CacheOutput()
.WithName("CacheDefault")
.WithOpenApi();

app.MapGet("/cachelong", (string param) =>
{
    var response = "Resposta gerada em:" + DateTime.Now.ToString();
    return response;
})
.CacheOutput(policy =>
{
    policy.SetVaryByQuery("param");
    policy.Expire(TimeSpan.FromSeconds(60));
})
.WithName("CacheLong")
.WithOpenApi();

app.UseOutputCache();

app.Run();

