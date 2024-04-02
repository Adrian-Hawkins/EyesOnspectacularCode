using EOSC.API.Auth;
using EOSC.API.Service.base64;
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

builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// services.AddAuthentication(options => 
// {
// options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
// options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
// });


//app.UseAuthentication();


app.MapControllers();

app.Run();