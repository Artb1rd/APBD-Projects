using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Provide input and output files");
            return;
        }

        List<Student> students = new List<Student>();
        try
        {
            using (StreamReader sr = new StreamReader(args[0]))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    string[] info = line.Split(",");

                    if (info.Length != 9)
                    {
                        StreamWriter sw = new StreamWriter(@"C:\APBD\tutorial2_ja-Artb1rd\Data\log.txt", true);
                        sw.WriteLine("Student {" + line + "} is incorrect");
                        sw.Close();
                        continue;
                    }

                    Student student = new Student
                    {
                        FirstName = info[0],
                        LastName = info[1],
                        Studies = new List<Study>(),
                        IdStudent = "s" + info[4],
                        BirthDay = DateTime.Parse(info[5]),
                        Mail = info[6],
                        MotherName = info[7],
                        FatherName = info[8]
                    };
                    student.Studies.Add(new Study(info[2], info[3]));
                    var singleStudent = students.Find(it => it.IdStudent == student.IdStudent &&
                                                          it.FirstName == student.FirstName
                                                          && it.LastName == student.LastName
                                                          && it.BirthDay == student.BirthDay);
                    if (singleStudent != null) {
                        var singleStudentStudies = singleStudent?.Studies.Find(x => x.Faculty == info[2] &&
                                                                         x.Mode == info[3]);
                        if (singleStudentStudies != null) continue;
                        singleStudent?.Studies.Add(new Study(info[2], info[3]));
                    }
                    else {
                        students.Add(student);
                    }
                }

                var result = new {
                    CreatedAt = DateTime.Now,
                    Author = "s25012",
                    Students = students
                };
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                string ser = JsonSerializer.Serialize(result, options);
                File.WriteAllText(args[1], ser);
            }
        }
        catch (FileNotFoundException e)
        {
            StreamWriter sw = new StreamWriter(@"C:\APBD\tutorial2_ja-Artb1rd\Data\log.txt", true);
            sw.WriteLine("File name does not exist");
            sw.Close();
        }
        catch (ArgumentException e)
        {
            StreamWriter sw = new StreamWriter(@"C:\APBD\tutorial2_ja-Artb1rd\Data\log.txt", true);
            sw.WriteLine("The given path is invalid");
            sw.Close();
        }
        Console.WriteLine(students.FindAll(x=>x.Studies.Count>1).Count);
        }
}
public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdStudent { get; set; }

    public List<Study> Studies { get; set; }
    public DateTime BirthDay { get; set; }
    public string Mail { get; set; }
    public string MotherName { get; set; }
    public string FatherName { get; set; }

    public override string ToString()
    {
        return FirstName + " " + IdStudent;
    }
}

public class Study
{
    public Study(string faculty, string mode)
    {
        Faculty = faculty;
        Mode = mode;
    }

    public string Faculty { get; set; }
    public string Mode { get; set; }
}