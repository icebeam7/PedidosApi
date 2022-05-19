var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PedidosContext>(options =>
{
    var server = Environment.GetEnvironmentVariable("SERVERNAME");
    var database = Environment.GetEnvironmentVariable("DATABASE");
    var username = Environment.GetEnvironmentVariable("USERNAME");
    var password = Environment.GetEnvironmentVariable("PASSWORD");

    var connectionString = $"Server={server};Initial Catalog={database};Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/ordenes", async (PedidosContext db) =>
    await db.Ordenes.ToListAsync()
)
.Produces<List<Ordenes>>(StatusCodes.Status200OK)
.WithName("GetOrdenes").WithTags("Ordenes");

app.MapGet("/api/ordenes/{id}", async (PedidosContext db, int id) =>
{
    try
    {
        var x = await db.Ordenes.FindAsync(id) is Ordenes orden ? Results.Ok(orden) : Results.NotFound();
        return x;
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
}
)
.Produces<Ordenes>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetOrdenByID").WithTags("Ordenes");

app.MapPost("/api/ordenes",
    async ([FromBody] Ordenes nuevaOrden, [FromServices] PedidosContext db, HttpResponse response) =>
    {
        db.Ordenes.Add(nuevaOrden);
        await db.SaveChangesAsync();
        return Results.Ok(nuevaOrden);
    })
.Accepts<Ordenes>("application/json")
.Produces<Ordenes>(StatusCodes.Status201Created)
.WithName("AddNewOrden").WithTags("Ordenes");

app.MapPut("/api/ordenes/{id}", async (int id, [FromBody] Ordenes updatedOrden, [FromServices] PedidosContext db, HttpResponse response) =>
{
    if (id != updatedOrden.OrdenesId)
        return Results.BadRequest();

    db.Entry(updatedOrden).State = EntityState.Modified;

    try
    {
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.WithName("UpdateOrden").WithTags("Ordenes");

app.MapDelete("/api/ordenes/{id}", async (int id, [FromServices] PedidosContext db, HttpResponse response) =>
{
    var orden = await db.Ordenes.FindAsync(id);

    if (orden == null)
        return Results.NotFound();

    db.Ordenes.Remove(orden);

    await db.SaveChangesAsync();
    return Results.Ok(orden);
})
.Produces<Ordenes>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("DeleteOrden").WithTags("Ordenes");

// Ordenes_Detalle
app.MapGet("/api/ordenes_detalle", async (PedidosContext db) =>
    await db.Ordenes_Detalle.ToListAsync()
)
.Produces<List<Ordenes_Detalle>>(StatusCodes.Status200OK)
.WithName("GetOrdenesDetalle").WithTags("Ordenes_Detalle");

app.MapGet("/api/ordenes_detalle/{id}", async (PedidosContext db, int id) =>
{
    try
    {
        var x = await db.Ordenes_Detalle.FindAsync(id) is Ordenes_Detalle orden_detalle ? Results.Ok(orden_detalle) : Results.NotFound();
        return x;
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
}
)
.Produces<Ordenes_Detalle>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetOrdenDetalleByID").WithTags("Ordenes_Detalle");

app.MapPost("/api/ordenes_detalle",
    async ([FromBody] Ordenes_Detalle nuevaOrdenDetalle, [FromServices] PedidosContext db, HttpResponse response) =>
    {
        db.Ordenes_Detalle.Add(nuevaOrdenDetalle);
        await db.SaveChangesAsync();
        return Results.Ok(nuevaOrdenDetalle);
    })
.Accepts<Ordenes_Detalle>("application/json")
.Produces<Ordenes_Detalle>(StatusCodes.Status201Created)
.WithName("AddNewOrdenDetalle").WithTags("Ordenes_Detalle");

app.MapPut("/api/ordenes_detalle/{id}", async (int id, [FromBody] Ordenes_Detalle updatedOrdenDetalle, [FromServices] PedidosContext db, HttpResponse response) =>
{
    if (id != updatedOrdenDetalle.OrdenesDetalleId)
        return Results.BadRequest();

    db.Entry(updatedOrdenDetalle).State = EntityState.Modified;

    try
    {
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.WithName("UpdateOrdenDetalle").WithTags("Ordenes_Detalle");

app.MapDelete("/api/ordenes_detalle/{id}", async (int id, [FromServices] PedidosContext db, HttpResponse response) =>
{
    var orden_detalle = await db.Ordenes_Detalle.FindAsync(id);

    if (orden_detalle == null)
        return Results.NotFound();

    db.Ordenes_Detalle.Remove(orden_detalle);

    await db.SaveChangesAsync();
    return Results.Ok(orden_detalle);
})
.Produces<Ordenes_Detalle>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("DeleteOrdenDetalle").WithTags("Ordenes_Detalle");


app.Run();