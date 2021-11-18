using System.Net.Mime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Se comenta en nel caso #9
///app.MapGet("/", () => "Hello World!");
app.MapGet("/oops", () => "Oops! An error happened.");


//Caso #1
//TODO: ¿Que pasaria si se cambiara de puerto?
//Antes
///app.Run();
//Despues 
///app.Run("http://localhost:3000");


//Caso #2
//TODO: ¿Que pasaria si lo ocupamos en multiples puertos?
///app.Urls.Add("http://localhost:3000"); // Aqui puede ser https
///app.Urls.Add("http://localhost:4000");
///app.Run();


//Caso #3
//TODO: Si lo trabajas con .NET CLI
//Ejemplo
//dotnet run --urls="https://localhost:7777"


//Caso #4
//TODO: ¿Que pasaria si lo trabajaras con Enviroment?
///var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
///app.Run($"http://localhost:{port}");

//Caso #5
//TODO: ¿Que pasaria si quiremos cargar  un certificado personalizado?
/*
 {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Kestrel": {
    "Certificates": {
      "Default": {
        "Path": "cert.pem",
        "KeyPath": "key.pem"
      }
    }
  }
}
 
 */

//TODO: Ya usando el certificado
/*

using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        var certPath = Path.Combine(builder.Environment.ContentRootPath, "cert.pem");
        var keyPath = Path.Combine(builder.Environment.ContentRootPath, "key.pem");

        httpsOptions.ServerCertificate = X509Certificate2.CreateFromPemFile(certPath, 
                                         keyPath);
    });
});

var app = builder.Build();

app.Urls.Add("https://localhost:3000");

app.MapGet("/", () => "Hello World");

app.Run();
 
 */

//Caso #6

//TODO: ¿Que pasaria si estuvieramos en modo desarrollador?
///if (!app.Environment.IsDevelopment())
///{
///    app.UseExceptionHandler("/oops");
///}

///app.Run();


//Caso #7

//TODO: ¿Que pasaria si se usara el acceso a configuration?

///var message = app.Configuration["MyKey"] ?? "Si no hubiera key";

///app.MapGet("/configuration", () => message);

//app.Run();



//Caso #8

//TODO: ¿Que pasaria si se usara logging?

///app.Logger.LogInformation("Se esta accediendo a configuration");

///var message = app.Configuration["MyKey"] ?? "Si no hubiera key";

///app.MapGet("/configuration", () => message);

///app.Run();



//Caso #9

//TODO:  Uso de Routing

///app.MapGet("/", () => "This is a GET");
///app.MapPost("/", () => "This is a POST");
///app.MapPut("/", () => "This is a PUT");
///app.MapDelete("/", () => "This is a DELETE");

///app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" },
//                          () => "This is an options or head request ");

///app.Run();


//caso #10

//TODO: Uso de los lambda expresion

///app.MapGet("/inline", () => "This is an inline lambda");

///var handler = () => "This is a lambda variable";

///app.MapGet("/functionLambda", handler);


//caso #11

//TODO: Uso de funciones locales
///string LocalFunction() => "This is local function";

///app.MapGet("/localFunction", LocalFunction);

///app.Run();


//caso #12
//TODO: Uso de clases e instancias

/*
var objhandler = new HelloHandler();

app.MapGet("/usoClases", objhandler.Hello);

app.Run();

class HelloHandler
{
    public string Hello()
    {
        return "Hello Instance method";
    }
}
*/

//Caso #13
//TODO: Uso de routes and link generator

app.MapGet("/hello", () => "Hello named route")
   .WithName("hi");

app.MapGet("/", (LinkGenerator linker) =>
        $"The link to the hello route is {linker.GetPathByName("hi", values: null)}");




//Caso  #14
//TODO: Uso de los parametros
app.MapGet("/users/{userId}/books/{bookId}",
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");




//Caso #15
//TODO: Route constrains

//app.MapGet("/todos/{id:int}", (int id) => db.Todos.Find(id));
//app.MapGet("/todos/{text}", (string text) => db.Todos.Where(t => t.Text.Contains(text));
//app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");

/*

/todos/{id:int}	                    /todos/1
/todos/{text}	                    /todos/something
/posts/{slug:regex(^[a-z0-9_-]+$)}	/posts/mypost
 */


//Caso #16
//TODO: Optional parameters
app.MapGet("/products", (int pageNumber) => $"Requesting page {pageNumber}");

/// <summary>
/// /products?pageNumber=3
/// 
/// </summary>
///
app.MapGet("/productsv2", (int? pageNumber) => $"Requesting page {pageNumber ?? 1}");

//TODO: Otra forma de hacerlo
string ListProducts(int pageNumber = 1) => $"Requesting page {pageNumber}";

app.MapGet("/productsv3", ListProducts);


//Caso #17

//TODO : regresemo un HTML

app.MapGet("/html", () => Results.Extensions.Html(@$"<!doctype html>
<html>
    <head><title>miniHTML</title></head>
    <body>
        <h1>Hello World</h1>
        <p>The time on the server is {DateTime.Now:O}</p>
    </body>
</html>"));




//Caso #18

app.MapGet("response", async () =>
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos/");
    response.EnsureSuccessStatusCode();
    string responsebody = await response.Content.ReadAsStringAsync();
    return responsebody;

});




app.Run();





static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions resultExtensions, string html)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions, nameof(resultExtensions));

        return new HtmlResult(html);
    }
}

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}

//Ejemplos de la pagina oficcioal de microsoft
//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

