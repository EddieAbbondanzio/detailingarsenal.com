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

Having trouble connecting? The database may not be running. If you didn't set the POSTGRES_PASSWORD environment variable the contaienr will fail to start.
https://hub.docker.com/_/postgres

## Prod

```
psql -h detailingarsenal-db-do-user-4028496-0.a.db.ondigitalocean.com -U detailingarsenal -d detailingarsenal-prod -p 25060
```

## Delete Databse

```
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
```
