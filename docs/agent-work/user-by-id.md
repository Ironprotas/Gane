# Adventurer by id

## Исходная задача

Исправить получение авантюриста по id и довести сценарий до API:

- в `GetAdventurer` не передавался параметр `$1`;
- в SELECT не было поля `id`;
- метод репозитория не был прокинут в сервис и контроллер;
- для отсутствующей записи нужен ответ `404`, для найденной — `200`.

Также доведён до рабочего состояния `DeadAdventurer` (раньше в сервисе был `NotImplementedException`).

## Изменённые файлы

- `Models/Dto/AdventurerDto.cs` — добавлено поле `Id`
- `Repositories/AdventurerRepository.cs` — параметр `$1`, выборка `id`, общий mapping
- `Services/IAdventurerService.cs` — `GetAdventurer`, реализация `DeadAdventurer`
- `Controllers/AdventureController.cs` — `GET api/Adventure/{id}` (200/404), `POST api/Adventure/{id}/dead`
- `GaNe.http` — примеры запросов
- `docs/agent-work/user-by-id.md` — этот отчёт

## Как проверить сценарии 200 и 404

Предусловия: PostgreSQL доступен, в `appsettings.Development.json` задан `ConnectionStrings:DefaultConnection`, приложение запущено:

```bash
dotnet run --launch-profile http
```

### 200 — авантюрист найден

1. Создать запись:

```bash
curl -s -X POST http://localhost:5135/api/Adventure/create \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"Aragorn\",\"archetype\":1,\"sex\":0,\"isLive\":true,\"rating\":10}"
```

Ответ — GUID созданного авантюриста.

2. Запросить по этому id:

```bash
curl -i http://localhost:5135/api/Adventure/<созданный-guid>
```

Ожидание: HTTP `200`, JSON с полями `id`, `name`, `expeditionId`, `archetype`, `sex`, `isLive`, `rating`.

### 404 — авантюрист не найден

```bash
curl -i http://localhost:5135/api/Adventure/11111111-1111-1111-1111-111111111111
```

Ожидание: HTTP `404`.

Альтернатива: запросы из `GaNe.http` (подставить реальный id вместо нулевого GUID для сценария 200).

## Что проверено / что не проверено

- Проверено: `dotnet build` проходит успешно.
- Не проверено: HTTP-сценарии `200` и `404` (приложение против живой БД не запускалось в рамках этой задачи).
- Не проверено: `POST api/Adventure/{id}/dead`.
- Не проверено: `GET api/Adventure/all` после появления `Id` в DTO.
