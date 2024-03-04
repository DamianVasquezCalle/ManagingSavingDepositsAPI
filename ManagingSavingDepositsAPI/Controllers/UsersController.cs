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
			if (size < 1) return new List<User>();
			if (size > 20) size = 20;

			var recordsToSkip = (page > 0 ? page - 1 : 0) * size;
			var users = _context.Users;
			var sortedUsers = sort == "ASC" ? users.OrderBy(x => x.Id).ToList() : users.OrderByDescending(x => x.Id).ToList();

			return sortedUsers.Skip(recordsToSkip).Take(size).ToList();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(User), 200)]
		public async Task<IActionResult> Get(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			return user == null ? NotFound() : Ok(user);
		}

		[HttpPost]
		[ProducesResponseType(typeof(User), 200)]
		public IActionResult Post([FromBody] User user)
		{
			var response = _context.Users.Add(user);
			_context.SaveChanges();
			return Ok(response.Entity);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(User), 200)]
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
			return Ok(user);
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
