FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "Ui/KafkaDockerSample.Ui.Api/KafkaDockerSample.Ui.Api.csproj"
WORKDIR "/source/Ui/KafkaDockerSample.Ui.Api"
RUN dotnet build "KafkaDockerSample.Ui.Api.csproj" -c Release -o /app
RUN dotnet publish "KafkaDockerSample.Ui.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
RUN rm *.pdb *.Development.json
ENTRYPOINT ["dotnet", "KafkaDockerSample.Ui.Api.dll"]