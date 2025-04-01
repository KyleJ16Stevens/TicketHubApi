var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

//Map attribute-routed controllers (like PurchaseController)
app.MapControllers();

app.Run();