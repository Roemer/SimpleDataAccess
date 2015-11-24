namespace SimpleDataAccess.Definitions
{
    public enum TableHint
    {
        /// <summary>
        /// Reads only the commited data, usually with locking
        /// </summary>
        ReadCommited,
        /// <summary>
        /// Allowes reading dirty values, does not lock
        /// </summary>
        ReadUncommited,
        /// <summary>
        /// Reads only the commited data (on row level), does not lock
        /// </summary>
        ReadPast,
    }
}