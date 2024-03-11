using AutoMapper;
using DB;
using DB.DomainObjects;
using ManagingSavingDepositsAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagingSavingDepositsAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		MSDAContext _context;
		IMapper _mapper;

		public UsersController(MSDAContext context, IMapper mapper) {
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("size/{size}/page/{page}/sort/{sort}")]
		public IEnumerable<DTOUser> Get(int size, int page, string sort)
		{
			if (size < 1) return new List<DTOUser>();
			if (size > 20) size = 20;
			var recordsToSkip = (page > 0 ? page - 1 : 0) * size;

			var users = _context.Users.Include(x => x.UserRole);
			var sortedUsers = sort.ToUpper() == "ASC" ? users.OrderBy(x => x.Id).ToList() : users.OrderByDescending(x => x.Id).ToList();

			return sortedUsers.Skip(recordsToSkip).Take(size).Select(_mapper.Map<DTOUser>);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(DTOUser), 200)]
		public async Task<IActionResult> Get(int id)
		{
			var user = await _context.Users
				.Include(x => x.UserRole)
				.FirstOrDefaultAsync(x => x.Id == id);
			return user == null ? NotFound() : Ok(_mapper.Map<DTOUser>(user));
		}

		[HttpPost]
		[ProducesResponseType(typeof(DTOUser), 200)]
		public IActionResult Post([FromBody] DTOUserCreate userCreate)
		{
			var userRole = _context.UserRoles.FirstOrDefault(x => x.Id == userCreate.UserRoleId);
			if (userRole == null) return BadRequest("Invalid User Role");

			var user = _mapper.Map<User>(userCreate);
			var response = _context.Users.Add(user);
			_context.SaveChanges();

			var newUser = response.Entity;
			newUser.UserRole = userRole;
			return Ok(_mapper.Map<DTOUser>(newUser));
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(DTOUser), 200)]
		public async Task<IActionResult> Put(int id, [FromBody] DTOUserCreate userUpdate)
		{
			var userRole = _context.UserRoles.FirstOrDefault(x => x.Id == userUpdate.UserRoleId);
			if (userRole == null) return BadRequest("Invalid User Role");

			var initUser = await _context.Users
				.FirstOrDefaultAsync(x => x.Id == id);
			if (initUser == null) return NotFound();

			var user = _mapper.Map<User>(userUpdate);
			user.Id = id;
			_context.Entry(initUser).CurrentValues.SetValues(user);
			_context.Entry(initUser).Property(x => x.Password).IsModified = false;
			_context.SaveChanges();

			userRole.Users.Clear();
			user.UserRole = userRole;
			return Ok(_mapper.Map<DTOUser>(user));
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
