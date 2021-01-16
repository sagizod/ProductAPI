# ProductAPI
Web API for online marketplace - Test


Prerequisites

1. Have .NET Core 5.0 SDK and Runtime installed 

2. SQL Server database called ProductDB, with table "Products"
Query to create table - 

CREATE TABLE [dbo].[Products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[price] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

3. If using docker, setup user login (Uncheck Enforce password policy)

4. Populate database with initial data (or later in usage step 3 using SwaggerUI)

Steps to build and use the API

1. In Startup.cs in the project,if using docker, set up user authentication details in the connection string- 
   
   Default details - 
            //Connection string for Docker 
            services.AddDbContext<ProductContext>(opt => opt.UseSqlServer("Data Source=host.docker.internal;Initial Catalog=ProductDB;User ID=Arun;Password=SQLPasswordTests"));
            
   Alternatively, remove the above line from Startup.cs and add the snippet below to use an in-memory database (you will need to repopulate the database in step 3 each time the web api is restarted)
    services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductDB"));
    
2. Open a command prompt and navigate to the ProductAPI.csproj location
    Run commands - 
      dotnet restore ProductAPI.csproj
      dotnet run
  
3. If using In-Memory DB, or SQL DB has no records yet, open a browser and navigate to localhost:5000/swagger/index.html to add the initial 3 records

4. The API is ready for the postman collection to be run.
   (Note, if the collection needs to be run once again -
      a. SQL Server - reseed the identity column back to 3 using command DBCC CHECKIDENT ('Products', RESEED, 3)
      b. In-Memory DB - restart the API, and add the 3 initial records again
    This is so the identity ID column starts at ID=4 when test 2 is run to add a new product
   )
    
5. There is a dockerfile included in the project to build a docker image and run it in a container (see point 1 regarding Docker connection string) 
  
