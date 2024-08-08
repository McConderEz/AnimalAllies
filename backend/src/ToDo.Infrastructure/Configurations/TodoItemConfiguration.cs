using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Constraints;
using ToDo.Domain.Models;

namespace ToDo.Infrastructure.Configurations;

public class TodoItemConfiguration: IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(Contraints.MAX_TITLE_LENGTH);

        builder.Property(x => x.CreationDate);
    }
}