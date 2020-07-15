using Egoal.Auditing;
using Egoal.Authorization;
using Egoal.Models;
using Egoal.Mvc.Auditing;
using Egoal.Mvc.Authorization;
using Egoal.Mvc.ExceptionHandling;
using Egoal.Mvc.ModelBinding;
using Egoal.Mvc.Results;
using Egoal.Mvc.Results.Wrapping;
using Egoal.Mvc.Uow;
using Egoal.Mvc.Validation;
using Egoal.Notifications;
using Egoal.Runtime.Session;
using Egoal.SignalR.Hubs;
using Egoal.SignalR.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Egoal
{
    public static class AspNetCoreModule
    {
        public static void AddAspNetCoreModule(this IServiceCollection services)
        {
            //See https://github.com/aspnet/Mvc/issues/3936 to know why we added these services.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IPrincipalAccessor, AspNetCorePrincipalAccessor>();

            services.AddTransient<ITokenService, JwtTokenService>();
            services.AddSingleton<IErrorInfoBuilder, ErrorInfoBuilder>();
            services.AddTransient<IActionResultWrapperFactory, ActionResultWrapperFactory>();

            AddJwt(services);

            services.AddTransient<IAuthorizationHandler, PermissionHandler>();
            services.AddTransient<UowActionFilter>();
            services.AddTransient<ExceptionFilter>();
            services.AddTransient<ValidationResultFilter>();
            services.AddTransient<ResultFilter>();

            services.AddTransient<IRealTimeNotifier, SignalRRealTimeNotifier>();

            services.AddTransient<IClientInfoProvider, HttpContextClientInfoProvider>();
        }

        private static void AddJwt(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var tokenOptions = new TokenOptions();
                options.TokenValidationParameters.ValidAudience = tokenOptions.ValidAudience;
                options.TokenValidationParameters.ValidIssuer = tokenOptions.ValidIssuer;
                options.TokenValidationParameters.IssuerSigningKey = tokenOptions.SecurityKey;
            });
        }

        public static void AddCustomFilters(this MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));

            options.Filters.AddService(typeof(UowActionFilter));
            options.Filters.AddService(typeof(ExceptionFilter));
            options.Filters.AddService(typeof(ValidationResultFilter));
            options.Filters.AddService(typeof(ResultFilter));
        }

        public static void AddCustomModelBinders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new StringModelBinderProvider());
        }

        public static void UseNotification(this IApplicationBuilder app)
        {
            app.UseSignalR(route =>
            {
                route.MapHub<NotificationHub>("/notification");
            });
        }
    }
}
