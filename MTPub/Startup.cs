using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MTPub
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
            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ValueEnteredEventConsumer>();
                x.AddConsumer<NameEnteredEventConsumer>();

                //x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Message<IValueEntered>(x =>
                    {
                        x.SetEntityName("fu-value-entered");
                    });
                    cfg.Message<INameEntered>(x =>
                    {
                        x.SetEntityName("fu-value-entered");
                    });


                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("test");
                        h.Password("test");
                    });

                    //cfg.ReceiveEndpoint()


                    //cfg.ConfigureEndpoints(context);
                    //cfg.ReceiveEndpoint("event-listener", e =>
                    //{
                    //    e.Bind("f-o-o-b-a-r");
                    //    e.ConfigureConsumer<ValueEnteredEventConsumer>(context);
                    //});
                    cfg.ReceiveEndpoint("fu-value-entered", e =>
                    {
                        EndpointConvention.Map<IValueEntered>(e.InputAddress);
                        EndpointConvention.Map<INameEntered>(e.InputAddress);
                        //e.Bind("exchange-name");
                        e.Bind<IValueEntered>();
                        //e.DeadLetterExchange = "input-queue_skipped";
                        e.ConfigureConsumer<ValueEnteredEventConsumer>(context);
                        e.ConfigureConsumer<NameEnteredEventConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
