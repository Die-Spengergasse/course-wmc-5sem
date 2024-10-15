# Typescript Basics

TypeScript wurde 2012 von Microsoft entwickelt, um die Entwicklungsprozesse mit JavaScript zu verbessern. Die Hauptmotivation war, die Schwächen von JavaScript bei der Entwicklung großer, skalierbarer Anwendungen zu beheben. JavaScript, ursprünglich für kleine Skripte konzipiert, wurde zunehmend in großen Projekten verwendet, wo die fehlende Typensicherheit und schwache Tools zu Problemen führten. TypeScript führt statische Typisierung ein, was Entwicklern hilft, Fehler frühzeitig zu erkennen und die Codewartung zu erleichtern. Es ist vollständig kompatibel mit JavaScript, wodurch bestehender Code einfach migriert werden kann.

TypeScript ist kein Compiler im klassischen Sinne, da es den Code nicht direkt in maschinenlesbare Form (wie ein typischer Compiler) übersetzt. Stattdessen wandelt es TypeScript-Code in standardkonformes JavaScript um, das von jedem Browser und JavaScript-Laufzeitsystem verstanden wird. Diese Übersetzung hilft Entwicklern, moderne Features und Typensicherheit zu nutzen, während die resultierende Ausgabe weiterhin in jedem JavaScript-Umfeld lauffähig ist. TypeScript agiert also eher als „Transpiler“, der den Quellcode in eine andere Version der gleichen Sprache überführt.

### Die Versionen von Typescript

- **TypeScript 1.0 (2014)**: Die erste stabile Version mit grundlegender Typprüfung und Unterstützung für ES5.
- **TypeScript 2.0 (2016)**: Einführung von Nulltypen (null und undefined), Kontrollflussanalyse und verbesserter Unterstützung von Module Imports.
- **TypeScript 3.0 (2018)**: Tuple-Verbesserungen, Projektreferenzen und erweiterte Generics.
- **TypeScript 4.0 (2020)**: Variadic Tuple Types, Editor-Hilfen und verbesserte Kontrollflussanalyse.
- **TypeScript 5.0 (2023)**: Unterstützung für Dekoratoren und erweiterte ESM-Unterstützung.

## Unterschiede zwischen JavaScript und TypeScript

### JavaScript:
- JavaScript ist eine dynamisch typisierte Sprache, was bedeutet, dass Variablen zur Laufzeit jeden Datentyp annehmen können.
- Es gibt keine Überprüfung von Typen während der Entwicklung, was zu Laufzeitfehlern führen kann.
- Kein direkter Support für Interfaces, Typen oder Klassen während der Entwicklung (obwohl JavaScript ab ES6 Klassen unterstützt).

### TypeScript:
- TypeScript ist eine **statisch typisierte** Obermenge von JavaScript. Das bedeutet, dass der Datentyp von Variablen während der Entwicklungszeit explizit festgelegt wird.
- TypeScript bietet eine **Typüberprüfung** zur Kompilierzeit, um potenzielle Fehler bereits vor der Ausführung zu erkennen.
- Unterstützung für **Interfaces, Enums, Generics**, und andere Features, die die Strukturierung von Code verbessern.
- TypeScript muss in JavaScript transpiliert werden, bevor es in einer Laufzeitumgebung ausgeführt werden kann.

## Die Aufgabe des Transpilers

Ein **Transpiler** (in TypeScript meist der `tsc`, der TypeScript-Compiler) ist dafür zuständig, den TypeScript-Code in regulären JavaScript-Code umzuwandeln, da Browser und Laufzeitumgebungen wie Node.js nativ kein TypeScript ausführen können.

Der TypeScript-Compiler überprüft auch die Typen und gibt während der Kompilierung Hinweise, wenn Typinkonsistenzen oder Fehler gefunden werden. 

Nach der Transpilierung wird JavaScript generiert, das in jeder modernen JavaScript-Umgebung lauffähig ist.

## Datentypen in TypeScript

TypeScript bietet eine Vielzahl von Datentypen, die in verschiedenen Situationen verwendet werden können. Einige der wichtigsten sind:

