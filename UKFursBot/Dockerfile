FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UKFursBot/UKFursBot.csproj", "UKFursBot/"]
RUN dotnet restore "UKFursBot/UKFursBot.csproj"
COPY . .
WORKDIR "/src/UKFursBot"
RUN dotnet build "UKFursBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UKFursBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UKFursBot.dll"]
