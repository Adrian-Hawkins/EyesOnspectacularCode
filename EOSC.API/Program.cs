using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using EOSC.API.Auth;
using EOSC.API.Service.base64;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });

//builder.Services.AddAuthentication().Add

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // TODO: Add support for Github Tokens
    OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
        Type = SecuritySchemeType.ApiKey,
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


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "Bearer";
//    options.DefaultChallengeScheme = "Bearer";
//});

// builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
// builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers.TryGetValue("Authorization", out var authHeaderValue);
    //todo: un-auth:
    if (authHeader)
    {
    }

    // Remove the 'Bearer ' if it exists.
    var token = authHeaderValue.ToString()["Bearer ".Length..].Trim();

    var tokenHandler = new JwtSecurityTokenHandler();
    // var key = Encoding.ASCII.GetBytes(JwtSecretKey);

    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});


// services.AddAuthentication(options => 
// {
// options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
// options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
// });


//app.UseAuthentication();


app.MapControllers();

app.Run();