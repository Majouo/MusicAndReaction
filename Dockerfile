FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY Production .

EXPOSE 5000

CMD ["dotnet", "MusicAndReaction.Server.dll"]