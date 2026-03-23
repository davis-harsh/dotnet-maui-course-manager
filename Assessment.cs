using SQLite;

namespace Milestone
{
    public class Assessment
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("assessmentid")]
        public int AssessmentId { get; set; }
        [Column("courseid")]
        public int CourseId { get; set; } // Foreign Key
        [Column("name")]
        public string Name { get; set; }
        [Column("startdate")]
        public DateTime StartDate { get; set; }
        [Column("enddate")]
        public DateTime EndDate { get; set; }
        [Column("type")]
        public AssessmentType Type { get; set; }
    }

    public enum AssessmentType
    {
        Performance,
        Objective
    }

    public static class AssessmentTypeValues
    {
        public static AssessmentType[] All => Enum.GetValues(typeof(AssessmentType))
                                                .Cast<AssessmentType>()
                                                .ToArray();
    }
}
