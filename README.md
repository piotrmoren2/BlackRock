# BlackRock Task

React + ASP.NET web app.

I only mocked a database using the provided CSV file as  SQLite would basically give the same outcome and a full, prod-like solution with a proper database server and EF Core seems to be a bit too much for a 3-5h task.

If I were to use something like MS SQL or Postgres I would add the following components:

1. DbContext files, which would introduce repository-like access to collections
2. Migrations creating/modifying data tables
3. Access authorization (JWT for example)
4. Role based authorization to data repositories
5. Additional measures to avoid dirty read/writes to the database. For example, Unit of Work pattern on data manipulation RestAPI requests

   
