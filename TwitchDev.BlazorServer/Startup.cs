using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchDev.BlazorServer.Data;
using TwitchDev.DataStorage;
using TwitchDev.DataStorage.Configuration;
using TwitchDev.DataStorage.Interfaces;
using TwitchDev.TwitchBot;
using TwitchDev.TwitchBot.Configuration;
using TwitchDev.TwitchBot.Interfaces;

namespace TwitchDev.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.Configure<RedisConfiguration>(Configuration.GetSection("Redis"));
            services.Configure<TwitchConfiguration>(Configuration.GetSection("Twitch"));
            services.Configure<TwitchBotConfiguration>(Configuration.GetSection("TwitchBot"));

            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<ITwitchService, TwitchService>();

            services.AddHostedService<TwitchBotService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
