using System.Xml.Linq;
using MCV_lab_1;

namespace TestProjectForMCV_1;

[TestFixture]
public class StudentXmlServiceTests
{
    private string _filePath;
    private StudentXmlService _service;

    [OneTimeSetUp]
    public void SetupBeforeAllTests()
    {
        _filePath = Path.GetTempFileName();
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
        _service = new StudentXmlService(_filePath);
        // Створюємо чистий файл для кожного тесту
        XDocument doc = new XDocument(new XElement("Students"));
        doc.Save(_filePath);
    }

    [Test]
    public void AddStudent_CreatesXmlFileIfNotExists()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        Assert.IsTrue(File.Exists(_filePath));
    }
    
    [Test]
    public void AddStudent_AddsStudentDetailsToXmlFile()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var studentElement = doc.Descendants("Student").FirstOrDefault();

        Assert.IsNotNull(studentElement);
    }
    
    [Test]
    public void AddStudent_CorrectlyAddsStudentId()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var studentId = doc.Descendants("Student").FirstOrDefault()?.Attribute("StudentId")?.Value;

        Assert.AreEqual("S001", studentId);
    }

    [Test]
    public void AddStudent_CorrectlyAddsStudentFirstName()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var firstName = doc.Descendants("Student").FirstOrDefault()?.Element("FirstName")?.Value;

        Assert.AreEqual("John", firstName);
    }

    [Test]
    public void AddStudent_CorrectlyAddsStudentLastName()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var lastName = doc.Descendants("Student").FirstOrDefault()?.Element("LastName")?.Value;

        Assert.AreEqual("Doe", lastName);
    }

    [Test]
    public void AddStudent_CorrectlyAddsStudentMajor()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var major = doc.Descendants("Student").FirstOrDefault()?.Element("Major")?.Value;

        Assert.AreEqual("Computer Science", major);
    }

    [Test]
    public void RemoveStudent_RemovesCorrectStudentFromXmlFile()
    {
        var student = new Student { StudentId = "S002", FirstName = "Jane", LastName = "Doe", Major = "Math" };
        _service.AddStudent(student);
        _service.RemoveStudent("S002");

        XDocument doc = XDocument.Load(_filePath);
        var students = doc.Descendants("Student");

        Assert.AreEqual(0, students.Count());
    }

    [Test]
    public void AddStudent_DoesNotBreakWhenStudentIsNull()
    {
        Assert.Throws<NullReferenceException>(() => _service.AddStudent(null));
    }
    [Test]
    public void RemoveStudent_ThrowsException_WhenStudentNotFound()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => _service.RemoveStudent("NonExistentID"));
        Assert.AreEqual("Student not found.", ex.Message);
    }

    [TestCase("S003", "Alice", "Johnson", "Physics")]
    [TestCase("S004", "Bob", "Smith", "Chemistry")]
    public void AddStudent_Parameterized_AddsMultipleStudentsToXmlFile(string studentId, string firstName, string lastName, string major)
    {
        var student = new Student { StudentId = studentId, FirstName = firstName, LastName = lastName, Major = major };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var students = doc.Descendants("Student");

        Assert.IsTrue(students.Any(s => s.Attribute("StudentId")?.Value == studentId));
    }
    [Test]
    public void AddStudent_StudentHasExpectedAttributes()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        XDocument doc = XDocument.Load(_filePath);
        var resultStudent = doc.Descendants("Student").FirstOrDefault(s => s.Attribute("StudentId").Value == "S001");

        Assert.Multiple(() =>
        {
            Assert.That(resultStudent, Is.Not.Null, "Student should exist.");
            Assert.That(resultStudent.Attribute("StudentId").Value, Is.EqualTo("S001"), "StudentId should match.");
            Assert.That(resultStudent.Element("FirstName").Value, Is.EqualTo("John"), "FirstName should match.");
            Assert.That(resultStudent.Element("LastName").Value, Is.EqualTo("Doe"), "LastName should match.");
            Assert.That(resultStudent.Element("Major").Value, Is.EqualTo("Computer Science"), "Major should match.");
        });
    }
    [Test]
    public void AddMultipleStudents_StudentsHaveExpectedAttributes()
    {
        var student1 = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        var student2 = new Student { StudentId = "S002", FirstName = "Jane", LastName = "Roe", Major = "Mathematics" };
    
        _service.AddStudent(student1);
        _service.AddStudent(student2);

        XDocument doc = XDocument.Load(_filePath);
        var students = doc.Descendants("Student").ToList();

        Assert.AreEqual(2, students.Count, "There should be two students.");

        Assert.Multiple(() =>
        {
            var resultStudent1 = students.FirstOrDefault(s => s.Attribute("StudentId").Value == "S001");
            Assert.That(resultStudent1, Is.Not.Null, "Student1 should exist.");
            Assert.That(resultStudent1.Attribute("StudentId").Value, Is.EqualTo("S001"), "Student1 Id should match.");
            Assert.That(resultStudent1.Element("FirstName").Value, Is.EqualTo("John"), "Student1 FirstName should match.");
            Assert.That(resultStudent1.Element("LastName").Value, Is.EqualTo("Doe"), "Student1 LastName should match.");
            Assert.That(resultStudent1.Element("Major").Value, Is.EqualTo("Computer Science"), "Student1 Major should match.");

            var resultStudent2 = students.FirstOrDefault(s => s.Attribute("StudentId").Value == "S002");
            Assert.That(resultStudent2, Is.Not.Null, "Student2 should exist.");
            Assert.That(resultStudent2.Attribute("StudentId").Value, Is.EqualTo("S002"), "Student2 Id should match.");
            Assert.That(resultStudent2.Element("FirstName").Value, Is.EqualTo("Jane"), "Student2 FirstName should match.");
            Assert.That(resultStudent2.Element("LastName").Value, Is.EqualTo("Roe"), "Student2 LastName should match.");
            Assert.That(resultStudent2.Element("Major").Value, Is.EqualTo("Mathematics"), "Student2 Major should match.");
        });
    }
    [TearDown]
    public void CleanupAfterEachTest()
    {
        File.Delete(_filePath);
    }
}