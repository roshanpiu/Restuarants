using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using WebApplication.Services;
using WebApplication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace WebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            //defined configuration sources for the application
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Even though we didn't inject services to the services collection directly there are some services injected when app startups
        public void ConfigureServices(IServiceCollection services)
        {
            // AddTransient - when ever there is some method that needs IGreeter this will create a new instance and pass it in
            //services.AddTransient<IGreeter, Greeter>();

            // AddSingleton - creates a one instance and uses it for the entire life span of the application
            services.AddSingleton<IGreeter, Greeter>();

            // gives us a service provider and we will supply a service that the container has to store away
            // we can also use  the services supplied from provider to configure our service
            services.AddSingleton(provider => Configuration);

            //adds a unique instance of InMemoryRestaurantData to each http request
            services.AddScoped<IRestaurantData, SqlRestaurantData>();

            //adds ASP.NET MVC services to the container and 
            services.AddMvc();

            services.AddDbContext<WebApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["SqlServerLocalDb"]));

            //User is the your type of user entity
            //IdentityRole is the your type of role entity
            //AddEntityFrameworkStores configures services like user store which is used to 
            //create users and validate passwords
            //Give AddEntityFrameworkStores the IdentityDbContext derived class to that will be used to communicate wil identity schema in the database
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<WebApplicationDbContext>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Defined how application react to requests
        // IApplicationBuilder is a service that injected by default to the container
        public void Configure(IApplicationBuilder app, IGreeter greeter, IHostingEnvironment env)
        {
            //every middleware receives a http context which has Request and Response objects

            //UseWelcomePage is a terminal middleware
            //app.UseWelcomePage();
            app.UseDeveloperExceptionPage();

            if(env.IsDevelopment())
            {
                //If there is an exception from http pipeline this will display the exception info webpage
                app.UseDeveloperExceptionPage();

            }

            app.UseIdentity();

            // serves up the index.html in wwwroot
            // app.UseDefaultFiles(); //goes to directories if finds default files serves them
            // app.UseStaticFiles();
            // app.UseFileServer(); //includes UseDefaultFiles and UseStaticFiles in correct order

            //the MVC middleware will look at the in comming request and will try to map it to a C# Controller 
            app.UseMvc(ConfigureRoutes); 
            
            // app.Run is called a terminal middleware this will not have the chance to call another middleware
            app.Run(async (context) => {

                //throw new System.Exception("Error!");
                var greeting = greeter.GetGreeting();
                await context.Response.WriteAsync(greeting);
            });
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //This is called 'Convention based routing'

            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}"); //id is optional
            
            //default controller is Home default action method is index
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}"); //id is optional
        }
    }
}
