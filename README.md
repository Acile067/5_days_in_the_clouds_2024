# Levi9 Challenge
A Levi9 cloud hackathon. 

## Built using 

- &nbsp; <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRxo1QGx_G_1-2qBwh3RMPocLoKxD782w333Q&usqp=CAU" align="center" width="28" height="28"/> <a href="https://dotnet.microsoft.com/en-us/apps/aspnet"> ASP.NET 8 + Entity Framework 8 </a>
- &nbsp;<img src="https://www.automatetheplanet.com/wp-content/uploads/2023/04/nUnit-logo.png" align="center" width="32" height="32"/><a href="https://nunit.org/"> NUnit </a>

## Installation:

You can run the app using the `dotnet` CLI. You can download and install the dotnet SDK 8.0 for your operating system [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). 
In case you already have it installed but are not sure of the version, run the 
`dotnet --list-sdks` command in your terminal to check.

## How to run:

### Clone the project 
   - `git clone https://github.com/Acile067/5_days_in_the_clouds_2024.git`
   - `cd 5_days_in_the_clouds_2024`
   - `cd Levi9_competition`
   - `cd Levi9_competition`

### Dotnet CLI:
   
 - Run the app on port **5050**: `dotnet run`

## API

<b> Go to `http://localhost:5050/swagger/index.html` to test it with Swagger UI. </b>

The application will be available on `http://localhost:5050` with a endpoints:</br>
Player: GET-> `/players` `/players/{id}` POST-> `/players/create`</br>
Team: GET-> `/teams/{id}` POST-> `teams`</br>
Match: POST-> `/matches`

An example POST request for the player _Player11_:

```http request
POST http://localhost:5050/players/create
```
`body:`
```http body
{
  "nickname": "Player11"
}
```
`response:`
```http response
HTTP/1.1 200 OK
        
{
  "id": "cb533375-4bfa-4a9d-b89a-c7f850d40ed2",
  "nickname": "Player11",
  "wins": 0,
  "losses": 0,
  "elo": 0,
  "hoursPlayed": 0,
  "teamId": null,
  "ratingAdjustment": null
}
```
An example GET request for a ID _eff975aa-0615-4423-9152-5ab749d95d2c_:
```http request
GET http://localhost:5050/players/eff975aa-0615-4423-9152-5ab749d95d2c
```
`response:`
```http response
HTTP/1.1 200 OK
        
{
  "id": "eff975aa-0615-4423-9152-5ab749d95d2c",
  "nickname": "Player4",
  "wins": 1,
  "losses": 0,
  "elo": 25,
  "hoursPlayed": 60,
  "teamId": "a10f9801-20cb-446d-8236-881a24a5e683",
  "ratingAdjustment": 50
}
```

An example GET request for a player that doesn't exist, returning `HTTP 404`:
```http request
GET http://localhost:5050/players/RandomID
```
```http request
HTTP/1.1 404 Not Found

"Player not found"
```
An example POST request for the team:
```http request
POST http://localhost:5050/teams
```
`body:`
```http body
{
 "teamName": "Team1",
 "players": [
 "b769b730-d1b9-4a94-8d2d-2936e050722c",
 "34e7a9e7-df16-4082-92d5-cadb8fc087e5",
 "64f6186b-3a39-47b9-9313-3fcb8e4f06fd",
 "187b683c-1255-46a6-a709-60f8af0fd200",
 "f8987880-8869-472c-8335-83f60132b15d"
 ]
}

```
`response:`
```http response
HTTP/1.1 200 OK
        
{
 "id": "b909d79d-04d3-442d-9b43-29b2a44cc628",
"teamName": "Team1",
 "players": [
 {
 "id": "f8987880-8869-472c-8335-83f60132b15d",
 "nickname": "Player5",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "34e7a9e7-df16-4082-92d5-cadb8fc087e5",
 "nickname": "Player2",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "187b683c-1255-46a6-a709-60f8af0fd200",
 "nickname": "Player4",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "b769b730-d1b9-4a94-8d2d-2936e050722c",
 "nickname": "Player1",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "64f6186b-3a39-47b9-9313-3fcb8e4f06fd",
 "nickname": "Player3",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 }
 ]
}
```

An example GET request for the team:
```http request
GET http://localhost:5050/teams/b909d79d-04d3-442d-9b43-29b2a44cc628
```
`response:`
```http response
HTTP/1.1 200 OK
        
{
 "id": "b909d79d-04d3-442d-9b43-29b2a44cc628",
"teamName": "Team1",
 "players": [
 {
 "id": "f8987880-8869-472c-8335-83f60132b15d",
 "nickname": "Player5",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "34e7a9e7-df16-4082-92d5-cadb8fc087e5",
 "nickname": "Player2",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "187b683c-1255-46a6-a709-60f8af0fd200",
 "nickname": "Player4",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "b769b730-d1b9-4a94-8d2d-2936e050722c",
 "nickname": "Player1",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 },
 {
 "id": "64f6186b-3a39-47b9-9313-3fcb8e4f06fd",
 "nickname": "Player3",
 "wins": 0,
 "losses": 0,
 "elo": 0,
 "hoursPlayed": 0,
 "teamId": "b909d79d-04d3-442d-9b43-29b2a44cc628",
 "ratingAdjustment": 50
 }
 ]
}
```
An example POST request for the match:

```http request
POST http://localhost:5050/matches
```
`body:`
```http body
{
 "team1Id": "dacfe004-42d8-4938-8e1c-a1fe46739cb6",
 "team2Id": "7265bc21-46bc-40e3-a2d5-3338c8cc7495",
 "winningTeamId": "dacfe004-42d8-4938-8e1c-a1fe46739cb6",
 "duration": 3
}
```
`response:`
```http response
HTTP/1.1 200 OK

```

## Tests

The dotnet CLI is required in order to run tests.

- `cd ..`
- `dotnet test`

```
Test summary: total: 6, failed: 0, succeeded: 6, skipped: 0, duration: 2.9s
```
