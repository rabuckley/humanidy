using Humanidy.Examples.Model;
using Humanidy.WebApiExample;
using Humanidy.WebApiExample.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddSingleton<TodoRepository>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

var v1 = app.MapGroup("v1.0");
var todosApi = v1.MapGroup("todos");

todosApi.MapGet("/", ([FromServices] TodoRepository repository) => Results.Ok(repository.List()));

todosApi.MapGet("/{id}", (
    [FromRoute(Name = "id")] TodoId id,
    [FromServices] TodoRepository repository) => repository.GetById(id) switch
{
    Todo todo => Results.Ok(todo),
    null => Results.NotFound()
});

app.Run();
