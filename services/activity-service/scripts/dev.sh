#!/bin/bash

dockerize -wait http://schema-registry:8081 -timeout 60s -wait-retry-interval 10s true
/etc/psql-migrate postgres activity_service db_tables.sql
pipenv run uwsgi app.ini --py-autoreload=1