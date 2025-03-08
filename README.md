# GreetingApp_2115100017
**UC1:** Using GreetingController return JSON for different HTTP Methods. Test using curl

**UC2:** Extend GreetingController to use Services Layer to get Simple Greeting message ”Hello World”

**UC3:** Ability for the Greeting App to give Greeting message with:
  1. User First Name and Last Name or
  2. With just First Name or Last Name based on User attributes provides or
  3. Just Hello World.

**UC4:** Ability for the Greeting App to save the Greeting Message in the Repository

**UC5:** Ability for the Greeting App to find a Greeting Message by Id in the Repository

**UC6:** Ability for the Greeting App to List all the Greeting Messages in the Repository

**UC7:** Ability for the Greeting App to Edit a Greeting Messages in the Repository

**UC8:** Ability for the Greeting App to delete a Greeting Messages in the Repository

**UC9:** Apply Swagger and global exception in the project.

**UC10:** 
 1. Previous Controller will have all the api's from UC1 to UC8. Creating Second Controller (UserController) where it will contain api for login, registration, forget password     and reset password. Apply logic for registration and loginapi for user. Apply hashing also(salting) for register and login.
 2. Setting up Redis Cache for Greeting Messages.

**UC11:** Applying  JWT into project.

**UC12:** Giving body to Forget password by applying SMTP server mail generation and sending token generated using jwt in the mail and 
giving body to Reset password using JWT token received in mail.

**UC13:** Create Primary/Foreign key relationship between greeting and user table.
Adjust greeting methods according to new relationship.

**UC14:** Applying Redis and RabbitMQ.
