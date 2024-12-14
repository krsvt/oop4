using System.Text.Json;
using System.Text;
using Representation.Configuration;
using BLL.Service;
using DAL.Entities;
using BLL.DTO;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});


builder.Configuration
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                       optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();


builder.Services.AddStorage(builder.Configuration);

builder.Services.AddScoped<FamilyTreeService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<UnionService>();
var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


var tree = app.MapGroup("/api/tree");
var person = app.MapGroup("/api/person");
var union = app.MapGroup("/api/union");

// GET http://localhost:5000/api/tree
tree.MapGet("/", (FamilyTreeService s) =>

    {
        // var data = new
        //    {
        //        start = "id4",
        //        persons = new
        //        {
        //            id1 = new { id = "id1", name = "Adam", birthyear = 1900, deathyear = 1980, own_unions = new[] { "u1" }, birthplace = "Alberta", deathplace = "Austin" },
        //            id2 = new { id = "id2", name = "Berta", birthyear = 1901, deathyear = 1985, own_unions = new[] { "u1" }, birthplace = "Berlin", deathplace = "Bern" },
        //            id3 = new { id = "id3", name = "Charlene", birthyear = 1930, deathyear = 2010, own_unions = new[] { "u3", "u4" }, parent_union = "u1", birthplace = "Château", deathplace = "Cuxhaven" },
        //            id4 = new { id = "id4", name = "Dan", birthyear = 1926, deathyear = 2009, own_unions = new string[0], parent_union = "u1", birthplace = "den Haag", deathplace = "Derince" },
        //            id5 = new { id = "id5", name = "Eric", birthyear = 1931, deathyear = 2015, own_unions = new[] { "u3" }, parent_union = "u2", birthplace = "Essen", deathplace = "Edinburgh" },
        //            id6 = new { id = "id6", name = "Francis", birthyear = 1902, deathyear = 1970, own_unions = new[] { "u2" }, birthplace = "Firenze", deathplace = "Faizabad" },
        //            id7 = new { id = "id7", name = "Greta", birthyear = 1905, deathyear = 1990, own_unions = new[] { "u2" } },
        //            id8 = new { id = "id8", name = "Heinz", birthyear = 1970, own_unions = new[] { "u5" }, parent_union = "u3" },
        //            id9 = new { id = "id9", name = "Iver", birthyear = 1925, deathyear = 1963, own_unions = new[] { "u4" } },
        //            id10 = new { id = "id10", name = "Jennifer", birthyear = 1950, own_unions = new string[0], parent_union = "u4" },
        //            id11 = new { id = "id11", name = "Klaus", birthyear = 1933, deathyear = 2013, own_unions = new string[0], parent_union = "u1" },
        //            id12 = new { id = "id12", name = "Lennart", birthyear = 1999, own_unions = new string[0], parent_union = "u5" }
        //        },
        //        unions = new
        //        {
        //            u1 = new { id = "u1", partner = new[] { "id1", "id2" }, children = new[] { "id3", "id4", "id11" } },
        //            u2 = new { id = "u2", partner = new[] { "id6", "id7" }, children = new[] { "id5" } },
        //            u3 = new { id = "u3", partner = new[] { "id3", "id5" }, children = new[] { "id8" } },
        //            u4 = new { id = "u4", partner = new[] { "id3", "id9" }, children = new[] { "id10" } },
        //            u5 = new { id = "u5", partner = new[] { "id8" }, children = new[] { "id12" } }
        //        },
        //        links = new[]
        //        {
        //            new[] { "id1", "u1" },
        //            new[] { "id2", "u1" },
        //            new[] { "id6", "u2" },
        //            new[] { "id7", "u2" },
        //            new[] { "id3", "u3" },
        //            new[] { "id5", "u3" },
        //            new[] { "u3", "id8" },
        //            new[] { "id3", "u4" },
        //            new[] { "id9", "u4" },
        //            new[] { "id8", "u5" },
        //            new[] { "u2", "id5" },
        //            new[] { "u1", "id3" },
        //            new[] { "u1", "id4" },
        //            new[] { "u4", "id10" },
        //            new[] { "u1", "id11" },
        //            new[] { "u5", "id12" }
        //        }
        //    };
        //
        //
        // return data;
        return s.GetFamilyTree();
    }

    );

// DELETE http://localhost:5000/api/tree
tree.MapDelete("/", (FamilyTreeService s) => { s.DeleteTree(); return Results.NoContent(); });


// GET http://localhost:5000/api/person
person.MapGet("/", (PersonService s) => { return s.GetAll(); });

// GET http://localhost:5000/api/person/<id>/relatives
person.MapGet("/{id}/relatives", async (PersonService s, int id) =>
        {
            var a = await s.GetImmediateRelatives(id);
            Console.WriteLine("a " + a.ToString());
            return Results.Ok(a);
        });

// POST http://localhost:5000/api/person
person.MapPost("/", async (PersonService s, Person person) =>
    {
        await s.CreatePerson(person);
        return Results.Created($"/api/person/{person.Id}", person);
    });

// POST http://localhost:5000/api/union
union.MapPost("/", async (UnionService s, UnionDTO union) =>
    {
        await s.CreateUnion(union);
    });

tree.WithOpenApi();
person.WithOpenApi();
union.WithOpenApi();
app.Run();
