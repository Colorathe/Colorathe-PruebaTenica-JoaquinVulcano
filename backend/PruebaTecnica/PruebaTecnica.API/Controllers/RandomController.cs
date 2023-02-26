using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomController : ControllerBase
    {
        [HttpGet("number")]
        public IActionResult GetRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 37);
            return Ok(randomNumber);
        }

        [HttpGet("color")]
        public IActionResult GetRandomColor()
        {
            Random random = new Random();
            string[] colors = { "rojo", "negro" };
            string randomColor = colors[random.Next(0, colors.Length)];
            return Ok(randomColor);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUserAmount(User user)
        {
            using (var context = new RouletteGameContext(Configuration.GetConnectionString("DefaultConnection")))
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase));

                if (existingUser != null)
                {
                    existingUser.Amount += user.Amount;
                    context.Users.Update(existingUser);
                }
                else
                {
                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpGet("prize")]
        public IActionResult GetPrizeAmount(int number, string color, double amount)
        {
            Random random = new Random();
            int randomNum = random.Next(0, 37);
            string randomColor = (randomNum % 2 == 0) ? "rojo" : "negro";
            double prizeAmount = 0;

            if (randomNum == number && randomColor.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount * 3;
            }
            else if (randomColor.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount / 2;
            }
            else if (randomNum % 2 == 0 && color.Equals("rojo", StringComparison.OrdinalIgnoreCase) ||
                    randomNum % 2 == 1 && color.Equals("negro", StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount;
            }

            return Ok(prizeAmount);
        }
        [HttpGet("generate")]
        public IActionResult GenerateNumberAndColor(int number, string color, double amount)
        {
            Random random = new Random();
            int randomNum = random.Next(0, 37);
            string randomColor = (randomNum % 2 == 0) ? "rojo" : "negro";
            double prizeAmount = 0;

            if (randomNum == number && randomColor.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount * 2;
            }
            else if (randomColor.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount / 2;
            }
            else if (randomNum % 2 == 0 && color.Equals("rojo", StringComparison.OrdinalIgnoreCase) ||
                    randomNum % 2 == 1 && color.Equals("negro", StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = amount;
            }
            else
            {
                prizeAmount = -amount;
            }

            return Ok(new
            {
                Number = randomNum,
                Color = randomColor,
                PrizeAmount = prizeAmount
            });
        }

    }
}
