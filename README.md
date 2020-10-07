# SearchFight
An application that queries search engines and compares how many results they return

## Requirements
  * dotnet core 3.1

## Usage
Run the following commands, followed by the search words you want to compare (you must provide at least 2 words), and enjoy the winner:
```
cd Searchfight
dotnet run .net java go "frontend frameworks"
```
  * This app supports a variable amount of search words
  * This app support quotation marks
  * This app supports searching in many search engines, by default it provides results of Google and Bing

## What if i want more search engines?
You just need to add the new search engine settings in the appsettings file; You must provide the URL of the API, the http headers, the query parameters necessary to make the request to the search engine API, you must also provide the path of the response that indicates the number of results obtained.

## What if i want to execute tests?
You just need to run the following command:
```
dotnet test
```
