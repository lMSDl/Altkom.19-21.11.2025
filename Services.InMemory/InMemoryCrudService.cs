using Services.Interfaces;

namespace Services.InMemory
{
    public class InMemoryCrudService<T> : ICrudService<T> where T : Models.Entity
    {
        private ICollection<T> _entities;

        public InMemoryCrudService() : this(new List<T>())
        {
        }
        protected InMemoryCrudService(ICollection<T> entites)
        {
            _entities = entites;
        }

        public Task<int> CreateAsync(T entity)
        {
            entity.Id = _entities.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _entities.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task DeleteAsync(int id)
        {
            var entity = _entities.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(_entities.ToArray().AsEnumerable());
        }

        public Task<T?> ReadAsync(int id)
        {
            var entity = _entities.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(int id, T entity)
        {
            var existingEntity = _entities.FirstOrDefault(x => x.Id == id);
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
                entity.Id = id;
                _entities.Add(entity);
            }
            return Task.CompletedTask;
        }
    }
}
