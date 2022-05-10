using FluentValidation;
using FluentValidation.AspNetCore;
using HoangNV.HotelBooking.BusinessLogic;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Repository.Interface;
using HoangNV.HotelBooking.Web.Helper;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddSessionStateTempDataProvider();

            services.AddDbContext<BookingContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HoangNV.HotelBooking.Web"));
                    });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization(options =>
              {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                      factory.Create(typeof(SharedResource));
              }).AddRazorRuntimeCompilation().AddFluentValidation(options =>
              {
                  // Validate child properties and root collection elements
                  options.ImplicitlyValidateChildProperties = true;
                  options.ImplicitlyValidateRootCollectionElements = true;
                  // Automatic registration of validators in assembly
                  options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
              }); ;

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddValidatorsFromAssemblyContaining<ConvenientValidator>();
            services.AddValidatorsFromAssemblyContaining<BedValidator>();
            services.AddValidatorsFromAssemblyContaining<HotelBranchValidator>();
            services.AddValidatorsFromAssemblyContaining<RoomTypeUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<RoomTypeValidator>();
            services.AddValidatorsFromAssemblyContaining<RoomAddValidator>();
            services.AddValidatorsFromAssemblyContaining<BookingDetailValidator>();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddValidatorsFromAssemblyContaining<UserAddValidator>();
            services.AddValidatorsFromAssemblyContaining<UserUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<PassWordUpdateViewModel>();

            services.AddSingleton<ViewCommonFunction>();

            services.AddScoped<IConvenientTypeRepository, ConvenientTypeRepository>();
            services.AddScoped<IConvenientTypeBS, ConvenientTypeBS>();

            services.AddScoped<IConvenientRepository, ConvenientRepository>();
            services.AddScoped<IConvenientBS, ConvenientBS>();

            services.AddScoped<IBedRepository, BedRepository>();
            services.AddScoped<IBedBS, BedBS>();

            services.AddScoped<IHotelBranchRepository, HotelBranchRepository>();
            services.AddScoped<IHotelBranchBS, HotelBranchBS>();

            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserBS, UserBS>();

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomBS, RoomBS>();

            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IRoomTypeBS, RoomTypeBS>();

            services.AddScoped<IRoomBedRepository, RoomBedRepository>();
            services.AddScoped<IRoomConvenientRepository, RoomConvenientRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingBS, BookingBS>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerBS, CustomerBS>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleBS, RoleBS>();

            services.Configure<SmtpSettings>(Configuration.GetSection("Smtp"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IThrottlingEmail, ThrottlingEmail>();
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

            var supportedCultures = new List<CultureInfo>
            {
                   new CultureInfo("vn"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vn"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();  
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }
    }
}
