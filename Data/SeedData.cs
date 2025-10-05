using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JokeWebApp.Models;

namespace JokeWebApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            if (await context.Joke.AnyAsync())
            {
                return;
            }

            var jokes = new List<Joke>
            {
                new Joke
                {
                    JokeQuestion = "Why do programmers prefer dark mode?",
                    JokeAnswer = "Because light attracts bugs!"
                },
                new Joke
                {
                    JokeQuestion = "How many programmers does it take to change a light bulb?",
                    JokeAnswer = "None, that's a hardware problem."
                },
                new Joke
                {
                    JokeQuestion = "What's a programmer's favorite place to hang out?",
                    JokeAnswer = "The Foo Bar."
                }
            };

            await context.Joke.AddRangeAsync(jokes);
            await context.SaveChangesAsync();
        }
    }
}
