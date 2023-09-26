
# Commify Tech Test

## Database Creation

This project uses Entity Framework Core as the ORM to talk to the underlying SQL Server database. To create the database before running the application for the first time, please follow these instructions:

* Open the [persistencesettings.json](src/CommifyTechTest.Persistence/persistencesettings.json) file and update the SQL connection string
* Open a command window in the [`src\CommifyTechTest.Persistence`](src\CommifyTechTest.Persistence) folder
* Run the `dotnet-ef database update` command (requires the EF Core CLI to be installed)

## Third Party Packages Used

As well as EF Core, The following lists some of the third-party packages used in this project:

* Autofac is used for DI management
* Mediatr is used to separate the presentation layer from the application layer
* Rather than wait until all employees have been processed before providing a response, the API will return an Accepted response to indicate that the employees have all been accepted and are currently being processed. Quartz is used to schedule tasks for adding employees
* NUnit, Moq and FluentAssertions are used for writing automated tests

## Running the Application

The backend for the application is an ASP.NET Web API that can be started the same way as running any other .NET application (e.g running `dotnet run` against the [CommifyTechTest project](src/CommifyTechTest), opening the solution in Visual Studio and pressing `F5`, etc). The base address for the backend API will be `https://localhost/7114` by default.

## Automated Tests

The solution contains a number of unit tests, as well as a functional test that spins up an instance of the API, uploads a CSV and then asserts that the relevant employees were saved to the database and the correct response was returned.

## Known Issues

The following lists the known issues that I did not have time to resolve before submitting the project:

* The unit of work is committed after each individual employee is added. It could be more performant to add all employees to the repository and then commit the unit of work at the end
* The functional test project uses the same database as the locally running instance, causing the tests to potentially pollute the database
* As the employees are added asynchronously, the functional test waits for 10 seconds before looking in the database to see if the employee was added. It is unlikely that it will take the full 10 seconds for the employee to be added so the functional test could proactively monitor the database and respond as soon as it's ready