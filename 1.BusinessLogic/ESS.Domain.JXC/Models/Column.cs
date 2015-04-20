namespace ESS.Domain.JXC.Models
{
    public class Column
    {
        public string ModuleNo { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public bool? IsInPrimaryKey { get; set; }
        public bool? IsInForeignKey { get; set; }
        public bool? IsNullable { get; set; }
        public bool? IsAutoKey { get; set; }
        public bool? IsTreeColumn { get; set; }
        public string SourceTableName { get; set; }
        public string SourceTableIDField { get; set; }
        public string SourceTableTextField { get; set; }
        public string SourceTableParentIDField { get; set; }
        public string Align { get; set; }
        public bool? InList { get; set; }
        public int? ListWidth { get; set; }
        public bool? InSearch { get; set; }
        public bool? Search_NewLine { get; set; }
        public bool? InForm { get; set; }
        public bool? NewLine { get; set; }
        public string Group { get; set; }
        public int? Index { get; set; }
    }
}