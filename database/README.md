For simplicity sake it's best to run Postgres in a Docker container on the dev workstation. This opens up the possibility for multiple versions of databases on the laptop without conflict. 

# Start / Stop
```
docker-compose up -d --build
docker-compose down
```
## Connect
```
psql -h localhost -U postgres -d detailingarsenal
```

## Initialization
To get the development database functional we'll need to set up the password for the postgres user, and create a new database.

To set the password set the POSTGRES_PASSWORD environment variable in the docker-compose.yml file. This will only be loaded on the very first start.
https://hub.docker.com/_/postgres

After that we'll need to create our new database. 
```
CREATE DATABASE detailingarsenal
```

## Prod

```
psql -h detailingarsenal-db-do-user-4028496-0.a.db.ondigitalocean.com -U detailingarsenal -d detailingarsenal-prod -p 25060
```

## Delete Databse

```
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
```
