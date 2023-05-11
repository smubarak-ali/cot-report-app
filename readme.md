
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


### Bash file to run with the docker sentiment the crontab
1- Run the script in the correct way
2- One should have #!bin/bash at the starting of the script. It is a shebang that is required by each script.
3- One should save the file without .sh extension
4- One should provide the execution permission to the script by giving command chmod 777 script_name
5- Run the script with bash script_name

Inside the bash file the following
docker start [container_name]