- **`number`**: Repräsentiert sowohl Ganzzahlen als auch Gleitkommazahlen.
- **`string`**: Repräsentiert Textwerte.
- **`boolean`**: Repräsentiert Wahrheitswerte (true/false).
- **`array`**: Ein Array von Werten eines bestimmten Typs, definiert durch `T[]` oder `Array<T>`.
- **`tuple`**: Ein Array mit einer festen Anzahl und Typen von Elementen.
- **`enum`**: Eine Sammlung von benannten Konstanten.
- **`any`**: Ein Typ, der jede Art von Wert akzeptiert (vermeiden, wenn möglich).
- **`void`**: Wird verwendet, wenn eine Funktion keinen Wert zurückgibt.
- **`null` und `undefined`**: Repräsentieren nicht vorhandene oder undefinierte Werte.
- **`object`**: Repräsentiert nicht-primitive Typen.

```typescript
let age: number = 25;
let name: string = "Alice";
let isDone: boolean = true;
let scores: number[] = [90, 85, 80];
let person: [string, number] = ["John", 30];
enum Color {Red, Green, Blue};
let c: Color = Color.Green;

let randomValue: any = 10;
randomValue = "Hallo";

function logMessage(message: string): void {
    console.log(message);
}

let u: undefined = undefined;
let n: null = null;
let person: object = { name: "John", age: 30 };
```

## Datentypen in Funktionsargumenten und Rückgabetypen

In TypeScript kann man sowohl die **Argumente** als auch den **Rückgabewert** einer Funktion typisieren.

```typescript
function add(a: number, b: number): number {
    return a + b;
}
```

In diesem Beispiel:
- `a` und `b` sind Argumente vom Typ `number`.
- Der Rückgabewert der Funktion ist ebenfalls vom Typ `number`.

Wenn eine Funktion keinen Wert zurückgeben soll, wird der Rückgabetyp mit `void` markiert:

```typescript
function logMessage(message: string): void {
    console.log(message);
}
```

### Bedeutung von unknown und never

#### unknown

*unknown* ist der sicherste Typ in TypeScript, wenn man den Typ einer Variablen nicht kennt. Er zwingt dich dazu, den Typ vor der Verwendung zu überprüfen. Anders als bei any verhindert *unknown*, dass du eine Variable direkt verwendest, ohne vorherige Typprüfung.

```typescript
let value: unknown;

value = "Hello";
value = 42;

if (typeof value === "string") {
    console.log(value.toUpperCase());  // Typprüfung erforderlich
}
```

Hier wird value als unknown deklariert, und es ist notwendig, den Typ mit *typeof* zu überprüfen, bevor der Wert als String verwendet wird.

#### never

*never* ist ein Typ, der verwendet wird, wenn eine Funktion niemals einen Wert zurückgibt oder wenn etwas unmöglich ist. Er tritt auf, wenn eine Funktion entweder eine Endlosschleife ist oder immer einen Fehler wirft.

Beispiel:

```typescript
function throwError(message: string): never {
    throw new Error(message);  // Diese Funktion gibt niemals einen Wert zurück
}

function infiniteLoop(): never
```

### Das type keyword


Das *type*-Keyword in TypeScript wird verwendet, um Alias-Typen zu erstellen. Hier sind einige reale Anwendungsfälle:

**Primitive Typen gruppieren:**
```typescript
type ID = string | number;
let userId: ID = 123;
```

**Funktionssignaturen definieren:**
```typescript
type MathFunction = (a: number, b: number) => number;
const add: MathFunction = (x, y) => x + y;
```

**Unions und Intersections:**

```typescript
type Status = "success" | "error";
type AdminUser = User & { isAdmin: boolean };
```

### Variadic Tuple Types

Variadic Tuple Types in TypeScript, eingeführt mit Version 4.0, ermöglichen es, Tuples flexibler zu gestalten.
Sie erlauben es, eine beliebige Anzahl von Elementen am Ende eines Tuples hinzuzufügen oder zu verarbeiten.
Diese Funktion ist besonders nützlich bei der Arbeit mit Typen, die mehrere Argumente aufnehmen können.

```typescript
type Tuple = [string, ...number[]];
const example: Tuple = ["TypeScript", 1, 2, 3];
```

Hier kann das Tuple mit einem festen string starten, gefolgt von beliebig vielen number-Werten.
Das sorgt für mehr Flexibilität bei der Definition von Typen und Funktionen.

```typescript
function logValues<T extends unknown[]>(...args: [...T, string]) {
    console.log(args);
}

logValues(1, true, "last"); // Gültig
logValues("hello", "world", "last"); // Gültig
```

In diesem Beispiel verlangt die Funktion logValues, dass der letzte Parameter ein string ist, während vorher beliebig viele Werte beliebiger Typen kommen können.


