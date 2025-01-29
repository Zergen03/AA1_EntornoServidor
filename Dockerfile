FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./

COPY Presentation/*.csproj Presentation/
COPY Data/*.csproj Data/
COPY Services/*.csproj Services/
COPY Models/*.csproj Models/
COPY AA1.DTOs/*.csproj AA1.DTOs/
COPY .env .env

RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/out ./
COPY .env .env


VOLUME [ "/app/ddbb" ]

ENTRYPOINT ["dotnet", "AA1.dll"]

#docker run -v ${PWD}/ddbb:/app/ddbb -it aa1
