# Wstępny opis
Moja pierwsza aplikacja webowa, oparta o MVC. Zawiera podstawowe funkcjonalności do tworzenia i obsługi kont oraz ich uprawnień. W podstronie ```Grupy``` dla użytkownika z uprawnieniami administratora jest wiele opcji do edycji i podsumowanie godzin (zamiana czasu przepracowanego na pieniądze).

## Import przykładowej bazy danych
- wrzuć plik ```MVCFirstApp.bak``` do folderu backup w MSSQL (np. ```C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Backup```)
- w SSMS, PPM na Databases, Restore Database, zaznacz Device, znajdź plik ```MVCFirstApp.bak``` w folderze backup
- w pliku ```appsettings.json``` zmodyfikuj connection string, dla swojej bazy

## Dostęp do poszczególnych stron:
- ```Logowanie i rejestracja:``` bez blokad dostępu
- ```Moje konto:``` zarejestrowani, bez uprawnień
	* ```Historia konta i wypłaty:``` uprawnienia pracownika
- ```Grupy:``` uprawnienia pracownika
	* ```Edycja w grupach:``` uprawnienia administratora
	- ```Sumowanie godzin:``` uprawnienia administratora
- ```Uprawnienia:``` uprawnienia administratora

## Konta testowe ```(login, hasło)```
- bez uprawnień: ```koodziej, 1234```
- uprawnienia pracownika: ```mateosz12, 1234```
- uprawnienia administratora: ```karol348, 1234```

### Jest kilka rzeczy do poprawy, o których się dowiedziałem pod koniec pisania projektu i nie chciałem już wprowadzać zmian min.:
- wyświetlanie i dodawanie grup, aktualnie jest statyczne, bez możliwości edycji w aplikacji
- nie wszystkie błędy Identity są spolszczone
- brak kaskadowego usuwania w EF
