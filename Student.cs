using SQLite;

namespace Milestone
{
    [Table("student")]

    public class Student
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("studentid")]
        public int StudentId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("completed_cus")]
        public int CompletedCUs { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("salt")]
        public byte[] Salt { get; set; }

    }

}
