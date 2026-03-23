using SQLite;

namespace Milestone
{
    public class Note
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("noteid")]
        public int NoteId { get; set; }
        [Column("courseid")]

        public int CourseId { get; set; } // Foreign Key
        [Column("content")]
        public string Content { get; set; }
        [Column("ishidden")]
        public bool IsHidden { get; set; }
    }

}
