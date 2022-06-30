# Markdown File

https://picsum.photos/200/300?random=1

Secret Manager
В Windows: %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
В Linux: ~/.microsoft/usersecrets/<user_secrets_id>/secrets.json
1. В консоли перейдите в папку с проектом
2. Активируйте секретное хранилище: 
	dotnet user-secrets init
3. Добавьте секрет в хранилище: 
	dotnet user-secrets set "Movies:ApiKey" "12345"
4. Теперь секреты доступны как часть конфигурации (например, Configuration["Movies:ApiKey"])

