# Avito Ads Parser
This is a simple parser for Avito ads. It can parse ads from the main page and from the search page.

## Setup 
Create a .env file in the root directory with the following content:
```env
MONGODB_URI=
MONGODB_NAME=
```
Here `MONGODB_URI` is the URI of the MongoDB database and `MONGODB_NAME` is the name of the database.

Run using docker:
```bash
docker build -t avito-parser .
docker run -it avito-parser
```
