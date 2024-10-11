namespace AzDevOps.NET.Model
{
    public class WorkItem
    {
        public WorkItemColumnsCollection Columns { get; set; } = new WorkItemColumnsCollection();
        public WorkItemRowCollection Rows { get; set; } = new WorkItemRowCollection();
        public int ColumnsCount => Columns.Count;
        public int RowsCount => Rows.Count;
    }
}
