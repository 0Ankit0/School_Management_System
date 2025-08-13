using System;

namespace SMS.Contracts.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}