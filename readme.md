## PPT Technical Questionnaire
### Description:
The following is the backend solution for the technical interview questionnaire as requested by Pacific Programming and Tech inc.

### Structure
The solution contains:

- A Web API project named `Backend`
- A folder named `DATA`

### Details

- The `DATA` folder contains the specified database file.
- This database file is referenced in `./Backend/appsettings.json`.
- The application currenty runs on port 7125 for HTTP and 7126 for HTTPS 
  as suggested in the frontend and can be modified in `./Backend/Program.cs`

### Tech Stack

- The project is build using `dotnet(v8) webapi` template.
- It leverages `EntityFrameworkCore` and it corresponding `Sqlite` packages