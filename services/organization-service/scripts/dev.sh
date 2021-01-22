#!/bin/bash

src=./src/OrganizationService.Application

dockerize -wait tcp://sql-server:1433 -timeout 30s -wait-retry-interval 5s true
dotnet ef database update --msbuildprojectextensionspath $src/obj/container -s $src -p $src
dotnet watch --project $src run