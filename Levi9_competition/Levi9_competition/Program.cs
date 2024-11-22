using Amazon.DynamoDBv2;
using Levi9_competition.Data;
using Levi9_competition.Interfaces;
using Levi9_competition.Repos;
using Levi9_competition.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
    new AmazonDynamoDBClient(
        "key",
        "secret",
        Amazon.RegionEndpoint.EUCentral1));

builder.Services.AddDbContext<AppDbContext>(
        options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("MyLeviDb"))
);


builder.Services.AddScoped<IPlayerRepo, PlayerRepo>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<ITeamRepo, TeamRepo>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<IMatchRepo, MatchRepo>();
builder.Services.AddScoped<MatchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
