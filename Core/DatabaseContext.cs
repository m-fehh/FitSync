using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definir o esquema para todas as entidades
            SetSchema(modelBuilder);

            // Configurar relacionamentos
            ConfigureRelationships(modelBuilder);

            // Configurar enums como string
            ConfigureEnumsAsString(modelBuilder);
        }

        private void SetSchema(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                var schemaName = "tb";

                // Definir o esquema e o nome da tabela
                entity.SetSchema(schemaName);
                entity.SetTableName(tableName);
            }
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        { }

        private void ConfigureEnumsAsString(ModelBuilder modelBuilder)
        { }
    }
}
