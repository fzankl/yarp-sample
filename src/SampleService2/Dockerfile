#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5002
ENV ASPNETCORE_URLS="http://*:5002"


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SampleService2/SampleService2.csproj", "SampleService2/"]
RUN dotnet restore "SampleService2/SampleService2.csproj"
COPY . .
WORKDIR "/src/SampleService2"
RUN dotnet build "SampleService2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleService2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleService2.dll"]