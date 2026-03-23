using System.Globalization;
using SQLite;

namespace Milestone
{
    public class LocalDbService
    {
        private const string DB_NAME = "local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService(string dbPath)
        {
            //ResetDatabaseAsync();
            //DeleteDatabase();

            var options = new SQLiteConnectionString(
                dbPath,
                storeDateTimeAsTicks: false,
                key: null,
                preKeyAction: null,
                postKeyAction: null,
                openFlags: SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite,
                vfsName: null);

            _connection = new SQLiteAsyncConnection(options);
            _connection.Trace = true;
            _connection.Tracer = s => Console.WriteLine("[SQL] " + s);

            InitializeTables();
        }

        public LocalDbService()
            : this(Path.Combine(FileSystem.AppDataDirectory, DB_NAME))
        {
        }

        private async Task InitializeTables()
        {
            _connection.CreateTableAsync<Student>();

            await _connection.CreateTableAsync<Term>();
            await _connection.CreateTableAsync<Course>();
            await _connection.CreateTableAsync<Note>();
            await _connection.CreateTableAsync<Assessment>();

        }

        public async Task ResetDatabaseAsync()
        {
            await _connection.DeleteAllAsync<Student>();
            await _connection.DeleteAllAsync<Course>();
            await _connection.DeleteAllAsync<Note>();
            await _connection.DeleteAllAsync<Term>();
            await _connection.DeleteAllAsync<Assessment>();
        }

