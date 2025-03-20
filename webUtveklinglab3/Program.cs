using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));

//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/api/talents", async (ApplicationContext context) =>
{
    return Results.Ok(await context.Skills.ToListAsync());
});

app.MapPost("/api/talents", async (ApplicationContext context, Skill skill) =>
{
    context.Skills.Add(skill);
    await context.SaveChangesAsync();
    return Results.Created($"/api/talents/{skill.Id}", skill);
});

app.MapGet("/api/projects", async (ApplicationContext context) =>
{
    return Results.Ok(await context.Projects.ToListAsync());
});

app.MapPost("/api/projects", async (ApplicationContext context, Project project) =>
{
    context.Projects.Add(project);
    await context.SaveChangesAsync();
    return Results.Created($"/api/projects/{project.Id}", project);
});

app.MapPost("/api/projects/{id}", async (ApplicationContext context, int id, Project project) =>
{
    if (id != project.Id)
    {
        return Results.BadRequest();
    }

    context.Entry(project).State = EntityState.Modified;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/api/projects/bulk", async (ApplicationContext context, List<Project> projects) =>
{
    if (projects == null || projects.Count == 0)
    {
        return Results.BadRequest("Projects list is empty.");
    }

    context.Projects.AddRange(projects);
    await context.SaveChangesAsync();
    return Results.Ok(new { message = $"{projects.Count} projects added successfully.", data = projects });
});

//app.UseCors("AllowAll");
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseAuthorization();

// app.MapControllers();

app.Run();