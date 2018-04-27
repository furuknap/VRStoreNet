# VR Store Demo project
##Read Me
This is a sample project called VR Store. The application is simple and focuses solely on the renting of videos, calculating the correct price, and tracking overdue rents.
*Please note that this project is made for a specific purpose based on requirements that are not made public.*

##Choices made
Based on the project requirements, I've chosen to produce a C# ASP.NET MVC solution because it offers extreme efficiency in creating this particular type of project. The total build time for the entire solution, including architecture, testing, and QA, is about 5 hours.

I've chosen to add a UI in addition to the API, despite not being part of the requirements. ASP.NET MVC makes this creation very simple and automatic so it is actually more work to take it away. The automatic UI also serves as a modest test bed to add and modify entities.

However, this causes a duplication in some of the controllers. This is a framework issue because ASP.NET MVC separates regular controllers and API controllers. One can write a single controller to serve both purposes, and/or write a service to handle both methods, but I have chosen to avoid both these options to reduce complexity. There are comments in place in the API Video controller.

The service would be simple, basically just migrating the current code into a new class and setting up a service locator. Next steps would introduce dependency injection, caching, and so on, so stopping before the service implementation is just a random point on a long scale.

In reality, I tend to put data access in a separate project so that it is usable from other applications.

I have avoided certain aspects in this solution. I do not have user management, permissions, no notifications on overdue copies, no payment integration, no visualization of user data such as videos rented, overdue payments, bonus points, etc.

##Project Overview
Videos are defined by their title and release date, plus available copies. Other properties are calculated, such as the price and availability is defined by calculations from the project requirements.

Each video has zero or more copies available. I chose this approach because it reduces duplication of data. Each copy is defined by its video parent, rented date, who rented it, and for how many days it has been rented. If there is no rented it, the copy is considered available for rent.

Finally, I've chosen to track the user's rent history. The UserHistory element tracks which users rents which copies, as well as any points earned. There is no management or visualization of user history.

##Running the project
1. Download the source from https://github.com/furuknap/VRStoreNet
2. Open .sln in Visual Studio 2017
3. Hit F5 to launch the project with debugging.

##Testing
Upon launch, the front page will show a list of available videos, which are none when you first launch. This is because no videos have available copies yet. As such, follow the link at the bottom right to manage videos.

On the Videos page, click on Details for any video. At the bottom there will be an empty list of copies associated with the video. Initially, no copies are available, so click the Add Copy link one or more times to add copies. The video will now show up on the front page.

From the front page, the video should have a link to log in or rent if you are already logged in. After logging in and clicking the Rent link, you can choose the number of days to rent the video and see the price. Clicking the Rent button will make the copy unavailable for renting.

Finally, return to the Video details page and note that the copy is now listed as rented. Once the return due date is past, the entry will also show the amount due in overages. You can click the Return link to mark the copy as returned.

##Troubleshooting
The model backing the 'ApplicationDbContext' context has changed since the database was created. 
Stop debugging the solution (red square Stop icon in Visual Studio).
Open Package Manager Console (View->Other Windows->Package Manager Console)
Type update-database and hit Enter
Hit F5 to retry running the application
