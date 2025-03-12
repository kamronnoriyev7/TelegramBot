using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Server;

namespace DataAccess
{
    public class Userconfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.ChatId);
            builder.Property(u => u.ChatId).IsRequired();
            builder.Property(u => u.Username).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.SelectedRegion).IsRequired();
            builder.Property(u => u.LastInteraction).IsRequired();

        }
    }
}
