# Task 1
During today's exercises, we will start working with REST (Representational State Transfer) server applications. 
Representational State Transfer). For convenient testing of a server application, it will be useful to have a 
tool to manually prepare http requests and send them to our server. For this purpose 
We will install the Postman tool. Please install the tool from the following page. If someone is working 
on a school computer - can also install the Postman tool in the form of a plugin for the 
Google Chrome browser.


# Task 2
In this task, please prepare a new ASP.NET Core Web Application project. Please 
Select an application template of the "API" type. The application to be prepared is to support the university in the process of 
management of student data.
1. please prepare a controller named StudentsController
2.The controller should have 5 public methods related to data management on 
students.
3.The application should save all data in local database in `CSV` file. The file should 
have the following form.
`Name`, `Name`, `Direction of study`, `Mode of study`, `S grade`, `Date of birth`, `Email`, `Mother's name`, `Father's name`.
4. the API should return data in `JSON` format.
5. the first stub should respond to a `HTTP GET` type request to `/students` address. W 
request without any parameters, the tip should retrieve the list of students from the local 
database - a file. We can say that the tip should return a result similar to a 
"select * from students" query.
6 In addition, the `GET` `/students` stub allows us to pass a parameter as a segment of the address of the 
URL that gives us the ability to retrieve a specific student. You can also add this as a separate suffix.
Example of a `GET` `/students/s1234` request.
In this case, we should return a single student with a specific index number.
7 The second stub responding to the `PUT` `/students/s1234` request allows us to update the 
data on a specific student. The tip accepts data sent in `JSON` format. 
Note: the stub expects to send complete data about the student. Only the field 
"index number" is not modified and is treated as an identifier of the student. In addition, 
in case of success, the tip should return the actual data associated with the enrolled 
student.
8 The third stub should allow us to add a new student by executing a `POST` request.
to the `/students` address. The stub accepts the data of the new student in the body of the request in `JSON` format. 
`JSON`. Before adding a new student, we should make sure that:
a. All data about the student is complete. If not, we return 
corresponding error code.
b. We check that the selected index number is unique and that its format is correct.
We add the student to the end of the `CSV` file that constitutes our database.
9. The fourth end should respond to requests like `DELETE` `/students/s1234` and allows us to 
remove a specific student from the database.
10. remember to return valid HTTP codes and possible error handling