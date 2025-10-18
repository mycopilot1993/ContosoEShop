using System.Text;
using gitactionswithdemo.api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProductDbContext(builder.Configuration);
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddSeedData();

var app = builder.Build();

app.UseEnvironmentConfiguration();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoints();
app.MapProductEndpoints();
app.MapWeatherEndpoints();

app.Run();
