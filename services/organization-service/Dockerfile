ARG DOTNET_SDK_VERSION=5.0.100-buster-slim-amd64

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} as base

EXPOSE 5001

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

WORKDIR /app

FROM base as dev

ENTRYPOINT [ "./scripts/dev.sh" ]

FROM base as restore

COPY ./src/OrganizationService.Domain/OrganizationService.Domain.csproj /app/src/OrganizationService.Domain/OrganizationService.Domain.csproj 
COPY ./src/OrganizationService.Infrastructure/OrganizationService.Infrastructure.csproj /app/src/OrganizationService.Infrastructure/OrganizationService.Infrastructure.csproj 
COPY ./src/OrganizationService.Application/OrganizationService.Application.csproj /app/src/OrganizationService.Application/OrganizationService.Application.csproj 
COPY ./test/OrganizationService.UnitTests/OrganizationService.UnitTests.csproj /app/test/OrganizationService.UnitTests/OrganizationService.UnitTests.csproj
COPY ./test/OrganizationService.IntegrationTests/OrganizationService.IntegrationTests.csproj /app/test/OrganizationService.IntegrationTests/OrganizationService.IntegrationTests.csproj
COPY ./test/OrganizationService.FunctionalTests/OrganizationService.FunctionalTests.csproj /app/test/OrganizationService.FunctionalTests/OrganizationService.FunctionalTests.csproj

RUN find . -name "*.csproj" -exec dotnet restore {} \;

COPY . .

FROM restore as test

ENTRYPOINT ["./scripts/test.sh"]

FROM restore as publish

RUN dotnet publish ./src/OrganizationService.Application -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} as deploy

EXPOSE 5001

WORKDIR /app

COPY --from=publish /publish .

# Create an unpriviledged to run the app in production
ARG UID=1000
ARG GID=1000

RUN addgroup --gid ${GID} "organizationservice" && \
    adduser \
    --disabled-password \
    --gecos "" \
    --ingroup "organizationservice" \
    --uid ${UID} \
    "organizationservice"

USER organizationservice:organizationservice

ENTRYPOINT [ "dotnet", "OrganizationService.Application.dll", "--urls", "http://0.0.0.0:5001" ]