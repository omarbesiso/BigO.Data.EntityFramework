using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigO.Data.EntityFramework;

/// <summary>
///     Useful extensions to help with entity framework configuration.
/// </summary>
[PublicAPI]
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds a table to the model for the <typeparamref name="TEntity" /> type and configures it to be created as a SQL
    ///     table with specified properties.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be added to the model.</typeparam>
    /// <param name="builder">The builder for the entity type being added to the model.</param>
    /// <param name="tableName">The name of the table in the database.</param>
    /// <param name="tableSchema">The schema of the table in the database. Default value is "dbo".</param>
    /// <param name="description">A description of the table. Default value is <c>null</c>.</param>
    /// <param name="isTemporal">Indicates if the table should be created as a temporal table. Default value is false.</param>
    /// <param name="historyTableName">The name of the history table in the database. Default value is <c>null</c>.</param>
    /// <param name="historyTableSchema">The schema of the history table in the database. Default value is "History".</param>
    /// <param name="periodStartColumn">The name of the period start column. Default value is "ValidFrom".</param>
    /// <param name="periodEndColumn">The name of the period end column. Default value is "ValidTo".</param>
    /// <returns>The same <see cref="EntityTypeBuilder{TEntity}" /> instance so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="tableName" /> is null or white space.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="periodStartColumn" /> is null or white space.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="periodEndColumn" /> is null or white space.</exception>
    /// <remarks>
    ///     This method adds the <typeparamref name="TEntity" /> type to the model as a SQL table and configures its
    ///     properties.
    ///     If <paramref name="isTemporal" /> is set to true, the table is created as a temporal table with specified
    ///     <paramref name="historyTableName" />, <paramref name="historyTableSchema" />, <paramref name="periodStartColumn" />
    ///     ,
    ///     and <paramref name="periodEndColumn" />. If a <paramref name="description" /> is provided, it is added as a comment
    ///     to the table.
    /// </remarks>
    public static EntityTypeBuilder<TEntity> ToSqlTable<TEntity>(this EntityTypeBuilder<TEntity> builder,
        string tableName,
        string? tableSchema = "dbo",
        string? description = null,
        bool isTemporal = false,
        string? historyTableName = null,
        string? historyTableSchema = "History",
        string periodStartColumn = "ValidFrom",
        string periodEndColumn = "ValidTo") where TEntity : class
    {
        if (string.IsNullOrWhiteSpace(description) && isTemporal)
        {
            builder.ToTable(tableName, tableSchema);
        }
        else
        {
            builder.ToTable(tableName, tableSchema, tableBuilder =>
            {
                if (!string.IsNullOrWhiteSpace(description))
                {
                    tableBuilder.HasComment(description);
                }

                if (isTemporal)
                {
                    tableBuilder.Property<DateTime>(periodStartColumn).HasColumnName(periodStartColumn);
                    tableBuilder.Property<DateTime>(periodEndColumn).HasColumnName(periodEndColumn);

                    tableBuilder.IsTemporal(temporalTableBuilder =>
                    {
                        if (!string.IsNullOrWhiteSpace(historyTableName))
                        {
                            temporalTableBuilder.UseHistoryTable(historyTableName, historyTableSchema);
                        }

                        temporalTableBuilder.HasPeriodStart(periodStartColumn).HasColumnName(periodStartColumn);
                        temporalTableBuilder.HasPeriodEnd(periodEndColumn).HasColumnName(periodEndColumn);
                    });
                }
            });
        }

        return builder;
    }
}