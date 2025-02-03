# AireLogic-TechinicalTest

Firstly, thank you for the opportunity, and thank you for reviewing this code.

My background is primariy C# .Net with Entity Framework so I have create a full working web API.
Most features have been written (I had misread the part where it states that not all features are required, and read it as all features are required!).
Code is layered to enable consumers to swap out the backend/frontend. An example of this is if the client wants
to use a different database (currently SQL Server), the code is extensible to provide different repositories. 

The API supports a swagger frontend so frontend developers can easily see how to consume the API. Error handling
is handled via middleware, and will provide a structured error response if an exception is raised. This is paired 
with a HandledException which controls HTTP Status Code and the message to return. 

I have also included BDD unit tests to help test and document the intended behaviour of the API. 

If you have any questions, feel free to ask.

Lewis
