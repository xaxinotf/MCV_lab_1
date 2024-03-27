namespace MCV_lab_1;

public class StudentFileService
{
    private readonly string _filePath;

    public StudentFileService(string filePath)
    {
        _filePath = filePath;
    }

    public void AddStudent(Student student)
    {
        var exists = File.Exists(_filePath);
        File.AppendAllText(_filePath, student.ToString() + Environment.NewLine);
    }

    public void RemoveStudent(string studentId)
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("The data file was not found.", _filePath);
        }

        var lines = File.ReadAllLines(_filePath);
        var updatedLines = lines.Where(line => !line.StartsWith(studentId + ",")).ToList();

        if (lines.Length == updatedLines.Count)
        {
            throw new InvalidOperationException($"Student with ID {studentId} not found.");
        }

        File.WriteAllLines(_filePath, updatedLines);
    }

    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();
        if (File.Exists(_filePath))
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    students.Add(new Student { StudentId = parts[0], FirstName = parts[1], LastName = parts[2], Major = parts[3] });
                }
            }
        }
        return students;
    }
}