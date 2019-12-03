namespace OpenDentBusiness
{
    /// <summary>
    /// When a task is created or a comment made, a series of these taskunread objects are 
    /// created, one for each user who is subscribed to the tasklist. Duplicates are intelligently avoided. 
    /// Rows are deleted once user reads the task.
    /// </summary>
    public class TaskUnread
    {
        public long TaskUnreadNum;

        ///<summary>FK to task.TaskNum.</summary>
        public long TaskNum;

        ///<summary>FK to userod.UserNum.</summary>
        public long UserNum;
    }
}
