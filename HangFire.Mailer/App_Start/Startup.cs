using Hangfire;
using Microsoft.Owin;
using Owin;

// Configure Owin configuration entry point
[assembly: OwinStartup(typeof(HangFire.Mailer.App_Start.Startup), "Configure")]

namespace HangFire.Mailer.App_Start
{
    public class Startup
    {
        public void Configure(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("MailerDb");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}