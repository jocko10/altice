using AlticeApi.BusinessObjects;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserBusinessObject,UserBusinessObject>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().Count().OrderBy().Expand());
builder.Services.AddSwaggerGen();


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