## Typisierte JSON-Objekte mit Interfaces

TypeScript bietet die Möglichkeit, **Interfaces** zu definieren, die eine Struktur von Objekten beschreiben. Dies ist besonders nützlich, wenn man mit JSON-Daten arbeitet.

Beispiel für ein einfach typisiertes JSON-Objekt:

```typescript
interface Person {
    name: string;
    age: number;
    email?: string; // Das Fragezeichen bedeutet, dass dieses Feld optional ist
}

const personData: Person = {
    name: "John Doe",
    age: 30
};
```

> Hinweis: Wir können auch mit dem *type* keyword den Typ für Person erstellen.
> Da Person aber ideomatisch für ein Objekt steht, verwenden wir hier ein Interface, obwohl wir
> nicht davon ableiten und es erweitern.

### Verwendung von Mehrfachtypen (Union Types) im Interface

In TypeScript ist es möglich, dass ein Feld in einem Interface mehr als einen Typ haben kann. Dazu verwendet man das `|`-Symbol, das sogenannte **Union Types** erstellt. Damit kann ein Feld beispielsweise entweder vom Typ `string` oder `number` sein.

#### Beispiel:
```typescript
interface FlexiblePerson {
    id: string | number;  // Kann entweder eine ID als string oder number sein
    name: string;
    age: number | null;    // Alter kann entweder eine Zahl oder null sein
    active: boolean | string;  // Kann als boolean oder als string angegeben werden
}

const examplePerson: FlexiblePerson = {
    id: 12345,
    name: "Jane Doe",
    age: null,
    active: "yes"
};
```

In diesem Beispiel kann `id` sowohl ein `string` als auch eine `number` sein. `age` kann entweder eine Zahl oder `null` sein, und `active` kann entweder ein `boolean` oder ein `string` sein.

Diese Flexibilität ist nützlich, wenn man mit Daten arbeitet, die aus verschiedenen Quellen stammen oder unterschiedliche Formate haben könnten.

### Verschachtelte JSON-Objekte


Manchmal enthalten JSON-Objekte andere Objekte oder Arrays. Auch diese können mit Interfaces typisiert werden:

```typescript
interface Address {
    street: string;
    city: string;
}

interface PersonWithAddress {
    name: string;
    age: number;
    address: Address;  // Ein verschachteltes Objekt
}

/* Alternative in einem Interface:
interface PersonWithAddress {
    name: string;
    age: number;
    address: {
        street: string;
        city: string;
    };
}
*/

const personWithAddress: PersonWithAddress = {
    name: "John Doe",
    age: 30,
    address: {
        street: "123 Main St",
        city: "Springfield"
    }
};
```


### Funktionen, die Interfaces zurückgeben

Man kann in TypeScript auch Funktionen definieren, die ein bestimmtes Interface zurückgeben. Das ist besonders nützlich, wenn man sicherstellen möchte, dass das Rückgabeobjekt einer bestimmten Struktur entspricht.

Hier ist ein Beispiel, bei dem eine Funktion ein `Person`-Interface zurückgibt:

```typescript
interface Person {
    name: string;
    age: number;
    email?: string;
}

function createPerson(name: string, age: number): Person {
    return {
        name: name,
        age: age,
        email: undefined // Optionales Feld
    };
}

const newPerson = createPerson("John", 30);
```

In diesem Beispiel gibt die Funktion `createPerson` ein Objekt zurück, das der Struktur des `Person`-Interfaces entspricht. Dies stellt sicher, dass das zurückgegebene Objekt immer die Eigenschaften `name` und `age` enthält und optional `email`.

### Arrays in Interfaces


Man kann auch Arrays von Objekten in einem Interface typisieren:

```typescript
interface PersonWithHobbies {
    name: string;
    age: number;
    hobbies: string[];  // Ein Array von Strings
}

const personWithHobbies: PersonWithHobbies = {
    name: "Jane Doe",
    age: 25,
    hobbies: ["Reading", "Traveling", "Sports"]
};
```

Für komplexere Szenarien mit verschachtelten Arrays und Objekten:

```typescript
interface Company {
    name: string;
    employees: {
        name: string;
        age: number;
        address: {
            street: string;
            city: string;
        };
    }[];
}

const companyData: Company = {
    name: "Tech Corp",
    employees: [
        {
            name: "John Doe",
            age: 30,
            address: {
                street: "123 Main St",
                city: "Springfield"
            }
        },
        {
            name: "Jane Doe",
            age: 25,
            address: {
                street: "456 Elm St",
                city: "Shelbyville"
            }
        }
    ]
};
```

