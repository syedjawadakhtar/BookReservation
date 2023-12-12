# Book Reservation Backend API
Assignment Submission - .NET Developer Natlex Group Oy

Submitted by: Syed Jawad Akhtar

## Table of Content

 1. [Description](#description)
 2. [API Overview](#api-overview)
 3. [Requirements](#requirements)
 4. [Steps to run the project](#steps-to-run-the-project)
 5. [Debug](#debug)
 6. [Future Prospects of the project](#future-prospects)
 7. [Time distribution](#time-distribution-for-the-assignment)

### Description

An ASP.NET API for Book Reservation system. A minimal API consists of book id, title, comments and status. Minimal API was chosen instead of Controller-based MVC because of the small size of the project. Swagger is implemented in it to test the REST API using user Interface.
MVC-based API must be used for proper documentation and complicated APIs.
Returning TypedResults instead of Results for better testability and automatically returning type metadata for OpenAPI to describe the endpoint.

### API Overview

|     API                                          |     Description                        |     Request   Body    |     Response   body     |
|--------------------------------------------------|----------------------------------------|-----------------------|-------------------------|
|     GET      /api/books                          |     Get all   books                    |     None              |     Array   of books    |
|     GET   /api/books/{id}                        |     Get a   book by ID                 |     None              |     A book              |
|     GET   /api/books/reserved                    |     List of   all "reserved" books     |     None              |     Array   of books    |
|     GET   /api/books/available                   |     List of   all "available" books    |     None              |     Array   of books    |
|     POST   /api/books                            |     Add a   new book                   |     Book   item       |     Book   item         |
|     POST   /api/bookitems/{id}/reserve           |     Reserve   a book with comment      |     None              |     Book   item         |
|     POST   /api/bookitems/{id}/remove-reserve    |     Remove   a status of book          |     None              |     Book   item         |
|     PUT   /api/books/{id}                        |     Update   an existing book          |     Book   item       |     None                |
|     DELETE   /api/books/{id}                     |     Delete   a book item               |     None              |     None                |

### Requirements

 1. Visual Studio (.NET > 5.0)
 2. Docker Desktop (Linux)

### Steps to run the project

 1. Clone this repository
 2. Open the project in Visual Studio
 3. Run through Docker

### Debug

If you encounter this error when starting the application:

```bash
One or more errors occured.

Failed to launch debug adapter. Additional information may be available in the output window.

The operation was canceled.
```

Try this Solution:
 
 a. Faced with identical error, I was able to resolve by deleting VSDBG debugger folder:
 %USERPROFILE%\vsdbg\vs2017u5
 
 b. Clean and Build
 
### Future Prospects

 1. Creating `Icollection` to store the status of the reservation of books and create an endpoint
 2. Use a persistent database to store this information
 3. Shifting to controller-based API for proper code organization, internal/external documentation with swagger and proper integration of microservices.

### Time distribution for the assignment

|     Task                                                                             |     Time   (hrs)    |
|--------------------------------------------------------------------------------------|---------------------|
|     Understanding   the tasks                                                        |     1.5             |
|     Learning   about ASP.NET, Entity Framework (EF) Core Basics, Razor pages, etc    |     3.5             |
|     Completing   the tasks with installing packages                                  |     4            |
|     Documentation                                                                    |     0.5             |
|     Total                                                                            |     9.5               |

