namespace SimpleDataAccess.Core
{
    public abstract class QueryHandlerBase<T> where T : DataEntityBase, new()
    {
        public EntityHandlerBase EntityHandler { get; private set; }

        protected QueryHandlerBase(EntityHandlerBase entityHandler)
        {
            EntityHandler = entityHandler;
        }

        public Query GetQuery()
        {
            return new Query();
        }
    }
}
