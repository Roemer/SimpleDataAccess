namespace SimpleDataAccess.Core
{
    public abstract class QueryHandlerBase<T> where T : DataEntityBase, new()
    {
        public EntityHandlerBase<T> EntityHandler { get; private set; }

        protected QueryHandlerBase(EntityHandlerBase<T> entityHandler)
        {
            EntityHandler = entityHandler;
        }

        public Query GetQuery()
        {
            return new Query();
        }
    }
}
