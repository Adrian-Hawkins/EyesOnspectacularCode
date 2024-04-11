using System.Net.Http.Headers;
using System.Text;
using EOSC.API.Attributes;
using EOSC.API.Infra;
using EOSC.API.Middleware;
using EOSC.API.Repo;
using EOSC.API.Service;
using EOSC.API.Service.base64;
using EOSC.API.Service.github_auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EOSC.Common.Constant;

var builder = WebApplication.CreateBuilder(args);
BotAuth _botAuth = new BotAuth();

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtTokenConfig = builder.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>()!;
builder.Services.AddSingleton(jwtTokenConfig);


builder.Services.AddAuthentication(authenticationOptions =>
{
    authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.RequireHttpsMetadata = true;
    jwtBearerOptions.SaveToken = true;
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
        ValidAudience = jwtTokenConfig.Audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddSingleton<IBase64Service, Base64Service>();
builder.Services.AddScoped<IGitHubAuth, GitHubAuth>();
builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EOSC", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// Break if we dont have EOSCDB
var connectionString = builder.Configuration.GetConnectionString("EOSCDB")!;
builder.Services.AddSingleton<IHistoryRepo>(new HistoryRepo());
builder.Services.AddSingleton<IHistoryService, HistoryService>();


var app = builder.Build();

app.UseMiddleware<RequestCompletedMiddleware>();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Headers.TryGetValue("bot", out var bot))
    {
        if (bot == _botAuth.GetBotToken())
        {
            await next();
            return;
        }
    }

    var path = ctx.Request.Path;
    if (path.Value != null && (path.Value.Contains("/login/oauth2/code/github") || path.Value.Contains("/api/login")))
    {
        // No auth on these endpoints
        await next();
        return;
    }

    // Check discord here as well 
    if (ctx.GetEndpoint()?.Metadata.GetMetadata<AnonAttribute>() != null)
    {
        // Endpoint allows anonymous access
        await next();
        return;
    }


    if (!ctx.User.Identity!.IsAuthenticated)
    {
        ctx.Response.StatusCode = 401;
        await ctx.Response.WriteAsync("Not authenticated");
        return;
    }

    var claim = ctx.User.Claims.ToList().Find(c => c.Type == "token");
    if (claim != null)
    {
        var claimValue = claim.Value;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + claimValue);
        // Why does this not tell us to use and agent :(
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Awesome-Octocat-App");
        var gitHubLogin = await httpClient.GetFromJsonAsync<GitHubLogin>("https://api.github.com/user");
        var name = gitHubLogin.Login;
        
    }

    await next.Invoke();
});


app.MapControllers();

app.Run();