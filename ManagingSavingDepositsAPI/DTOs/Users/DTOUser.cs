namespace ManagingSavingDepositsAPI.DTOs.Users;

public class DTOUser
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public int UserRoleId { get; set; }
	public string UserRoleName { get; set; }
}
