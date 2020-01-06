FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore "./Isitar.DoenerOrder.csproj"
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
RUN ls -al
ENTRYPOINT ["dotnet", "Isitar.DoenerOrder.dll"]