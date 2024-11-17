# onoicrm

`onoicrm.Admin` - Админская часть проекта

`onoicrm.FrontEnd` - Frontend часть проекта 

`onoicrm.Infrastructure.Ef` - Проект который управляет миграциями и EntityFramework

`onoicrm.Domain` - Доменный проект решения где хранится все основные сушьности и.т.д. 

`onoicrm.Setup` - Проект который наполняет тестовыми данными для дальнейшей работы

`onoicrm.Api` - Startup проект и API

`docker` - Настройки докера

### Запуск проекта
1. Скопировать .env.example в .env
2. В корневой директории выполнить `docker compose build --no-cache`. Эта команда соберет все контейнеры
3. Для запуска докера выполнить в консоли `docker compose up -d`
4. В системном файле hosts прописать строку `onoicrm.local    127.0.0.1` (127.0.0.1 - это значение параметра `INTERFACE` из .env)

Теперь при открытии onoicrm.local в браузере будет открыт проект front 

Админка живет по адресу onoicrm.local/admin

API живет по адресу onoicrm.local/api/v1

### Разработка 
При написании админки и фронта изменения автоматически применяются и работает hotreload в браузере

При написании API необходимо пересобирать образ командой `docker compose build`, т.к. dotnet компилирует бэк в dll
