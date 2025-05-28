using Microsoft.EntityFrameworkCore;
using PatientsEFCF.Data;
using PatientsEFCF.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

app.Run();