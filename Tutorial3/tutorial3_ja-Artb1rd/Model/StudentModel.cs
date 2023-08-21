namespace Zadanie3.Model;

public class StudentModel
{
    public StudentModel()
    {
    }

    public StudentModel(string[] student)
    {
        FirstName = student[0];
        LastName = student[1];
        DirectionOfStudy = student[2];
        Mode = student[3];
        IndexNumber = student[4];
        DateOfBirth = (student[5]);
        Email = student[6];
        MothersName = student[7];
        FathersName = student[8];
    }

    public StudentModel(string firstName, string lastName, string directionOfStudy, string mode, string indexNumber, string dateOfBirth, string email, string mothersName, string fathersName)
    {
        FirstName = firstName;
        LastName = lastName;
        DirectionOfStudy = directionOfStudy;
        Mode = mode;
        IndexNumber = indexNumber;
        DateOfBirth = dateOfBirth;
        Email = email;
        MothersName = mothersName;
        FathersName = fathersName;
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DirectionOfStudy { get; set; }
    public string Mode { get; set; }
    public string IndexNumber { get; set; }
    public string DateOfBirth { get; set; }
    public string Email { get; set; }
    public string MothersName { get; set; }
    public string FathersName { get; set; }

    public string ToString()
    {
        return FirstName + "," + LastName + "," + DirectionOfStudy + "," + Mode + "," + IndexNumber + "," +
               DateOfBirth + "," + Email + "," + MothersName + "," + FathersName;
    }
}