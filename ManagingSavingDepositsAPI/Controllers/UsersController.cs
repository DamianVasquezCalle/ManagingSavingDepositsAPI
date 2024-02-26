using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagingSavingDepositsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		MSDAContext _context;

		public UsersController(MSDAContext context) {
			_context = context;
		}

		[HttpGet("size/{size}/page/{page}/sort/{sort}")]
		public IEnumerable<User> Get(int size, int page, string sort)
		{
			return _context.Users.ToList();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			return user == null ? NotFound() : Ok(user);
		}

		[HttpPost]
		public void Post([FromBody] User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] User user)
		{
			if (id != user.Id)
			{
				return BadRequest();
			}

			var initUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			if (initUser == null)
			{
				return NotFound();
			}

			_context.Entry(initUser).CurrentValues.SetValues(user);
			_context.SaveChanges();
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			if (user == null)
			{
				return NotFound();
			}
			_context.Users.Remove(user);
			_context.SaveChanges();
			return Ok();
		}
	}
}
