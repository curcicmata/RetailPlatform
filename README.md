# Retail Platform

This is a simple Retail Platform application that processes Carts and Cart Items for customers.

How to run:
- clone this project
- run in Terminal:
     **docker-compose up --build**
  - access the application via browser: http://localhost:5000/swagger
 

Inside **CartController** there is [Authorize] attribute for HTTP POST and HTTP DELETE endpoints.
For testing purposes, there is a **TokenController** that generates JWT for username='test' and password='Password123!'. Use generated token after that for authorization in Swagger to test those endpoints.

MassTransit (version before going commercial) is used for messaging and SQL server is used as a database.
