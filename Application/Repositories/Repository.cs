using CoreSystem;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await IncludeRelatedEntities(_dbSet).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await IncludeRelatedEntities(_dbSet).ToListAsync();
        }

        public async Task<(IEnumerable<T> Entities, int TotalCount)> GetAllWithFiltersAsync(string? searchText, int pageNumber, int pageSize)
        {
            var query = IncludeRelatedEntities(_dbSet);

            if (!string.IsNullOrEmpty(searchText))
            {
                query = ApplySearchFilter(query, searchText);
            }

            // Contar total antes da paginação
            int totalCount = await query.CountAsync();

            // Aplicar paginação
            var entities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (entities, totalCount);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                await UpdateAsync(entity);
            }
        }

        private IQueryable<T> IncludeRelatedEntities(IQueryable<T> query)
        {
            var entityType = typeof(T);
            var navigationProperties = entityType.GetProperties()
                .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string));

            foreach (var property in navigationProperties)
            {
                var navigation = _context.Model.FindEntityType(entityType)?
                    .GetNavigations()
                    .FirstOrDefault(n => n.Name == property.Name);

                if (navigation != null)
                {
                    query = query.Include(property.Name);
                }
            }

            return query;
        }


        private IQueryable<T> ApplySearchFilter(IQueryable<T> query, string searchText)
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string));

            foreach (var prop in properties)
            {
                query = query.Where(entity => EF.Functions.Like(EF.Property<string>(entity, prop.Name), $"%{searchText}%"));
            }

            return query;
        }
    }
}
