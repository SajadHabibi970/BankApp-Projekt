# Bankkonto - Blazor WebAssembly

Det är en liten bankapp byggd i Blazor WebAssembly.
Man kan skapa konton, göra transaktioner och se en historik över allt som händer.
Alla data sparas i webbläsaren med LocalStorage så den ligger kvar mellan laddningar.

___

## Funktioner

### Konton
- Skapa olika konton (namn, typ, valuta, startsaldo)
- Se en lista på alla konton
- Se senaste uppdatering
- Möjlighet att lägga på ränta på sparkonton

### Transaktioner
- Insättning
- Uttag
- Överföring mellan konton
- Felmeddelanden om något inte går (t.ex för lite saldo)
- Varje händelse loggas som en transaktion

### Historik
- Komplett lista över transaktioner
- Filtrering på konto
- Sortering på datum och belopp
- Färgkodade händelser (grönt för in, rött för ut)
- Datumintervall-filter

### Lagring
- LocalStorage för konton och transaktioner
- Export av alla data till JSON (kopierar till urklipp)
- Import av JSON (återställer allt)

### Lösenordslås
- En enkel lösenordssida före appen startar
- Menyn döljs tills lösenordet är godkänt
- Krävs vid varje omladdning

___

## Krav (G) som projektet uppfyller

- Domänklasser : BankAccount, Transaction, AccountType, TransactionType
- Interface för konton, transaktioner, och lagring
- Services : AccountService, TransactionService, StorageService
- Dependency Injection i hela projektet
- Sidor för konto, transaktioner och historik
- Funktioner för : skapa konto, insättning, uttag och överföring
- Transaktionshistorik med sortering och filtrering
- LocalStorage sparar alla data
- Navigation via routing i Blazor

___

## VG-delar

1. Ränta på sparkonton
En knapp lägger på 1 % ränta automatisk
Varje ränta registreras som en egen transaktion

2. Export och import av JSON
Exportknapp kopierar alla konton + transaktioner som JSON
Import läser in JSON och återställer alla objekt

3. Lösenordssida (UI-lock)
Appen är låst innan man skriver lösenordet
Man kan inte navigera till andra sidor utan att låsa upp
Sidomenyn döljs automatiskt tills appen är upplåst

___

## Teknik och struktur

### Domänklasser
- BankAccount - håller info om kontot och innehåller metoder för insättning och uttag
- Transaction - sparar info om varje händelse
- AccountType - enum för kontotyper
- TransactionType - enum för olika transaktiontyper

### Interfaces
- IAccountService - hur kontohanteringen ska fungera
- ITransactionService - hur transaktioner sparas och hämtas
- IStorageService - all kommunikation med LocalStorage
- IBankAccount - gemensamma kontrakt för domänklasserna
- ITransaction - gemensamma kontrakt för domänklasserna

### Services
- AccountService - skapar konton, hanterar saldo, ränta osv
- TransactionService - sparar och laddar alla transaktioner
- StorageService - sparar och läser JSON i LocalStorage
- DataExportService - export och import av alla data

### Sidor
- CreateAccount - formulär för att skapa nya konton
- Accounts - visar alla konton
- Transaction - där man gör insättningar, uttag och överföringar
- History - tabell med alla händelser + filter
- Password - lösenordssidan som visas först
- MainLayout - min layout som också kollar om lösenordet är godkänt.

___

## Så kör man det

1. Öppna projektet i Rider eller Visual Studio
2. Kör som Blazor WebAssembly
3. Appen startar i webbläsaren
4. Skriv in PIN-koden : (1234)

___

## Sammanfattning

Appen hanterar konton, transaktioner och historik på ett enkelt sätt.
All data sparas lokalt och det finns både export och import av JSON och ett lösenordslås innan man kommer in.
Jag har försökt hålla koden tydlig, dela upp allt i services och få UI:t så smidigt som möjligt.
