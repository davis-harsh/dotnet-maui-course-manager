using SQLite;

namespace Milestone
{
    [Table("course")]
    public class Course
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("courseid")]
        public int CourseId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("instructor")]
        public string Instructor { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("status")]
        public CourseStatus Status { get; set; }
        [Column("startDate")]
        public DateTime StartDate { get; set; }
        [Column("endDate")]
        public DateTime EndDate { get; set; }
        [Column("assessmentcount")]
        public int AssessmentCount { get; set; }
        [Column("summary")]
        public string Summary { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("cus")]
        public int CUs { get; set; }
        [Column("completedon")]
        public DateTime? CompletedOn { get; set; }
        [Column("termid")]
        public int TermId { get; set; } // Foreign Key

        public static Dictionary<int, string> TermNames { get; set; } = new()
        {
            { 1, "Term 1" },
            { 2, "Term 2" },
            { 3, "Term 3" },
            { 4, "Term 4" },
            { 5, "Term 5" },
            { 6, "Term 6" }
        };

        public List<string> CourseAssessments =>
            new List<string>
            {
                $"Performance Assessment: {Name}",
                $"Objective Assessment: {Name}"
            };
    }

    public enum CourseStatus
    {
        Inactive,
        Active,
        Dropped,
        Completed
    }

    public static class CourseStatusValues
    {
        public static CourseStatus[] All => Enum.GetValues(typeof(CourseStatus))
                                                .Cast<CourseStatus>()
                                                .ToArray();
    }
}