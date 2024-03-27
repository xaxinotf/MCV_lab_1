// See https://aka.ms/new-console-template for more information

using MCV_lab_1;

var studentService = new StudentFileService("student.txt");
studentService.AddStudent(new Student
{
    FirstName = "AADA",
    LastName = "RWER",
    StudentId = "1",
    Major = "EWRE"
});

Console.WriteLine(studentService.GetAllStudents().First());