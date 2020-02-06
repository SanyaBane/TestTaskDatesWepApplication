Решение состоит из 3 компонентов:

 - ASP.NET WebAPI сервер
 - Клиентское консольное приложение
 - БД


## ASP.Net WebAPI

Использует EntityFramework для работы с подключением к SQL Server.
Стандартная  строка подключения прописывается  в файле "appsettings.json".
По умолчанию:

"ConnectionStrings": 
{
    "DefaultConnection": "Server=DESKTOP-1R7VSBP;Database=voichuk_test_task_db;Trusted_Connection=True;"
}
  
Использован подход CodeFirst.

Два контроллера: Account и Date ("/api/Account/" и "api/Date/").

Поддерживаются  запросы **PUT** (добавление) и **POST** (фильтрация по заданию, регистрация, авторизация) и **GET** для получения всех имеющихся в БД диапазонов дат.

Примеры использования апи:

1) https://localhost:5001/api/Account/Register - POST - регистрация нового пользователя. 

=== Body example: ===

Login:user322
Password:228

2) https://localhost:5001/api/Account/Token - POST - авторизация существующего пользователя

=== Body example: ===

Login:user322
Password:228

3) https://localhost:5001/api/Date/get/all - GET - получение списка всех имеющихся в БД диапазонов дат.

4) https://localhost:5001/api/Date/insert_date_range - PUT - вставка нового диапазона. Только для авторизованных пользователей.

=== Body example: ===

Start:2018-02-23
End:2018-03-08

5) https://localhost:5001/api/Date/get_date_range_intersect - POST - фильтрация по заданию. Только для авторизованных пользователей.

=== Body example: ===

Start:2018-01-01
End:2018-01-02

## Консольное приложение

Навигация по меню реализуется ответом в виде цифры.

Даты для осуществления фильтрации и вставки новых принимаются в стандартных форматах (к примеру: "dd.mm.yyyy" или "yyyy-mm-dd").

Адрес по умолчанию следующий: "https://localhost:5001". Его можно изменить, изменив константу (HOST_ADDRESS) в файле "Requests.cs".

## БД

Состоит из двух таблиц **DateRanges** и **Users** 

== DateRanges ==
ID - int - primary key
Start - datetime
End - datetime

== Users ==
Login - string - primary key 
Password - string



---------------------
Oleksandr Voichuk
sanyabane@gmail.com
---------------------
