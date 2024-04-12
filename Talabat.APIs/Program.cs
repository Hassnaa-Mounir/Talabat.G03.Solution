using Microsoft.EntityFrameworkCore;
using Talabat.RepositoryLayer.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services - Create kestral 

            builder.Services.AddControllers(); //add services of api
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer(); // to configure document of open api (swagger)
            builder.Services.AddSwaggerGen();  // to configure document of open api (swagger)

            builder.Services.AddDbContext<StoreContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
            // add dependency injection for dbcontext class and life time scoped
            #endregion

            var app = builder.Build();

            #region Update-DataBase
            //StoreContext dbcontext = new StoreContext();// invalid
            //await dbcontext.Database.MigrateAsync();

            /// Ask CLR ToCreate object from dbcontext Explicitly
           using var Scope = app.Services.CreateScope();
            // Group of Services LifeTime Scoped
            var services =Scope.ServiceProvider;
            // you will  have service itself
            var dbcontext =services.GetRequiredService<StoreContext>(); 
            // you will have object from dbcontextClass by CLR Explicitly
            await dbcontext.Database.MigrateAsync();
             //update DataBase
            #endregion

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