In diesem Beispiel enthält das `Company`-Interface ein Array von `PersonWithAddress`-Objekten, wodurch die Struktur des JSON-Datensatzes genau definiert ist.

## Übungsbeispiel: Wahlsystem in Österreich

Am 29. September 2024 fand in Österreich eine Wahl statt. Deine Aufgabe ist es, ein Wahlsystem in TypeScript zu entwickeln, das die Wählerdaten, Parteien und abgegebenen Stimmen verwaltet. In Österreich gibt es mehrere Parteien, und jeder Wähler darf eine Stimme abgeben. Deine Aufgabe ist es, ein Programm zu schreiben, das Parteien, Wähler und die abgegebenen Stimmen strukturiert und typisiert verwaltet.

Das Programm soll:

- Die verschiedenen Parteien in einem Enum definieren.
- Die Wähler in einer Datenstruktur erfassen.
- Eine Funktion zur Stimmabgabe bereitstellen, die sicherstellt, dass jeder Wähler nur einmal abstimmen kann.
- Die Stimmabgabe und Wählerinformationen überprüfen und protokollieren.

Die zur Wahl stehenden Parteien sollen in einem Enum erfasst werden. Definiere ein Enum Party, das die folgenden Parteien enthält: *OEVP, SPOE, FPOE, Gruene, NEOS*.

Du sollst nun die Daten der Wähler erfassen. Jeder Wähler hat einen Namen, eine eindeutige ID und eine mögliche abgegebene Stimme.

Definiere ein Interface Voter, das die folgenden Eigenschaften enthält:
- id: Eine eindeutige ID (number).
- name: Der Name des Wählers (string).
- vote: Die gewählte Partei aus dem Enum Party (optional, da der Wähler noch nicht abgestimmt haben könnte).

Implementiere eine Funktion *createVoter*, die einen neuen Wähler erstellt.
Jeder Wähler darf nur einmal abstimmen. Die Funktion *vote* nimmt die ID des Wählers und die gewählte Partei entgegen und aktualisiert den Wählerdatensatz, indem sie die abgegebene Stimme speichert.
Am Ende sollen die Ergebnisse der Wahl zusammenfassen. Dafür erhältst du eine Liste von Wählern, und die Funktion *calculateResults* soll die Anzahl der Stimmen pro Partei berechnen und ausgeben.

Erstelle für die Übung ein Verzeichnis *typescript_vote* und lege wie in [Die erste Typescript App](10_FirstApp.md) beschrieben eine leere Typescript App an.
Verwende dann die untenstehende *app.ts* Datei, um deine Implementierung vorzunehmen.

**src/app.ts**
```typescript
// 1. Definiere das Enum 'Party' für die verschiedenen Parteien


// 2. Definiere das Interface 'Voter' für Wählerinformationen


// 3. Funktion createVoter zur Erstellung eines neuen Wählers
// Die Funktion soll eine id (number) und name (string) bekommen und einen Voter zurückgeben

// 4. Funktion vote zur Stimmabgabe durch einen Wähler
// Die Funktion soll einen voter (Typ Voter) und eine party (Typ Party) bekommen und die
// Partei beim Voter setzen. Das darf er allerdings nur, wenn der Voter nicht schon abgestimmt hat.

// 5. Funktion calculateResults zur Berechnung der Wahlergebnisse.
//    Die Funktion bekommt ein Array von voters und gibt einen Record mit dem Key der Patei und der Anzahl als Wert zurück.
//    Lies nach, wie ein Record in Typescript definiert ist und wie er erstellt werden kann.
//    Hinweis: Erstelle zuerst einen Record mit allen Parteien mit der Anzahl 0 und verwende dann reduce.

// =============== TESTS (nicht verändern) ===============

// Test-Wähler erstellen
let voter1 = createVoter(1, "Max Mustermann");
let voter2 = createVoter(2, "Anna Musterfrau");
let voter3 = createVoter(3, "Hans Testmann");

// Stimmen abgeben
vote(voter1, Party.OEVP);
vote(voter2, Party.Gruene);
vote(voter3, Party.FPOE);
vote(voter1, Party.SPOE); // Sollte nichts zuweisen.

// Wahlergebnisse berechnen und ausgeben
const result = calculateResults([voter1, voter2, voter3]);
console.log(result);
```

