using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static BlazorServerTemplate.Data.Constants;

namespace BlazorServerTemplate.Data.Entities.Users;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string NameIdentifier { get; set; } = string.Empty;
    public UserSettings Settings { get; set; } = null!;
    public List<Permission> Permissions { get; set; } = null!;
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNameConstants.Users).OwnsOne(x => x.Settings).WithOwner();
    }
}
