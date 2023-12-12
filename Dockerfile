# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development

# Use the .NET SDK for building our application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeForum/CodeForum.csproj", "./"]
RUN dotnet restore "./CodeForum.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CodeForum/CodeForum.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CodeForum/CodeForum.csproj" -c Release -o /app/publish

# Final stage / image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeForum.dll"]