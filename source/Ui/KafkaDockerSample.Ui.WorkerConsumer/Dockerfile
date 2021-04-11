FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source/Ui/KafkaDockerSample.Ui.WorkerConsumer
COPY . .
RUN dotnet restore "KafkaDockerSample.Ui.WorkerConsumer.csproj"
RUN dotnet build "KafkaDockerSample.Ui.WorkerConsumer.csproj" -c Release -o /app
RUN dotnet publish "KafkaDockerSample.Ui.WorkerConsumer.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
RUN rm *.pdb *.Development.json
ENTRYPOINT ["dotnet", "KafkaDockerSample.Ui.WorkerConsumer.dll"]