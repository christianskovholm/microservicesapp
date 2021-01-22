FROM golang:1.15-buster as base

EXPOSE 8080

# Install postgres client for database migrations
RUN apt update && \
    apt upgrade -y && \
    apt install -y postgresql-client

WORKDIR /app

FROM base as dev

# Install inotify-tools for hot reloading
RUN apt install -y inotify-tools

ENTRYPOINT [ "./scripts/dev.sh" ]

FROM base as restore

# Install dependencies before copying the app files.
# This is to utilize image layer caching, so that dependencies do not have
# to be reinstalled if there have only been made modifications to the app source code
COPY go.mod .

RUN go mod download
RUN go mod verify

COPY . .

FROM restore as test

ENTRYPOINT [ "./scripts/test.sh" ]

FROM restore as build

# Create an unpriviledged to run the app in production
ARG UID=1000
ARG GID=1000

RUN addgroup --gid ${GID} "ideaservice" && \
    adduser \
    --disabled-password \
    --gecos "" \
    --ingroup "ideaservice" \
    --uid ${UID} \
    "ideaservice"

COPY . .

# Build the app
RUN make

FROM scratch as deploy

# Copy the app from the build stage
COPY --from=build /go/bin/ideaservice /go/bin/ideaservice

# Copy and use the unpriviledged user created in the build stage
COPY --from=build /etc/passwd /etc/passwd
COPY --from=build /etc/group/ /etc/group

USER ideaservice:ideaservice

EXPOSE 8080

ENTRYPOINT [ "/go/bin/ideaservice" ]