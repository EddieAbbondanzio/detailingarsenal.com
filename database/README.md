This is a development set up to run a local Postgres database in docker.

# Start

```
docker-compose up -d --build
```

# Stop

```
docker-compose down
```

# Connect

## Dev

```
psql -h localhost -U postgres -d detailingarsenal
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
