using System.Xml.Linq;

namespace MCV_lab_1;

public class StudentXmlService
{
    private readonly string _filePath;

    public StudentXmlService(string filePath)
    {
        _filePath = filePath;
    }

    public void AddStudent(Student student)
    {
        XDocument doc;
        XElement root;

        if (File.Exists(_filePath))
        {
            doc = XDocument.Load(_filePath);
            root = doc.Element("Students");
        }
        else
        {
            doc = new XDocument();
            root = new XElement("Students");
            doc.Add(root);
        }

        root.Add(new XElement("Student",
            new XAttribute("StudentId", student.StudentId),
            new XElement("FirstName", student.FirstName),
            new XElement("LastName", student.LastName),
            new XElement("Major", student.Major)));

        doc.Save(_filePath);
    }

    public void RemoveStudent(string studentId)
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        XDocument doc = XDocument.Load(_filePath);
        XElement root = doc.Element("Students");

        var studentToRemove = root.Elements("Student")
            .FirstOrDefault(s => s.Attribute("StudentId").Value == studentId);

        if (studentToRemove != null)
        {
            studentToRemove.Remove();
            doc.Save(_filePath);
        }
        else
        {
            throw new InvalidOperationException("Student not found.");
        }
    }
}