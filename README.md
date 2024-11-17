# DentTish

`DentTish.Admin` - Админская часть проекта

`DentTish.FrontEnd` - Frontend часть проекта 

`DentTish.Infrastructure.Ef` - Проект который управляет миграциями и EntityFramework

`DentTish.Domain` - Доменный проект решения где хранится все основные сушьности и.т.д. 

`DentTish.Setup` - Проект который наполняет тестовыми данными для дальнейшей работы

`DentTish.Api` - Startup проект и API

`docker` - Настройки докера

### Запуск проекта
1. Скопировать .env.example в .env
2. В корневой директории выполнить `docker compose build --no-cache`. Эта команда соберет все контейнеры
3. Для запуска докера выполнить в консоли `docker compose up -d`
4. В системном файле hosts прописать строку `denttish.local    127.0.0.1` (127.0.0.1 - это значение параметра `INTERFACE` из .env)

Теперь при открытии denttish.local в браузере будет открыт проект front 

Админка живет по адресу denttish.local/admin

API живет по адресу denttish.local/api/v1

### Разработка 
При написании админки и фронта изменения автоматически применяются и работает hotreload в браузере

При написании API необходимо пересобирать образ командой `docker compose build`, т.к. dotnet компилирует бэк в dll
