using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

//DB in memory
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));

//SQL Server
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

// Shows if the database is created in memory
app.MapGet("/dbconexion",async ([FromServices] TareasContext dbContext)=>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});

// Shows the data uploaded in the database using GET request
app.MapGet("/api/tareas",async ([FromServices] TareasContext dbContext)=>
{
    //return Results.Ok(dbContext.Tareas);

    // Show tareas including also info of property categoria
    return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria));
});

// Upload data using a POST request
app.MapPost("/api/tareas",async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea)=>
{
    // Default fields autofill
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;

    await dbContext.AddAsync(tarea);

    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

// Update data using a PUT request locating data by ID
app.MapPut("/api/tareas/{id}",async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id)=>
{
    var tareaActual = dbContext.Tareas.Find(id);

    // Check if tareaID is located in the database
    if(tareaActual != null)
    {
        //Update new values for tarea
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    // Return not found if the id provided is not in any field
    return Results.NotFound();
});


// Delete data using a DELETE request locating data by ID
app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null)
    {
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

app.Run();
