# Aplikacja do pomiaru czasu reakcji 

## Uruchomienie
### Docker
Przy uruchomieniu z wykorzystaniem dockera należy mieć zaintalowany Docker i należy uruchomić następujące komendy w folderze z plikiem Dockerfile.
```bash
docker build . -t musicandreaction:1.0.0
```
```bash
docker run -p 8080:8080 musicandreaction:1.0.0
```
Następnie aplikacja powinna być dostępna w przeglądarce pod adresem localhost:8080

### .Net
Przy uruchomieniu z wykorzystaniem środowiska .Net należy zainstalować .Net w wersji 8.0. Następnie należy wpisać następującą komendę w folderze production.
```bash
dotnet MusicAndReaction.Server.dll  
```
Następnie aplikacja powinna być dostępna w przeglądarce pod adresem localhost:5000
