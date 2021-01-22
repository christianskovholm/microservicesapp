FROM python:3.8.2-slim-buster as base

EXPOSE 5000

WORKDIR /app

# Install system dependencies for dockerize, psycopg2-binary and confluent_kafka
RUN apt update && \
    apt upgrade -y && \
    apt install -y --no-install-recommends wget gcc postgresql-client-11 libpcre3 libpcre3-dev python3-dev librdkafka-dev

RUN apt clean
RUN rm -rf /var/lib/apt/lists/*

# Upgrade pip
RUN python -m pip install --upgrade pip

# Install pipenv and set venv location to project dir
RUN pip install pipenv

ENV WORKON_HOME=/.venvs

# Lock and install dependencies
COPY Pipfile .

RUN pipenv lock
RUN pipenv install

FROM base as dev

# Install dockerize
ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

# Install development venv
RUN pipenv install --dev

ENTRYPOINT [ "/app/scripts/dev.sh" ]

FROM dev as test

COPY . .

ENTRYPOINT [ "/app/scripts/test.sh" ]

FROM base as deploy

# Use an unpriviledged to run the app in production
ARG UID=1000
ARG GID=1000

RUN addgroup --gid ${GID} "activityservice" && \
    adduser \
    --disabled-password \
    --gecos "" \
    --ingroup "activityservice" \
    --no-create-home \
    --uid ${UID} \
    "activityservice"

USER activityservice

# Copy application files
COPY avro_schema.json app.ini ./

ADD activityservice /app/activityservice

ENTRYPOINT [ "pipenv", "run", "uwsgi", "app.ini" ]