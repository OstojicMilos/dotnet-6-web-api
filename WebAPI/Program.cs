using CleanArchitecture.WebAPI;
using CleanArchitecture.WebAPI.Filters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>())
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
    .ConfigureApiBehaviorOptions(options =>
    {
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var errorMessages = context.ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage);
            logger.LogError($"Validation errors have been found: {string.Join("|", errorMessages)}");

            return builtInFactory(context);
        };
    });
builder.Services.AddServices(builder.Configuration);
builder.Services.AddOpenApiDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.SetSensoryRanges();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.EnsureDatabaseSetup();
}

app.UseHttpsRedirection();

app.UseOpenApiDocumentation();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();