using EOSC.API.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // TODO: Add support for Github Tokens
    var openApiSecurityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
        Type = SecuritySchemeType.Http,
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header
    };
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme());
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { openApiSecurityScheme, new List<string>() }
    });
});

builder.Services.AddSingleton<IBase64Service, Base64Service>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();