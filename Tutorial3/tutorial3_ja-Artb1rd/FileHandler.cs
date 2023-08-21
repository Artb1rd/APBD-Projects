using Microsoft.Extensions.Logging.Console;
using Zadanie3.Model;

namespace Zadanie3;

public class FileHandler
{   
    private static readonly string FILE_PATH = "C:\\APBD\\tutorial3_ja-Artb1rd\\Data\\data.csv";
    public static List<StudentModel> Read()
    {
        return File.ReadLines(FILE_PATH).Select(line => line.Split(",")).Skip(1).Where(line => line[0] != "string")
            .Select(
                simpleData => new StudentModel(simpleData)
            ).ToList();
    }

    public static StudentModel? ReadByIndex(string index) 
    {
        return Read().Find(line => line.IndexNumber == index);
    }

    public static RequestStatus Insert(StudentModel student)
    {
        var isStudentExists = Read().Exists(single => student.IndexNumber == single.IndexNumber);
        if (isStudentExists) return RequestStatus.ERROR_EXISTS;
        if (ValidatorUtil.isStringValid(student.FirstName, student.LastName, 
                student.FathersName, student.MothersName,
                student.Mode, student.DirectionOfStudy)
            && ValidatorUtil.isEmailValid(student.Email)
            && ValidatorUtil.isIndexValid(student.IndexNumber))
        {
            File.AppendAllText(FILE_PATH,"\n"+student.ToString());
            return RequestStatus.SUCCESS;
        }
        return RequestStatus.ERROR_PROVIDED_DATA;
    }

    public static RequestStatus Delete(string index)
    {
        var isExists =  File.ReadLines(FILE_PATH).Select(line => line.Split(",")).ToList().Exists(line=>line[4] == index);
        if (!isExists) return RequestStatus.ERROR_NOT_EXISTS;
        var result = File.ReadLines(FILE_PATH).Select(line => line.Split(","))
            .Where(line => line[0] != "string" && line[4] != index)
            .Select(
                simpleData => new StudentModel(simpleData).ToString()
            ).ToList();
        File.WriteAllLines(FILE_PATH, result);
        return RequestStatus.SUCCESS;
    }

    public static RequestStatus Update(string index, StudentModel student)
    {
        var isExists =  File.ReadLines(FILE_PATH).Select(line => line.Split(",")).ToList().Exists(line=>line[4] == index);
        if (!isExists) return RequestStatus.ERROR_NOT_EXISTS;
        // var result = (Read().First(student => student.IndexNumber == index) = student);
        // var result = Read().Where(student => student.IndexNumber == index);
        var result = File.ReadLines(FILE_PATH).Select(line => line.Split(","))
            .Where(line => line[0] != "string" && line[4] != index)
            .Select(
                simpleData => new StudentModel(simpleData).ToString()
                // simpleData1=> new StudentModel(result).ToString()
            ).ToList();
        File.WriteAllLines(FILE_PATH, result.Append(student.ToString()));
        return RequestStatus.SUCCESS;
    }

    }




public enum RequestStatus
{
    SUCCESS,
    ERROR_EXISTS,
    ERROR_NOT_EXISTS,
    ERROR_PROVIDED_DATA,
}
