#!/bin/bash

/etc/psql-migrate postgres idea_service db_tables.sql
go test -v ./queries
go test -v ./http