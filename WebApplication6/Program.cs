using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

List<DryStorage> repo = [];
app.MapGet("/dry_storage", () => repo);

app.MapPost("/dry_storage", (CreateDryStorageDTO dto) =>
{
    var dryStorage = new DryStorage
    {
        id = Guid.NewGuid(),
        name = dto.name,
        view = dto.view,
        count = dto.count,
        DateNow = DateTime.Now,
        DateEnd = DateTime.Now.AddDays(7)
    };
    repo.Add(dryStorage);
});

app.MapPut("/dry_storage", ([FromQuery] Guid id, UpdateDryStorageDTO dto) =>
{
    var buffer = repo.Find(x => x.id == id);
    if (buffer != null)
    {
        buffer.count = dto.count;
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/dry_storage", ([FromQuery] Guid id) =>
{
    var buffer = repo.Find(x => x.id == id);
    if (buffer != null)
    {
        repo.Remove(buffer);
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();

class DryStorage
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string view { get; set; }
    public int count { get; set; }
    public DateTime DateNow { get; set; }
    public DateTime DateEnd { get; set; }
}

record class CreateDryStorageDTO(string name, string view, int count);
record class UpdateDryStorageDTO(int count);
