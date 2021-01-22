FROM mcr.microsoft.com/mssql/server:2017-CU21-ubuntu-16.04

# Install packages required for mssql-cli
RUN apt-get update \
    && apt-get install -y --no-install-recommends apt-transport-https curl software-properties-common

# Install mssql-cli
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN apt-add-repository https://packages.microsoft.com/ubuntu/16.04/prod
RUN apt-get install -y mssql-cli
RUN apt-get install -f

COPY ./scripts/mssql-cli /usr/local/bin/cli