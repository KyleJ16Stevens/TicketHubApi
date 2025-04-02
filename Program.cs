var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}



app.UseHttpsRedirection();

app.UseAuthorization();

//Map attribute-routed controllers (like PurchaseController)
app.MapControllers();

app.Run();