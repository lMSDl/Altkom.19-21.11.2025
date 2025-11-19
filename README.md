## CLI
* Nowy projekt
    * konsolowy
    ```
    dotnet new console [-o <LOKALIZACJA> -n <NAZWA_PROEKTU>]
    ```    
    * WebAPI
    ```
    dotnet new webapi [-o <LOKALIZACJA> -n <NAZWA_PROEKTU>] [--no-https]
    ```
    * biblioteki
    ```
    dotnet new classlib [-o <LOKALIZACJA> -n <NAZWA_PROEKTU>]
    ```
* Kompilacja i uruchomienie
    ```
    dotnet build
    ```
    ```
    dotnet [watch] run [PARAMETRY]
    dotnet <NAZWA_PROJEKTU>.dll [<PARAMETRY>]
    ```
* Pakiety i referencje
    * Dodawanie referencji
    ```
    dotnet add reference <ŚCIEŻKA_PROJEKTU>
    ```
    * Dodawanie pakietów
    ```
    dotnet add package <NAZWA_PAKIETU>
    ```
    * Pobranie pakietów
    ```
    dotnet restore
    ```