        public static void DeleteDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "local_db.db3");
            if (File.Exists(dbPath))
                File.Delete(dbPath);
        }


        public async Task<int> CreateTerm(Term term)
        {
            if (term != null)
            {
                return await _connection.InsertAsync(term);
            }
            return 0;
        }

        public async Task<Course> GetCourseAsync(int courseId)
        {
            return await _connection.Table<Course>().FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<Student> GetStudentName()
        {
            return await _connection.Table<Student>().OrderByDescending(x => x.StudentId).FirstOrDefaultAsync();
        }

        public async Task<string> GetStudentNameById(int id)
        {
            var student = await _connection.Table<Student>()
                .FirstOrDefaultAsync(x => x.StudentId == id);

            return student?.Name ?? "Student not found";
        }
        public async Task<List<Student>> GetStudents()
        {
            return await _connection.Table<Student>().ToListAsync();
        }

        public async Task<List<Course>> GetCourses()
        {
            return await _connection.Table<Course>().ToListAsync();
        }

        public async Task<List<Course>> GetCompletedCourses()
        {
            return await _connection.Table<Course>()
                .Where(c => c.Status == CourseStatus.Completed && c.CompletedOn != null)
                .OrderByDescending(c => c.CompletedOn)
                .ToListAsync();
        }

        public async Task<List<Course>> GetActiveCourses()
        {
            return await _connection.Table<Course>()
                .Where(c => c.Status == CourseStatus.Active)
                .OrderByDescending(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Term>> GetTerms()
        {
            return await _connection.Table<Term>().ToListAsync();
        }
        public async Task<List<Assessment>> GetAssessments()
        {
            return await _connection.Table<Assessment>().ToListAsync();
        }

        public async Task<List<Note>> GetNotes(int courseId, bool includeHidden = false)
        {
            var query = _connection.Table<Note>().Where(n => n.CourseId == courseId);
            if (!includeHidden)
                query = query.Where(n => !n.IsHidden);

            return await query.ToListAsync();
        }

        public async Task<Student> GetByStudentUsername(string username)
        {
            return await _connection.Table<Student>().Where(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByStudentId(int studentId)
        {
            return await _connection.Table<Student>().Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
        }
        public async Task<Course> GetCourseById(int courseId)
        {
            return await _connection.Table<Course>().Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task<Term> GetTermById(int termId)
        {
            return await _connection.Table<Term>().Where(x => x.TermId == termId).FirstOrDefaultAsync();
        }

        public async Task<Course> GetCourseByName(string name)
        {
            return await _connection.Table<Course>().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task Create(Student student)
        {
            await _connection.InsertAsync(student);
        }

        public async Task Create(Assessment assessment)
        {
            await _connection.InsertAsync(assessment);
        }

        public async Task Create(Course course)
        {
            await _connection.InsertAsync(course);
        }

        public async Task Create(Term term)
        {
            await _connection.InsertAsync(term);
        }

        public async Task Update(Student student)
        {
            await _connection.UpdateAsync(student);
        }

        public async Task Update(Assessment assessment)
        {
            await _connection.UpdateAsync(assessment);
        }

        public async Task Update(Course course)
        {
            await _connection.UpdateAsync(course);
        }

        public async Task Update(Note note)
        {
            await _connection.UpdateAsync(note);
        }

        public async Task Update(Term term)
        {
            await _connection.UpdateAsync(term);
        }

        public async Task Delete(Student student)
        {
            await _connection.DeleteAsync(student);
        }
        public async Task Delete(Assessment assessment)
        {
            await _connection.DeleteAsync(assessment);
        }
        public async Task Delete(Note note)
        {
            await _connection.DeleteAsync(note);
        }
        public async Task Delete(Course course)
        {
            await _connection.DeleteAsync(course);
        }

        public async Task Delete(Term term)
        {
            await _connection.DeleteAsync(term);
        }

        public async Task<List<Course>> GetCoursesByTerm(int termId)
        {
            return await _connection.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public async Task<List<Assessment>> GetAssessmentsByCourseId(int courseId)
        {
            return await _connection.Table<Assessment>()
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetActiveCourses(string status)
        {
            return await _connection.Table<Course>().Where(c => c.Status == CourseStatus.Active).ToListAsync();
        }

        public async Task<int> AddNote(Note note)
        {
            return await _connection.InsertAsync(note);
        }

        public async Task<int> DeleteNote(int noteId)
        {
            return await _connection.DeleteAsync<Note>(noteId);
        }

        public async Task<int> AddAssessment(Assessment assessment)
        {
            return await _connection.InsertAsync(assessment);
        }



        public async Task<int> DeleteAssessment(int assessmentId)
        {
            return await _connection.DeleteAsync<Assessment>(assessmentId);
        }

        public async Task SeedCourseDatabase()
        {
            var existingCourses = await GetCourses();
            if (existingCourses.Count == 0)
            {
                var courses = new List<Course>
            {
                new Course
            {
                CourseId = 1,
                Name = "Intro to Programming",
                Instructor = "Anika Patel",
                Email = "anika.patel@strimeuniversity.edu",
                Phone = "555-123-4567",
                Status = CourseStatus.Active,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10),
                AssessmentCount = 2,
                Summary = "Learn beginner coding concepts.",
                Description = "This course introduces students to the foundational concepts of computer programming using a high-level language. Topics include variables, conditionals, loops, functions, and basic problem-solving strategies. Students will gain hands-on experience writing simple programs and developing logical thinking.",
                CUs = 3,
                TermId = 1
            },
                new Course
            {
                Name = "Database Systems",
                Instructor = "Emily Chen",
                Email = "emily.chen@wgu.edu",
                Phone = "555-234-5678",
                Status = CourseStatus.Active,
                AssessmentCount = 2,
                Summary = "Explore SQL and relational database design.",
                Description = "This course teaches core concepts in relational database design and query formulation using SQL. Students will learn data modeling, normalization, and transaction control. Practical exercises focus on creating, managing, and optimizing databases for real applications.",
                CUs = 4,
                TermId = 1
            },
            new Course
            {
                Name = "Discrete Mathematics",
                Instructor = "Caitlin Brown",
                Email = "caitlin.brown@wgu.edu",
                Phone = "555-345-6789",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Mathematical foundations for computer science.",
                Description = "Students will explore logic, set theory, combinatorics, and graph theory as foundational tools in computing. The course emphasizes proof techniques and algorithmic thinking. Concepts are applied to topics like network analysis and computational complexity.",
                CUs = 3,
                TermId = 1
            },
            new Course
            {
                Name = "Computer Architecture",
                Instructor = "Jason White",
                Email = "jason.white@wgu.edu",
                Phone = "555-456-7890",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Understand the inner workings of modern computers.",
                Description = "This course examines how hardware components like CPUs, memory, and buses interact to execute instructions. Students will study digital logic, instruction sets, and performance optimization. Labs may include designing simple processors or simulating instruction pipelines.",
                CUs = 4,
                TermId = 1
            },
            new Course
            {
                Name = "Cybersecurity Fundamentals",
                Instructor = "Sirius Black",
                Email = "sirius.black@wgu.edu",
                Phone = "555-567-8901",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Basics of security threats and countermeasures.",
                Description = "This course introduces students to common cybersecurity threats such as malware, phishing, and social engineering. It covers basic defensive techniques including firewalls, encryption, and access control. Students will also explore the ethical and legal aspects of cybersecurity.",
                CUs = 3,
                TermId = 1
            },
            new Course
            {
                Name = "Technical Writing",
                Instructor = "Ben Wyatt",
                Email = "ben.wyatt@pawnee.in.gov",
                Phone = "555-678-9012",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Improve your ability to communicate technical ideas clearly.",
                Description = "Students will learn to create professional documents such as user guides, manuals, and project proposals. The course emphasizes clarity, structure, and audience awareness. Real-world scenarios help students refine their editing and formatting skills.",
                CUs = 4,
                TermId = 1
            },
            new Course
            {
                Name = "Business Analytics",
                Instructor = "Sophia Martinez",
                Email = "sophia.martinez@strimeuniversity.edu",
                Phone = "555-789-0123",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Learn data-driven decision-making using statistical analysis and business intelligence tools.",
                Description = "This course introduces statistical methods and visualization techniques for interpreting business data. Students will use analytics platforms to create dashboards and support strategic decisions. Topics include forecasting, performance metrics, and customer behavior analysis.",
                CUs = 3,
                TermId = 2
            },
            new Course
            {
                Name = "Environmental Science",
                Instructor = "David Robinson",
                Email = "david.robinson@strimeuniversity.edu",
                Phone = "555-890-1234",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Explore ecological systems, climate change, and sustainable solutions to environmental challenges.",
                Description = "This course explores energy systems, pollution, and the impact of human activity on the environment. Students will evaluate sustainability initiatives and environmental policies. Fieldwork and case studies foster critical thinking about ecological stewardship.",
                CUs = 4,
                TermId = 2
            },
            new Course
            {
                Name = "Psychology of Learning",
                Instructor = "Lena Thompson",
                Email = "lena.thompson@strimeuniversity.edu",
                Phone = "555-901-2345",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Understand cognitive development, behavior patterns, and strategies for effective learning.",
                Description = "Students will explore how cognitive, emotional, and social factors influence the learning process. Topics include motivation, memory, reinforcement, and instructional design. Applications include creating supportive environments in education and workplace settings.",
                CUs = 3,
                TermId = 2
            },
            new Course
            {
                Name = "Digital Marketing",
                Instructor = "Alex Reed",
                Email = "alex.reed@strimeuniversity.edu",
                Phone = "555-012-3456",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Master SEO, social media, and content strategy to create engaging digital campaigns.",
                Description = "This course examines online consumer behavior and digital advertising strategies. Students will learn about SEO, analytics, email marketing, and social media management. The course emphasizes measuring campaign success and optimizing engagement.",
                CUs = 4,
                TermId = 2
            },
            new Course
            {
                Name = "Human Anatomy",
                Instructor = "Emily Sanders",
                Email = "emily.sanders@strimeuniversity.edu",
                Phone = "555-123-4567",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Study the structure and function of the human body, including key physiological processes.",
                Description = "Students will examine the systems of the human body, including muscular, skeletal, circulatory, and nervous systems. The course emphasizes anatomical terminology and spatial relationships. Labs and diagrams help build a strong foundation for health-related disciplines.",
                CUs = 3,
                TermId = 2
            },
            new Course
            {
                Name = "Music Theory",
                Instructor = "Ryan Carter",
                Email = "ryan.carter@strimeuniversity.edu",
                Phone = "555-234-5678",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Analyze musical composition, notation, and harmony to understand the fundamentals of music.",
                Description = "This course introduces scales, chords, intervals, and rhythm in Western music notation. Students will study musical form and structure while developing listening and composition skills. Exercises include writing and analyzing short musical pieces.",
                CUs = 4,
                TermId = 2
            },
            new Course {
                Name = "Algorithms",
                Instructor = "David Smith",
                Email = "david.smith@wgu.edu",
                Phone = "555-111-2222",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Introduction to algorithm design and analysis.",
                Description = "This course explores fundamental algorithm design strategies including divide-and-conquer, greedy techniques, and dynamic programming. Students will analyze time and space complexity using Big O notation. Emphasis is placed on solving computational problems efficiently through structured thinking.",
                CUs = 3,
                TermId = 3
            },

            new Course {
                Name = "Data Structures",
                Instructor = "Emily Johnson",
                Email = "emily.johnson@wgu.edu",
                Phone = "555-333-4444",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Understanding how data is organized and manipulated.",
                Description = "Learners will study key data structures such as arrays, linked lists, stacks, queues, trees, and hash tables. The course focuses on how these structures impact algorithm efficiency and application design. Hands-on programming reinforces concepts through practical examples and performance analysis.",
                CUs = 3,
                TermId = 3
            },

            new Course {
                Name = "Operating Systems",
                Instructor = "Michael Lee",
                Email = "michael.lee@wgu.edu",
                Phone = "555-555-6666",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Exploration of OS concepts and process management.",
                Description = "This course covers core operating system functionality such as process scheduling, memory management, and file systems. Students will explore concurrency, deadlock prevention, and system security. Labs may involve configuring or simulating components of modern operating systems.",
                CUs = 4,
                TermId = 3
            },

            new Course {
                Name = "Computer Networks",
                Instructor = "Rachel Green",
                Email = "rachel.green@wgu.edu",
                Phone = "555-777-8888",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Study of network protocols and communication models.",
                Description = "Students will learn how data moves between systems via layered network protocols like TCP/IP. The course introduces routing, switching, and wireless communication. Topics also include network troubleshooting, cybersecurity fundamentals, and real-world communication technologies.",
                CUs = 3,
                TermId = 3
            },

            new Course {
                Name = "Software Engineering",
                Instructor = "Jacob White",
                Email = "jacob.white@wgu.edu",
                Phone = "555-999-0000",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Principles and methodologies in software development.",
                Description = "This course introduces the software development lifecycle from requirements gathering to maintenance. Topics include design patterns, agile development, version control, and documentation. Learners will collaborate on team-based projects that simulate real-world engineering practices.",
                CUs = 4,
                TermId = 3
            },

            new Course {
                Name = "Database Systems",
                Instructor = "Laura Carter",
                Email = "laura.carter@wgu.edu",
                Phone = "555-222-3333",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Foundations of relational database management.",
                Description = "This course explores concepts behind relational databases, SQL querying, and normalization. Students will design schema, manage data integrity, and use transactions effectively. Practical exercises focus on building and interacting with real database systems for business use cases.",
                CUs = 3,
                TermId = 3
            },
            new Course {
                Name = "Cybersecurity Principles",
                Instructor = "Mark Thompson",
                Email = "mark.thompson@wgu.edu",
                Phone = "555-444-5555",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Fundamentals of security policies and defenses.",
                Description = "This course introduces core concepts in information security, including threat modeling and system vulnerabilities. Students will examine authentication, access control, encryption, and network defense strategies. Emphasis is placed on implementing security policies across organizations.",
                CUs = 3,
                TermId = 4
            },

            new Course {
                Name = "Cloud Computing",
                Instructor = "Anna Roberts",
                Email = "anna.roberts@wgu.edu",
                Phone = "555-666-7777",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Concepts of cloud services and architectures.",
                Description = "Learners will explore the fundamentals of cloud service models, including IaaS, PaaS, and SaaS. The course covers virtual machines, cloud storage, scalability, and deployment automation. Students will gain hands-on experience deploying and managing resources in public cloud platforms.",
                CUs = 3,
                TermId = 4
            },

            new Course {
                Name = "Machine Learning",
                Instructor = "James Walker",
                Email = "james.walker@wgu.edu",
                Phone = "555-888-9999",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Introduction to supervised and unsupervised learning.",
                Description = "This course introduces foundational machine learning algorithms including regression, classification, and clustering. Students will explore data preparation, model evaluation, and training techniques. Practical labs reinforce the application of models to real-world datasets using modern ML libraries.",
                CUs = 4,
                TermId = 4
            },

            new Course {
                Name = "Artificial Intelligence",
                Instructor = "Sarah Adams",
                Email = "sarah.adams@wgu.edu",
                Phone = "555-111-2222",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Study of AI techniques and applications.",
                Description = "Students will survey classic and modern AI methods such as rule-based systems, search algorithms, and neural networks. The course emphasizes real-world use cases in recommendation engines, robotics, and intelligent agents. Ethical implications and AI decision-making are also discussed.",
                CUs = 3,
                TermId = 4
            },

            new Course {
                Name = "Web Development",
                Instructor = "Chris Evans",
                Email = "chris.evans@wgu.edu",
                Phone = "555-333-4444",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Building responsive and dynamic web applications.",
                Description = "This course covers front-end and back-end web development with a focus on usability and performance. Students will learn HTML, CSS, JavaScript, and server-side scripting to build interactive applications. Topics also include RESTful APIs and deployment practices.",
                CUs = 3,
                TermId = 4
            },

            new Course {
                Name = "Mobile App Development",
                Instructor = "Olivia Wilson",
                Email = "olivia.wilson@wgu.edu",
                Phone = "555-555-6666",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Creating applications for mobile platforms.",
                Description = "Learners will design and implement mobile apps for Android and iOS platforms using cross-platform tools. The course explores touch interface design, sensor integration, and performance optimization. Students will publish functional prototypes and apply best practices in mobile UX.",
                CUs = 4,
                TermId = 4
            },
           new Course {
                Name = "Software Testing & QA",
                Instructor = "Brian Taylor",
                Email = "brian.taylor@wgu.edu",
                Phone = "555-777-8888",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Ensuring software quality through testing.",
                Description = "This course covers the fundamentals of software testing, including unit testing, integration testing, and system testing. Students will learn how to design test cases, write automated tests, and interpret test results. Emphasis is placed on quality assurance strategies in agile and continuous delivery environments.",
                CUs = 3,
                TermId = 5
            },

            new Course {
                Name = "Human-Computer Interaction",
                Instructor = "Megan Stewart",
                Email = "megan.stewart@wgu.edu",
                Phone = "555-999-0000",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Optimizing user experience in digital systems.",
                Description = "Students will explore the design of intuitive and accessible digital interfaces through usability principles and interaction models. The course focuses on user-centered design, prototyping, and evaluation methods. Learners will apply cognitive psychology and visual design to create effective user experiences.",
                CUs = 3,
                TermId = 5
            },

            new Course {
                Name = "Embedded Systems",
                Instructor = "Daniel Harris",
                Email = "daniel.harris@wgu.edu",
                Phone = "555-222-3333",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Programming and interfacing embedded devices.",
                Description = "This course introduces microcontroller programming and real-time operating principles used in embedded systems. Students will build and debug applications that interact with sensors, motors, and communication protocols. Emphasis is placed on hardware-software integration and resource-constrained design.",
                CUs = 4,
                TermId = 5
            },

            new Course {
                Name = "Parallel Computing",
                Instructor = "Jessica Brooks",
                Email = "jessica.brooks@wgu.edu",
                Phone = "555-111-2222",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "High-performance computing principles.",
                Description = "Learners will study computational techniques that divide work across multiple processors to improve performance. Topics include parallel architectures, data partitioning, and task synchronization. Practical labs will use multicore programming and GPU-based acceleration.",
                CUs = 3,
                TermId = 5
            },

            new Course {
                Name = "Blockchain Technology",
                Instructor = "Alex Martinez",
                Email = "alex.martinez@wgu.edu",
                Phone = "555-333-4444",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Cryptographic principles and decentralized systems.",
                Description = "This course explores the underlying cryptographic techniques that make decentralized ledgers secure and trustworthy. Students will examine blockchain architecture, smart contracts, and consensus protocols. Real-world applications such as cryptocurrency and supply chain tracing are discussed.",
                CUs = 3,
                TermId = 5
            },

            new Course {
                Name = "Game Development",
                Instructor = "Samuel Reed",
                Email = "samuel.reed@wgu.edu",
                Phone = "555-555-6666",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Design and development of interactive games.",
                Description = "Students will design and build playable games using industry-standard engines and scripting languages. The course covers game mechanics, level design, storytelling, and user engagement. Emphasis is placed on iteration, teamwork, and creative problem-solving.",
                CUs = 4,
                TermId = 5
            },

            new Course {
                Name = "Big Data Analytics",
                Instructor = "Thomas Hill",
                Email = "thomas.hill@wgu.edu",
                Phone = "555-777-8888",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Processing large-scale datasets for insights.",
                Description = "This course introduces techniques and tools used to process and analyze massive datasets. Students will learn about distributed computing, data mining, and real-time analytics. Emphasis is placed on turning complex data into actionable intelligence.",
                CUs = 3,
                TermId = 6
            },

            new Course {
                Name = "Robotics",
                Instructor = "Vanessa King",
                Email = "vanessa.king@wgu.edu",
                Phone = "555-999-0000",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Exploring autonomous and assistive robotic systems.",
                Description = "Students will explore robotic design, sensing, control, and decision-making systems. The course covers both theoretical frameworks and practical applications in automation and assistive technologies. Projects focus on developing small-scale working robots using sensors and logic programming.",
                CUs = 3,
                TermId = 6
            },

            new Course {
                Name = "Quantum Computing",
                Instructor = "Nathan Scott",
                Email = "nathan.scott@wgu.edu",
                Phone = "555-222-3333",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Introduction to quantum algorithms and computing.",
                Description = "This course provides a foundation in quantum mechanics as it relates to computation. Learners will investigate key quantum algorithms and their potential to solve complex problems faster than classical computers. The course emphasizes conceptual understanding through simulations and theoretical analysis.",
                CUs = 4,
                TermId = 6
            },

            new Course {
                Name = "Bioinformatics",
                Instructor = "Laura Mitchell",
                Email = "laura.mitchell@wgu.edu",
                Phone = "555-111-2222",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Computational approaches to biological data.",
                Description = "Students will learn how to apply algorithms and data science techniques to solve problems in genetics and molecular biology. The course covers sequence alignment, genome analysis, and protein structure prediction. Emphasis is placed on hands-on exploration using public bioinformatics tools and databases.",
                CUs = 3,
                TermId = 6
            },

            new Course {
                Name = "Computer Vision",
                Instructor = "Henry Parker",
                Email = "henry.parker@wgu.edu",
                Phone = "555-333-4444",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Image processing and recognition techniques.",
                Description = "This course explores how machines perceive and interpret images and videos. Students will learn image filtering, edge detection, object recognition, and machine learning techniques used in modern vision systems. Applications include facial recognition, augmented reality, and autonomous navigation.",
                CUs = 3,
                TermId = 6
            },

            new Course {
                Name = "Ethical Hacking",
                Instructor = "Sophia Gonzalez",
                Email = "sophia.gonzalez@wgu.edu",
                Phone = "555-555-6666",
                Status = CourseStatus.Inactive,
                AssessmentCount = 2,
                Summary = "Penetration testing and security evaluation.",
                Description = "This course introduces students to ethical hacking practices and methodologies. Topics include network scanning, vulnerability analysis, exploitation techniques, and reporting. Learners will use simulated environments to test and secure systems while adhering to ethical and legal standards.",
                CUs = 4,
                TermId = 6
            },
                    };

                foreach (var course in courses)
                {
                    await Create(course);
                }
            }
        }

        public async Task SeedStudentDatabase()
        {
            var existingStudents = await GetStudents();

            if (existingStudents.Count == 0)
            {
                var hash1 = SecurityHelper.HashPassword("ichibanINU<3", out var salt1);
                var hash2 = SecurityHelper.HashPassword("dHarsh123", out var salt2);
                var hash3 = SecurityHelper.HashPassword("test", out var salt3);

                var students = new List<Student>
        {
            new Student
            {
                StudentId = 1,
                Name = "Longmire Harsh",
                Email = "longmire.harsh@gmail.com",
                Phone = "509-101-2017",
                CompletedCUs = 60,
                Username = "longmire",
                PasswordHash = hash1,
                Salt = salt1
            },
            new Student
            {
                StudentId = 2,
                Name = "Davis Harsh",
                Email = "dharsh@wgu.edu",
                Phone = "206-643-0703",
                CompletedCUs = 112,
                Username = "dharsh",
                PasswordHash = hash2,
                Salt = salt2
            },
            new Student
            {
                StudentId = 3,
                Name = "Test Student",
                Email = "test.student@wgu.edu",
                Phone = "555-867-5309",
                CompletedCUs = 0,
                Username = "test",
                PasswordHash = hash3,
                Salt = salt3
            },
        };

                foreach (var student in students)
                {
                    await Create(student);
                }
            }
        }

        public async Task SeedTermDatabase()
        {
            var existingTerms = await GetTerms();
            if (existingTerms.Count == 0)
            {
                var terms = new List<Term>
                {
                    new Term
                    {
                        Name = "Term 1",
                        StartDate = DateTime.ParseExact("2025-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2025-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)

                    },
                    new Term
                    {
                        Name = "Term 2",
                        StartDate = DateTime.ParseExact("2025-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2026-01-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    },
                    new Term
                    {
                        Name = "Term 3",
                        StartDate = DateTime.ParseExact("2026-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2026-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    },
                    new Term
                    {
                        Name = "Term 4",
                        StartDate = DateTime.ParseExact("2026-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2027-01-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)

                    },
                    new Term
                    {
                        Name = "Term 5",
                        StartDate = DateTime.ParseExact("2027-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2027-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)

                    },
                    new Term
                    {
                        Name = "Term 6",
                        StartDate = DateTime.ParseExact("2027-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("2028-01-31", "yyyy-MM-dd", CultureInfo.InvariantCulture)

                    }

                };
                foreach (var term in terms)
                {
                    await Create(term);
                }
            }
        }
    }
}