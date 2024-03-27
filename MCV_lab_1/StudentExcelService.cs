using ClosedXML.Excel;

namespace MCV_lab_1;

public class StudentExcelService
{
    private readonly string _filePath;

    public StudentExcelService(string filePath)
    {
        _filePath = filePath;
    }

    public void AddStudent(Student student)
    {
        var workbook = new XLWorkbook();
        IXLWorksheet worksheet;

        if (File.Exists(_filePath))
        {
            workbook = new XLWorkbook(_filePath);
            worksheet = workbook.Worksheet(1);
        }
        else
        {
            worksheet = workbook.Worksheets.Add("Students");
            worksheet.Cell(1, "A").Value = "StudentId";
            worksheet.Cell(1, "B").Value = "FirstName";
            worksheet.Cell(1, "C").Value = "LastName";
            worksheet.Cell(1, "D").Value = "Major";
        }

        var lastRow = worksheet.LastRowUsed().RowNumber() + 1;
        worksheet.Cell(lastRow, "A").Value = student.StudentId;
        worksheet.Cell(lastRow, "B").Value = student.FirstName;
        worksheet.Cell(lastRow, "C").Value = student.LastName;
        worksheet.Cell(lastRow, "D").Value = student.Major;

        workbook.SaveAs(_filePath);
    }

    public void RemoveStudent(string studentId)
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("The data file was not found.", _filePath);
        }

        var workbook = new XLWorkbook(_filePath);
        var worksheet = workbook.Worksheet(1);

        var row = worksheet.RowsUsed().FirstOrDefault(r => r.Cell(1).Value.ToString() == studentId);
        if (row != null)
        {
            row.Delete();
            workbook.Save();
        }
        else
        {
            throw new InvalidOperationException($"Student with ID {studentId} not found.");
        }
    }
}