# Parser Web Application

This is a web application developed to accept user input Web page URL and read and display the html content of the page for the following information :
*  Read image URLs from the page and display it in a carousel.
*  Read the total number of the words used in the web page.
*  Read the text content on the page for all the words and find the top 10 most used words and display the details in the form of chart.

# Tech Stack

This project is developed using the asp.net core framework version 6. For server side coding, C# langugage is used and for client side coding, JQuery a javasscript based scripting language is used and few thrid party js packages such as slick.js for image carousel and google visualization charts are leveraged.  

# Project Structure

Following three projects are part of the solution
*  Parser.API Is a .net core web API project which has a API controller (LoadUrlController) method named "LoadUrl" which takes url parameter of a webpage and leverge HtmlAgilityPack libraries to read the HTML content of the page for images, words and count and return the result.
*  Parser.Web is a .net core MVC web application which is used to render a web page with the user interfacing requesting the URL from the end user and further request the Parser.API to retrive the result set containing the image URLs and word information and deplay them using the slick carousel and google visualization chart.
*  Parset.API.Test is a simple NUnit test application used to test the API.

# Project UI

Below is the sample UI design page with the steps to access the page.

* Step 1

![image](https://user-images.githubusercontent.com/61128349/192678639-17365ce9-9690-4453-ac71-b303fbba55ef.png)

* Step 2
![image](https://user-images.githubusercontent.com/61128349/192678796-5731a5e6-b5bb-4853-a257-fe06ef6e3a31.png)
