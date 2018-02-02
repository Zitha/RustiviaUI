using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(p => p.Id).HasColumnOrder(0);

            Property(p => p.UserName)
                .IsRequired().HasMaxLength(20);

            Property(p => p.Password)
                .IsOptional().HasMaxLength(100);

            HasMany(a => a.Roles).WithMany(b => b.Users).Map(m =>
            {
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
                m.ToTable("webpages_UsersInRoles");
            });
        }
    }
}