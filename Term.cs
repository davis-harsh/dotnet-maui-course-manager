using SQLite;

namespace Milestone
{
    [Table("term")]

    public class Term
    {
        [PrimaryKey, AutoIncrement]
        [Column("termid")]
        public int TermId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("startdate")]
        public DateTime StartDate { get; set; }
        [Column("enddate")]
        public DateTime EndDate { get; set; }
    }
}