var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();