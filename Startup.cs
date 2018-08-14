using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutofacMultipleImpTest.Controllers;
using AutofacMultipleImpTest.Logger;
using AutofacMultipleImpTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace AutofacMultipleImpTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Multiple Interface Implementations", Version = "v1" });
            });

            services.ConfigureSwaggerGen(c => { c.CustomSchemaIds(x => x.FullName); });

            var builder = new ContainerBuilder();

            builder.RegisterType<TestLog>().As<ISampleLog>();

            // do your injection configuration here
            builder.RegisterType<DefaultService>()
                .As<IService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultService>()
                .Named<IService>("ForDefaultController")
                .InstancePerLifetimeScope();

            //updated after comment -----------
            builder.RegisterType<DefaultController>()
                .WithParameter(ResolvedParameter.ForNamed<IService>("ForDefaultController"));
            // -------------------

            builder.RegisterType<CallService>()
                .Named<IService>("ForCallController")
                .InstancePerLifetimeScope();

            builder.RegisterType<CallController>()
                .WithParameter(ResolvedParameter.ForNamed<IService>("ForCallController"));

            builder.Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multiple Interface Implementations v1"); });
        }
    }
}