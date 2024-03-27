namespace MCV_lab_1;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentId { get; set; }
    public string Major { get; set; }

    public override string ToString()
    {
        return $"{StudentId},{FirstName},{LastName},{Major}";
    }
}