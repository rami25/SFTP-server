namespace SFTPPortal.Domain.Enums;

public enum UserRole
{
    Admin,  // Can manage users + access all entities
    User    // Can only access their assigned entity
}