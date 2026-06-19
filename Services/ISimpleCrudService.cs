namespace WebApplicationPractica.Services
{
    public interface ISimpleCrudService<TEntity, TDtoInput, TDtoOutput>
    {
        public Task<TDtoOutput?> getDtoById(int id);
        public Task<TEntity?> getEntityById(int id);
        public Task<TDtoOutput?> getByName(string name);
        public Task<IEnumerable<TDtoOutput>> getAll();
        public Task<TDtoOutput?> create(TDtoInput entity);
        public Task updateById(int id, TDtoInput dto);
        public Task deleteById(int id);
    }
}
