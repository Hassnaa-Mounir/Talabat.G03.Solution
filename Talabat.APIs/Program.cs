using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;

using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Entities.Idintity;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Services;
using Talabat.RepositoryLayer;
using Talabat.RepositoryLayer.Data;
using Talabat.RepositoryLayer.Identity;
using Talabat.ServicesLayer.AuthService;

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

            /// builder.Services.AddEndpointsApiExplorer(); // to configure document of open api (swagger)
            /// builder.Services.AddSwaggerGen();  // to configure document of open api (swagger)
            builder.Services.AddSwaggerServices();
            builder.Services.AddDbContext<StoreContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // add dependency injection for dbcontext class and life time scoped

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();

            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();

            //make more generic

            ///  //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            ///  //builder.Services.AddAutoMapper(typeof(MappingProfile));
            ///
            ///  //builder.Services.Configure<ApiBehaviorOptions>(options =>
            ///  //{
            ///  //    options.InvalidModelStateResponseFactory = (actionContext =>
            ///  //    {
            ///  //        var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
            ///  //                                             .SelectMany(P => P.Value.Errors)
            ///  //                                             .Select(E => E.ErrorMessage)
            ///  //                                             .ToList();
            ///  //        var response = new ApiValidationErrorResponse()
            ///  //        {
            ///  //            Errors = errors
            ///  //        };
            ///  //        return new BadRequestObjectResult(response);
            ///  //    });
            ///  //});

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.addApplicationServices();
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>();

          // builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
          builder.Services.AddAuthServices(builder.Configuration);
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
            var _dbContext = services.GetRequiredService<StoreContext>();// ask clr for creating object from dbcontext explicitly 
            var _IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();// ask clr for creating object from dbcontext explicitly 
           
            // to make object from services logger factory(scopped) to helped me that catch exception
            var LoggerFactory =services.GetRequiredService<ILoggerFactory>();
            var logger = LoggerFactory.CreateLogger<Program>();

            try  {
                await _dbContext.Database.MigrateAsync();// update database 
                await StoreContextDataSeed.SeedAsync(_dbContext);
                await _IdentityDbContext.Database.MigrateAsync(); // update database
                var _userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityContextSeed.SeedUserAsync(_userManger);
                #region DataSeeding

                await StoreContextDataSeed.SeedAsync(_dbContext);

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex ,"An Error Occured During Appling Migration"); // if you have exception it will return in console
            }
            #endregion



            // Configure the HTTP request pipeline.

            #region  Configure Middlewares - (Container will configure pipeline)

            app.UseMiddleware<ExceptionMiddleware>();

            ///app.Use(async (httpContext, _next) =>
            ///{
            ///    try
            ///    {
            ///        // Take An Action With the request
            ///        await _next.Invoke(httpContext);
            ///        // Take An Action With the response
            ///    }
            ///    catch (Exception ex)
            ///    {
            ///        logger.LogError(ex.Message); // Development
            ///        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ///        httpContext.Response.ContentType = "application/json";
            ///        var response = app.Environment.IsDevelopment() ?
            ///            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
            ///            :
            ///            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
            ///        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            ///        var json = JsonSerializer.Serialize(response, options);
            ///        await httpContext.Response.WriteAsync(json);
            ///    }
            ///});

            if (app.Environment.IsDevelopment())
            {
                //request
                ///app.UseSwagger();
                ///app.UseSwaggerUI();
                app.UseSwaggerMiddleware();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers(); // to find route of each controller which match middleware


            #endregion

            app.Run();
        }

    }
}
