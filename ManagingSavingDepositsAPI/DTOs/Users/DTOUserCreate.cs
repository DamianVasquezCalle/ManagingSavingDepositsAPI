namespace ManagingSavingDepositsAPI.DTOs.Users;

public class DTOUserCreate
{
	public string Name { get; set; }
	public string LastName { get; set; }
	public string Password { get; set; }
	public string Email { get; set; }
	public int UserRoleId { get; set; }
}
