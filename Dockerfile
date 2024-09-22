FROM mysql:latest

ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_DATABASE=videogamestore

COPY videogamestore.sql /docker-entrypoint-initdb.d/

EXPOSE 3306