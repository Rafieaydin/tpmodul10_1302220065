using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Model.Mahasiwsa;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<mhsDB>(opt => opt.UseInMemoryDatabase("mahasiswa"));
builder.Services.AddOpenApiDocument(config =>
{   
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";  
});

var app = builder.Build();

// configure swagger tutorial di https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", async  (mhsDB db) =>
{
    var mhs = new Mahasiwsa[]
{
        new Mahasiwsa { Id = 1, Nama = "Nasya Kirana Marendra", Nim = "1302223148" },
        new Mahasiwsa { Id = 2, Nama = "Zabrina Virgie", Nim = "1302223055" },
        new Mahasiwsa { Id = 3, Nama = "Dara Sheiba M.C", Nim = "1302223075 " },
        new Mahasiwsa { Id = 4, Nama = "M Tsaqif Zayyan", Nim = "1302220141" },
        new Mahasiwsa { Id = 5, Nama = "M Arifin Ilham", Nim = "1302223061" },
        new Mahasiwsa { Id = 6, Nama = "Rafie Aydin Ihsan", Nim = "1302220065" },
};
    db.mhs.AddRange(mhs);
    await db.SaveChangesAsync();
    // dari aspnetcore.httml
    return Results.Redirect("/swagger"); // redirect to swagger
});

app.MapGet("/mahasiswa", async (mhsDB db) =>
{
    // default data using array list 

    return Results.Ok(await db.mhs.ToListAsync());
});

app.MapGet("/mahasiswa/{id}", async (mhsDB db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(mhs);
});

app.MapPost("/mahasiswa", async (mhsDB db, Mahasiwsa mhs) =>
{
    Console.WriteLine(mhs);
    db.mhs.Add(mhs);
    await db.SaveChangesAsync();
    return Results.Created($"/mahasiswa/{mhs.Id}", mhs);
});

app.MapPut("/mahasiswa/{id}", async (mhsDB db, int id, Mahasiwsa mhs) =>
{
    if (id != mhs.Id)
    {
        return Results.BadRequest();
    }
    db.Entry(mhs).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
    
app.MapDelete("/mahasiswa/{id}", async (mhsDB db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    db.mhs.Remove(mhs);
    await db.SaveChangesAsync();
    return Results.NoContent();
});







app.Run();
