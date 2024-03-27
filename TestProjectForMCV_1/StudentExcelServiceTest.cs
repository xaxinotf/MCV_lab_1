using ClosedXML.Excel;
using MCV_lab_1;

namespace TestProjectForMCV_1;

[TestFixture]
public class StudentExcelServiceTests
{
    private string _filePath;
    private StudentExcelService _service;

    [OneTimeSetUp]
    public void SetupBeforeAllTests()
    {
        // Генеруємо унікальний шлях до тимчасового файлу для кожного прогону тестів
        _filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
        // Ініціалізація сервісу перед кожним тестом
        _service = new StudentExcelService(_filePath);
    }

    [Test]
    public void AddStudent_CreatesFileIfNotExists()
    {
        // Створення тестового студента
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };

        // Додавання студента
        _service.AddStudent(student);

        // Перевірка, що файл було створено
        Assert.IsTrue(File.Exists(_filePath));
    }
    [Test]
    public void AddStudent_AddsCorrectStudentId()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        AssertStudentAttribute("S001", 1);
    }
    [Test]
    public void AddStudent_FileIsNotEmptyAfterAddingStudent()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        var fileContent = File.ReadAllText(_filePath);
        Assert.IsNotEmpty(fileContent);
    }
    [Test]
    public void AddStudent_AddsCorrectFirstName()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        AssertStudentAttribute("John", 2);
    }
    [Test]
    public void AddStudent_AddsCorrectLastName()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        AssertStudentAttribute("Doe", 3);
    }

    [Test]
    public void AddStudent_AddsCorrectMajor()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);

        AssertStudentAttribute("Computer Science", 4);
    }

    private void AssertStudentAttribute(string expectedValue, int column)
    {
        using (var workbook = new XLWorkbook(_filePath))
        {
            var worksheet = workbook.Worksheet(1);
            var lastRow = worksheet.LastRowUsed();
            var actualValue = lastRow.Cell(column).Value;

            Assert.AreEqual(expectedValue, actualValue.ToString());
        }
    }
    
    [Test]
    public void RemoveStudent_RemovesSpecifiedStudent()
    {
        // Потрібно додати код для створення тестового файла з декількома студентами
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Computer Science" };
        _service.AddStudent(student);
        var studentIdToRemove = "S001";
        _service.RemoveStudent(studentIdToRemove);

        using (var workbook = new XLWorkbook(_filePath))
        {
            var worksheet = workbook.Worksheet(1);
            var row = worksheet.RowsUsed().FirstOrDefault(r => r.Cell(1).Value.ToString() == studentIdToRemove);

            // Перевірка, що студент був видалений
            Assert.IsNull(row);
        }
    }

    [Test]
    public void RemoveStudent_ThrowsExceptionWhenStudentNotFound()
    {
        // Спроба видалити неіснуючого студента
        var studentId = "NonExistentID";

        // Перевірка на виключення
       Assert.Throws<FileNotFoundException>(() => _service.RemoveStudent(studentId)); ;
    }

    [Test]
    [TestCase("S001", "John", "Doe", "Computer Science")]
    [TestCase("S002", "Jane", "Roe", "Mathematics")]
    [TestCase("S003", "Bill", "Smith", "Physics")]
    public void AddStudent_Parameterized_AddsStudentWithVariousDetails(string studentId, string firstName, string lastName, string major)
    {
        // Створення тестового студента з параметрами
        var student = new Student { StudentId = studentId, FirstName = firstName, LastName = lastName, Major = major };

        // Додавання студента
        _service.AddStudent(student);

        // Перевірка, що студент був доданий з правильними деталями
        using (var workbook = new XLWorkbook(_filePath))
        {
            var worksheet = workbook.Worksheet(1);
            var lastRow = worksheet.RowsUsed().Last();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(studentId, lastRow.Cell(1).Value.ToString(), "StudentId should match.");
                Assert.AreEqual(firstName, lastRow.Cell(2).Value.ToString(), "FirstName should match.");
                Assert.AreEqual(lastName, lastRow.Cell(3).Value.ToString(), "LastName should match.");
                Assert.AreEqual(major, lastRow.Cell(4).Value.ToString(), "Major should match.");
            });
        }
    }
    [Test]
    public void AddStudent_AddsWithCorrectDetails()
    {
        var student = new Student { StudentId = "S001", FirstName = "John", LastName = "Doe", Major = "Physics" };
        _service.AddStudent(student);

        using (var workbook = new XLWorkbook(_filePath))
        {
            var worksheet = workbook.Worksheet(1);
            var lastRow = worksheet.LastRowUsed();

            Assert.Multiple(() =>
            {
                Assert.That(lastRow.Cell(1).Value.ToString(), Is.EqualTo(student.StudentId), "StudentId does not match.");
                Assert.That(lastRow.Cell(2).Value.ToString(), Is.EqualTo(student.FirstName), "FirstName does not match.");
                Assert.That(lastRow.Cell(3).Value.ToString(), Is.EqualTo(student.LastName), "LastName does not match.");
                Assert.That(lastRow.Cell(4).Value.ToString(), Is.EqualTo(student.Major), "Major does not match.");
                Assert.That(workbook.Worksheets.Count, Is.Not.Zero, "The workbook should not be empty.");
            });
        }
    }
    [Test]
    public void RemoveStudent_RemovesStudentCorrectly()
    {
        // Додавання студента, щоб потім його видалити
        var student = new Student { StudentId = "S002", FirstName = "Alice", LastName = "Johnson", Major = "Mathematics" };
        _service.AddStudent(student);
    
        // Видалення студента
        _service.RemoveStudent(student.StudentId);

        using (var workbook = new XLWorkbook(_filePath))
        {
            var worksheet = workbook.Worksheet(1);
            var studentCells = worksheet.RowsUsed().SelectMany(row => row.CellsUsed()).Where(cell => cell.Value.ToString() == student.StudentId);

            Assert.That(studentCells, Is.Empty, "Student should have been removed.");
        }
    }
    
    [TearDown]
    public void CleanupAfterEachTest()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath); // Видаляємо файл після кожного тесту
        }
    }
}