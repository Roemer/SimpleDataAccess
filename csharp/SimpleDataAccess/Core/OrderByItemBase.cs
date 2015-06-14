namespace SimpleDataAccess.Core
{
    public abstract class OrderByItemBase
    {
        public abstract string Generate();

        public override string ToString()
        {
            return Generate();
        }
    }
}
