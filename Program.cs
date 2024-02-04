using UrlShorter;
using UrlShorter.Models;
using Microsoft.EntityFrameworkCore;
using UrlShorter.Extensions;
using UrlShorter.Entittes;
using UrlShorter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<UrlServices>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigration();
}

app.MapPost("api/shorten", async (ShortenUrlRequest request,
    UrlServices urlServices,
    AppDbContext dbContext,
    HttpContext httpContext) => {

        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest("Неправильно введённая ссылка");
        }
        var code = await urlServices.GenerateUniqueCode();

        var resUrl = new ShortenUrl {
            Id = Guid.NewGuid(),
            LongUrl = request.Url,
            Code = code,
            ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
            CreateOnUTC = DateTime.Now
        };

        dbContext.ShortenUrls.Add(resUrl);

        await dbContext.SaveChangesAsync();

        return Results.Ok();

});
app.UseHttpsRedirection();

app.Run();
