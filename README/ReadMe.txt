Начало работы :

1. Отработать скрипт DatabaseInitialization.sql

2. В файле appsettings.json изменить секции "Secret" и "DBConnectionString"
Секция "Secret" - секретный ключ для генерации токена аутентификации
Секция "DBConnectionString" - подключение к базе данных MSSQL
Строка имеет вид "Server=Host\\Instance;Database=DatabaseName;User Id=user name;Password=user password"
Дополнительная информация о строке подключения - https://docs.microsoft.com/ru-ru/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=netframework-4.8

3. Ознакомится с файлом Методы API.txt
