using autoberles_backend.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoberles_tests.Helpers
{
    public class FakeEmailService : EmailService
    {
        public FakeEmailService() : base(CreateFakeConfig())
        {
        }

        private static IConfiguration CreateFakeConfig()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Email:SmtpHost", "localhost"},
                    {"Email:SmtpPort", "25"},
                    {"Email:Username", "test"},
                    {"Email:Password", "test"},
                    {"Email:From", "test@test.com"}
                }).Build();
        }
        public override Task SendEmailAsync(string to, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
