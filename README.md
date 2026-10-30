# 🌌 Constellation Restore API

Web API на **ASP.NET Core (.NET 9)**, которое восстанавливает повреждённый код созвездия — определяет, каких символов не хватает в повреждённой строке по отношению к эталонной.

> 🐛 Помогает Багги восстановить код и открыть дверь сокровищницы.

---

## ✨ Возможности

- 🔍 Поиск отсутствующих символов **без использования `Contains()` / `IndexOf()`** — только циклы и сравнения.
- 📤 Возврат результата в структурированном виде (`missingSymbols` + `isComplete`).
- ✅ Строгая валидация входных данных (`A-Z`, уникальность символов в эталоне).
- 📚 Встроенный Swagger UI для ручного тестирования.
- 🧩 Чистая архитектура: DTO → Service → Controller.

---

## 🛠️ Стек

| Компонент | Версия |
|---|---|
| .NET | 9.0 |
| ASP.NET Core | 9.0 (Controllers) |
| Swashbuckle.AspNetCore | 6.9.0 |

---

## 📂 Структура проекта

```
ConstellationRestore/
├── ConstellationRestore.csproj
├── Program.cs
├── Controllers/
│   └── ConstellationsController.cs
├── Dtos/
│   ├── RestoreRequest.cs
│   └── RestoreResponse.cs
└── Services/
    ├── IConstellationService.cs
    └── ConstellationService.cs
```

---

## 🚀 Быстрый старт

### Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Установка и запуск

```bash
git clone https://github.com/a1exx2a/constellationrestore.git
cd ConstellationRestore

dotnet restore
dotnet run
```

После запуска открой Swagger:

```
http://localhost:5xxx/swagger
```

> Порт смотри в консоли при запуске (`Now listening on: ...`).

---

## 🔌 API

### `POST /api/constellations/restore`

Возвращает отсутствующие в повреждённом коде символы (отсортированные по алфавиту).

#### Request body

```json
{
  "reference": "DRAGON",
  "damaged": "GOD"
}
```

| Поле | Тип | Обязательное | Описание |
|---|---|---|---|
| `reference` | string | ✅ | Эталонный код. Только `A-Z`, без повторов. |
| `damaged` | string | ✅ (может быть `""`) | Повреждённый код. Только символы из `reference`. |

#### Response `200 OK`

```json
{
  "missingSymbols": "ANR",
  "isComplete": false
}
```

| Поле | Тип | Описание |
|---|---|---|
| `missingSymbols` | string | Отсутствующие символы, отсортированные по алфавиту. |
| `isComplete` | bool | `true`, если отсутствующих символов нет. |

#### Response `400 Bad Request`

Возвращается при:

- отсутствии `reference` или `damaged`,
- наличии символов не из `A-Z` в `reference`,
- повторе символов в `reference`,
- наличии в `damaged` символа, которого нет в `reference`.

Пример:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Reference": ["Reference содержит повторяющийся символ 'O'."]
  }
}
```

---

## 🧪 Примеры запросов

### 1. Базовый пример

```json
{ "reference": "DRAGON", "damaged": "GOD" }
```
```json
{ "missingSymbols": "ANR", "isComplete": false }
```

### 2. Все символы на месте

```json
{ "reference": "STAR", "damaged": "TARS" }
```
```json
{ "missingSymbols": "", "isComplete": true }
```

### 3. Пустой повреждённый код

```json
{ "reference": "CYGNUS", "damaged": "" }
```
```json
{ "missingSymbols": "CGNSUY", "isComplete": false }
```

### 4. Ошибка валидации — повтор в эталоне

```json
{ "reference": "ORION", "damaged": "OR" }
```
→ `400 Bad Request` — «повторяющийся символ 'O'».

### 5. Ошибка валидации — символ не из эталона

```json
{ "reference": "STAR", "damaged": "STARX" }
```
→ `400 Bad Request` — «символ 'X' отсутствует в reference».

---

## 🧠 Как работает алгоритм

```
Для каждого символа в reference:
    пройти по всем символам damaged
    если совпадение найдено → символ есть
    иначе → добавить в список отсутствующих

Отсортировать список
Вернуть как строку
```

Никаких `Contains()`, `IndexOf()`, `HashSet` и т.п. — только явные циклы и `==`, как требует задание.

---

## 🏗️ Архитектура

```
HTTP Request
    ↓
[ApiController] + IValidatableObject  →  валидация (400 при ошибке)
    ↓
ConstellationsController  →  вызывает сервис
    ↓
IConstellationService / ConstellationService  →  бизнес-логика
    ↓
RestoreResponse  →  JSON → клиент
```

---