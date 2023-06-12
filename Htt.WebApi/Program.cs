using Htt.WebApi.Abstract;
using Htt.WebApi.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 



var connectionString = builder.Configuration.GetConnectionString("HttDbConnectionString");
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>(provider => new CategoriesRepository(connectionString!));
builder.Services.AddScoped<IProductsRepository, ProductsRepository>(provider => new ProductsRepository(connectionString!));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
