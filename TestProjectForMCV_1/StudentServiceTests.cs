using MCV_lab_1;

namespace TestProjectForMCV_1;

[TestFixture]
public class StudentServiceTests
{
     private string _filePath;
        private StudentFileService _service;

        // Аналог @BeforeClass в jUnit
        [OneTimeSetUp]
        public void SetupBeforeAllTests()
        {
            _filePath = Path.GetTempFileName();
            _service = new StudentFileService(_filePath);
        }

        // Аналог @Before в jUnit
        [SetUp]
        public void SetupBeforeEachTest()
        {
            // Перед кожним тестом очищаємо файл
            File.WriteAllText(_filePath, string.Empty);
        }

        [Test]
        public void AddStudent_WritesStudentToFile()
        {
            var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };

            _service.AddStudent(student);

            var content = File.ReadAllText(_filePath);
            Assert.That(content, Does.Contain(student.ToString()));
        }

        [Test]
        public void GetAllStudents_StudentsHaveCorrectStructure()
        {
            // Додавання студентів
            _service.AddStudent(new Student { StudentId = "S006", FirstName = "George", LastName = "Wilson", Major = "Chemistry" });
            _service.AddStudent(new Student { StudentId = "S007", FirstName = "Helen", LastName = "Clark", Major = "Biology" });

            var students = _service.GetAllStudents();

            // Перевірка структури кожного студента
            Assert.True(students.All(s => s.GetType().GetProperty("StudentId") != null), "All students should have 'StudentId' property.");
            Assert.True(students.All(s => s.GetType().GetProperty("FirstName") != null), "All students should have 'FirstName' property.");
            Assert.True(students.All(s => s.GetType().GetProperty("LastName") != null), "All students should have 'LastName' property.");
            Assert.True(students.All(s => s.GetType().GetProperty("Major") != null), "All students should have 'Major' property.");
        }
        
        [Test]
        public void GetAllStudents_ReturnsListOfAddedStudents()
        {
            var student1 = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
            var student2 = new Student { StudentId = "S002", FirstName = "Jane", LastName = "Doe", Major = "Math" };
            _service.AddStudent(student1);
            _service.AddStudent(student2);

            var students = _service.GetAllStudents();

            Assert.That(students, Has.Count.EqualTo(2));
            Assert.That(students.Select(s => s.StudentId), Is.EquivalentTo(new[] { "S001", "S002" }));
        }

        [Test]
        public void RemoveStudent_RemovesSpecifiedStudentFromFile()
        {
            var student = new Student
                { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
            _service.AddStudent(student);
            _service.RemoveStudent("S001");

            var content = File.ReadAllText(_filePath);
            Assert.That(content, Is.Empty);
        }

        [Test]
        public void GetAllStudents_WithNoStudents_ReturnsEmptyList()
        {
            var students = _service.GetAllStudents();

            Assert.That(students, Is.Empty);
        }
        
        [TestCase("S001", "John", "Doe", "Computer Science")]
        [TestCase("S002", "Jane", "Roe", "Mathematics")]
        public void AddStudent_WritesMultipleStudentsToFile_Parameterized(string id, string firstName, string lastName, string major)
        {
            var student = new Student { StudentId = id, FirstName = firstName, LastName = lastName, Major = major };

            _service.AddStudent(student);

            var content = File.ReadAllText(_filePath);
            Assert.That(content, Does.Contain($"{id},{firstName},{lastName},{major}"));
        }

        [Test]
        public void GetAllStudents_ContainsStudentWithSpecificAttributes()
        {
            var student = new Student { StudentId = "S003", FirstName = "Alice", LastName = "Johnson", Major = "Biology" };
            _service.AddStudent(student);

            var students = _service.GetAllStudents();

            Assert.That(students, Has.Exactly(1)
                .Matches<Student>(s => s.StudentId == "S003" && s.FirstName == "Alice" && s.LastName == "Johnson" && s.Major == "Biology"));
        }
        
        [Test]
        public void GetAllStudents_ReturnsAllStudentsRegardlessOfOrder()
        {
            var student1 = new Student { StudentId = "S004", FirstName = "Bob", LastName = "Smith", Major = "Physics" };
            var student2 = new Student { StudentId = "S005", FirstName = "Claire", LastName = "Adams", Major = "Chemistry" };
            _service.AddStudent(student1);
            _service.AddStudent(student2);

            var expectedStudents = new List<Student>
            {
                new Student { StudentId = "S004", FirstName = "Bob", LastName = "Smith", Major = "Physics" },
                new Student { StudentId = "S005", FirstName = "Claire", LastName = "Adams", Major = "Chemistry" }
            };

            var actualStudents = _service.GetAllStudents();

            Assert.That(actualStudents, Is.EquivalentTo(expectedStudents)
                .Using<Student>((s1, s2) => s1.StudentId == s2.StudentId && s1.FirstName == s2.FirstName && s1.LastName == s2.LastName && s1.Major == s2.Major ? 0 : 1));
        }
        
        [Test]
        public void RemoveStudent_ThrowsException_WhenStudentNotFound()
        {
            // Спроба видалити неіснуючого студента
            var studentId = "NonExistentID";

            // Перевірка на кидання виключення
            var ex = Assert.Throws<InvalidOperationException>(() => _service.RemoveStudent(studentId));
            Assert.That(ex.Message, Is.EqualTo($"Student with ID {studentId} not found."));
        }
        
        // Аналог @AfterClass в jUnit
        [OneTimeTearDown]
        public void CleanupAfterAllTests()
        {
            File.Delete(_filePath);
        }
}