
#### Dockerfile

    FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
    WORKDIR /app
    COPY . ./
    RUN dotnet restore && dotnet publish -c Release -o out

    FROM mcr.microsoft.com/dotnet/aspnet:6.0
    WORKDIR /app
    COPY --from=build-env /app/out .
    ENV CONNECTION_STR=[pqsql_connection_string_here]
    ENV MYFXBOOK_USER=[myfxbook_username_here]
    ENV MYFXBOOK_PASS=[myfxbook_password_here]
    ENV MYFXBOOK_URL='https://www.myfxbook.com'

    EXPOSE 80
    ENTRYPOINT [ "dotnet", "COTReport.API.dll" ]

