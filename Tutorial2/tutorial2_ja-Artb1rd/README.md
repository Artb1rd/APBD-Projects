# Tutorial 2

The task is to write an application that reads a `CSV` file with a list of students then filters it and removes duplicates and incorrect records from it. Then save the file in `JSON` format.

## Functioning of the application in a nutshell.
* The program receives two arguments - the location of the input file and the location of the output file.
* Then the file is read and duplicate students are filtered.
* Finally, a new file without duplicates is created (or overwritten) in selected location.

## Requirements
* The system should read students from the input file.
* The system should write only unique records to the final file.
* The system should write all exceptions and errors that occurred to the `log.txt` file.
* In case the system encounters a student who already exists, but in other studies, it should only add new studies to the existing record.
* In the output file, the student's index number should start with the letter `s`.

## Notes:
* The fields in the `.csv` file are respectively: `Name`, `Name`, `Direction of Study`, `Mode of Study`, `S Name`, `Date of Birth`, `Email`, `Mother's Name`, `Father's Name`.
* A record is non-unique when it has the same S, First and Last Name, and Direction and Mode of study as another student. 
* An erroneous record is one that does not consist of 9 fields.
* The format of the data in the output file is. 
```yaml
{
  CreatedAt: datetime,
  Author: string,
  Students: [
      {
        Index: string,
        Name: string
        Surname: string,
        Email: string,
        BirthDate: date,
        MothersName: string,
        FathersName: string,
        Studies: [
           {
              Name: string,
              Mode: string
           }...
        ]
      }...
    ]
}
```

# Help
* To handle `CSV` files, you can use the `CsvHelper` library.
* To handle `JSON` files you can use the `JsonSerializer` class.