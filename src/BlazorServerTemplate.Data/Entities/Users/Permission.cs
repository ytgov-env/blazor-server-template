using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static BlazorServerTemplate.Data.Constants;

namespace BlazorServerTemplate.Data.Entities.Users;

public class Permission
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public List<User> Users { get; set; } = null!;
}

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNameConstants.Permissions);
    }
}
