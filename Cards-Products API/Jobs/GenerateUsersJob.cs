using Bogus;
using Cards_Products_API.Data;
using Cards_Products_API.Services;
using Quartz;

namespace Cards_Products_API.Jobs
{
    public class GenerateUsersJob : IJob
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;

        public GenerateUsersJob(AppDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var user = await _userService.CreateRandomUser();
                Console.WriteLine($"Usuario creado: {user.First_Name} {user.Last_Name} - {user.Email}");
            }
        }
    }
}
