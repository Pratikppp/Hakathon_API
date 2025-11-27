using Hackathon_API.Models;
using Hackathon_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<WeatherForecastServices>();
builder.Services.AddHttpClient<AeroDataBoxService   >();
builder.Services.AddScoped<HealthRiskService>();
builder.Services.AddScoped<SafetyService>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<AeroDataBoxService>();
builder.Services.AddScoped<WeatherForecastServices>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReact");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
