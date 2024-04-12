namespace Talabat.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services - Create kestral 

            builder.Services.AddControllers(); //add services of api
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer(); // to configure document of open api (swagger)
            builder.Services.AddSwaggerGen();  // to configure document of open api (swagger)
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            #region  Configure Middlewares - (Container will configure pipeline)
            if (app.Environment.IsDevelopment())
            {
                // request
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); // to find route of each controller which match middleware


            #endregion

            app.Run();
        }

    }
}
