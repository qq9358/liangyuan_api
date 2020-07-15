using Egoal.Email.MailKit;
using Egoal.Invoice.GuangDongBaiWangJiuBin;
using Egoal.Localization;
using Egoal.Payment.Alipay;
using Egoal.Payment.IcbcPay;
using Egoal.Payment.SaobePay;
using Egoal.Payment.WeChatPay;
using Egoal.Redis;
using Egoal.ShortMessage.Huyi;
using Egoal.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Egoal.Web.Api
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
            services.AddKernelModule();
            services.AddBackgroundJob();
            services.AddAspNetCoreModule();
            services.AddEntityFrameworkCoreModule();
            services.AddDbContext(Configuration.GetConnectionString(AppConsts.ConnectionStringName));
            services.AddModelModule();
            services.AddApplicationModule(Configuration);
            services.AddDomainModule();
            services.AddRepositoryModule();

            services.AddWeChatModule(Configuration);
            services.AddWeChatPayModule();
            services.AddAlipayModule(Configuration);
            services.AddSaobePayModule(Configuration);
            services.AddIcbcPayModule(Configuration);

            services.AddHuyiShortMessage(Configuration);

            services.AddMailKitEmail(Configuration);

            services.AddGuangDongBaiWangJiuBinInvoice(Configuration);

            services.AddRedisLock(Configuration);

            services.AddLocalization(options => options.ResourcesPath = "Localization.Resources");
            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc(mvcOptions =>
            {
                mvcOptions.AddCustomFilters();
                mvcOptions.AddCustomModelBinders();
            })
            .AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            });
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v5.5", new Info { Title = "WebApi接口文档", Version = "v5.5" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.CustomSchemaIds((type) => type.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v5.5/swagger.json", "WebApi接口文档 v5.5");
            });

            app.UseAuthentication();

            //app.UseHttpsRedirection();

            app.UseCors(builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromDays(7));
            });

            app.UseNotification();

            app.UseStaticFiles();

            app.UseMvc();

            applicationLifetime.ApplicationStarted.Register(new Action<object>(OnStarted), app.ApplicationServices);
            applicationLifetime.ApplicationStopping.Register(new Action<object>(OnStopping), app.ApplicationServices);
        }

        private void OnStarted(object state)
        {
            var serviceProvider = state as IServiceProvider;

            ApplicationModule.Start(serviceProvider);
            RedisModule.Start(serviceProvider);
            WeChatModule.Start(serviceProvider);
        }

        private void OnStopping(object state)
        {
            var serviceProvider = state as IServiceProvider;

            RedisModule.Stop(serviceProvider);
        }
    }
}
