# Object und Array destructing

## Object destructuring

Object Destructuring (Objekt-Dekonstruktion) ist eine Syntax, die es ermöglicht, Werte aus einem Objekt herauszuziehen und sie direkt Variablen zuzuweisen.
Es spart Schreibaufwand und verbessert die Lesbarkeit des Codes.

Beispiel:
```typescript
// Ein Objekt mit mehreren Eigenschaften
const user = {
  name: "Alice",
  age: 30,
  email: "alice@example.com"
};

// Destructuring Syntax, um Eigenschaften des Objekts Variablen zuzuordnen
const { name, age } = user;

console.log(name); // "Alice"
console.log(age);  // 30
```

Hier wird das Objekt user dekonstruiert, und die Eigenschaften name und age werden direkt in Variablen mit demselben Namen extrahiert.
Es kann natürlich auch mit definierten Typen in TypeScript verwendet werden:

```typescript
interface User {
  name: string;
  age: number;
  email: string;
}

const user: User = {
  name: "Alice",
  age: 30,
  email: "alice@example.com"
};

const { name, age }: { name: string; age: number } = user;

console.log(name); // "Alice"
console.log(age);  // 30
```

Hier fügt TypeScript Typen hinzu, um die Variablen name und age explizit zu typisieren.

### Dekonstruktion von verschachtelten Objekten

```typescript
const user = {
  name: "Alice",
  address: {
    city: "Berlin",
    zip: "12345"
  }
};

// Dekonstruktion eines verschachtelten Objekts
const { address: { city, zip } } = user;

console.log(city); // "Berlin"
console.log(zip);  // "12345"
```

In diesem Fall wird das verschachtelte Objekt address weiter dekonstruiert, um die Variablen city und zip zuzuordnen.

### Object destructuring in Funktionsparametern

#### Beispiel

```typescript
interface User {
  name: string;
  age: number;
  email: string;
}

function greetUser({ name, age }: User): void {
  console.log(`Hello, ${name}! You are ${age} years old.`);
}

const user = {
  name: "Alice",
  age: 30,
  email: "alice@example.com"
};

greetUser(user); // Ausgabe: "Hello, Alice! You are 30 years old."
```

###  Verwenden von Standardwerten mit Object Destructuring

```typescript
interface Config {
  theme?: string;
  fontSize?: number;
}

function applySettings({ theme = "light", fontSize = 12 }: Config): void {
  console.log(`Theme: ${theme}, Font size: ${fontSize}`);
}

applySettings({}); // Ausgabe: "Theme: light, Font size: 12"
applySettings({ theme: "dark" }); // Ausgabe: "Theme: dark, Font size: 12"
```

Erklärung:
Die Funktion applySettings nimmt ein Objekt des Typs Config als Argument.
Durch Object Destructuring werden die Eigenschaften theme und fontSize extrahiert.
Standardwerte (*theme = "light", fontSize = 12*) werden verwendet, falls diese Werte nicht im Objekt vorhanden sind.
Vorteil: Standardwerte machen den Code robuster, weil die Funktion auch dann funktioniert, wenn bestimmte Werte nicht übergeben werden.

#### Erklärung

Die Funktion greetUser nimmt als Parameter ein Objekt des Typs User.
Durch Object Destructuring wird das Objekt im Funktionskopf direkt in die Eigenschaften name und age zerlegt.
Die Funktion verwendet diese Eigenschaften, um eine Nachricht zu erstellen.
Vorteil: Anstatt innerhalb der Funktion immer auf user.name oder user.age zuzugreifen, kannst du direkt die zerlegten Werte nutzen. Es ist kürzer und sauberer.

## Spread-Syntax

Die Spread-Syntax wird verwendet, um alle Elemente oder Eigenschaften eines Objekts oder Arrays "aufzubreiten" bzw. zu kopieren.
Sie ermöglicht es, Arrays oder Objekte auf einfache Weise zu kopieren oder zusammenzuführen.

### Beispiel mit Objekten

```typescript
const user = {
  name: "Alice",
  age: 30
};

// Kopieren eines Objekts mit Spread-Syntax
const userCopy = { ...user };

console.log(userCopy); // { name: "Alice", age: 30 }

// Kombinieren von Objekten mit Spread-Syntax
const updatedUser = { ...user, email: "alice@example.com" };

console.log(updatedUser); 
// { name: "Alice", age: 30, email: "alice@example.com" }
```

Hier wird das Objekt user mit der Spread-Syntax kopiert (*{ ...user }*).
Im zweiten Beispiel wird ein neues Objekt updatedUser erstellt, das die Eigenschaften des ursprünglichen user-Objekts enthält und zusätzlich eine neue Eigenschaft email erhält.

### Spread-Syntax mit Arrays

```typescript
const arr1 = [1, 2, 3];
const arr2 = [4, 5, 6];

// Kopieren eines Arrays
const arrCopy = [...arr1];

console.log(arrCopy); // [1, 2, 3]

// Zusammenfügen von Arrays
const combinedArray = [...arr1, ...arr2];

console.log(combinedArray); // [1, 2, 3, 4, 5, 6]
```

Die Spread-Syntax wird verwendet, um ein Array zu kopieren oder Arrays zusammenzuführen. In diesem Fall werden arr1 und arr2 zu einem neuen Array combinedArray kombiniert.

#### Typing in TypeScript

```typescript
interface User {
  name: string;
  age: number;
  email?: string;
}

const user: User = {
  name: "Alice",
  age: 30
};

// Durch Spread Syntax ein neues Objekt erstellen
const updatedUser: User = { ...user, email: "alice@example.com" };

console.log(updatedUser); 
// { name: "Alice", age: 30, email: "alice@example.com" }
```

In TypeScript können wir sicherstellen, dass das Ergebnis der Spread-Syntax den korrekten Typ hat, wie in diesem Fall der Typ User.

## Array destructuring

Mit Array Destructuring kannst du Werte aus einem Array extrahieren und sie direkt Variablen zuweisen, basierend auf ihrer Position im Array.
Einfaches Beispiel:

```typescript
const numbers = [1, 2, 3];

// Array Destructuring
const [first, second, third] = numbers;

console.log(first);  // 1
console.log(second); // 2
console.log(third);  // 3
```

Hier werden die ersten drei Werte des Arrays numbers extrahiert und den Variablen first, second und third zugewiesen. Die Zuweisung erfolgt basierend auf der Reihenfolge der Werte im Array.

### Überspringen von Werten:

Wenn du nur bestimmte Werte aus einem Array extrahieren willst, kannst du Werte überspringen, indem du einfach leere Kommas verwendest:

```typescript
const numbers = [1, 2, 3, 4];

// Nur den ersten und vierten Wert extrahieren
const [first, , , fourth] = numbers;

console.log(first);  // 1
console.log(fourth); // 4
```

In diesem Beispiel wird der zweite und dritte Wert des Arrays ignoriert, und nur der erste (first) und der vierte (fourth) Wert werden extrahiert.
Du kannst auch Standardwerte definieren, falls das Array nicht genug Werte enthält:

```typescript
const numbers = [1];

// Destructuring mit Standardwerten
const [first, second = 10] = numbers;

console.log(first);  // 1
console.log(second); // 10 (Standardwert, da das Array nur ein Element hat)
```

In diesem Fall hat das Array numbers nur ein Element, daher wird für die Variable second der Standardwert 10 verwendet.
Ähnlich wie bei Objekten kannst du den Rest-Operator (...) verwenden, um den Rest des Arrays in eine Variable zu packen:

```typescript
const numbers = [1, 2, 3, 4, 5];

// Den ersten Wert extrahieren und den Rest in `rest` speichern
const [first, ...rest] = numbers;

console.log(first);  // 1
console.log(rest);   // [2, 3, 4, 5]
```

In diesem Beispiel wird der erste Wert des Arrays in first gespeichert, und der Rest der Werte ([2, 3, 4, 5]) wird in das Array rest gepackt.

### Array Destructuring in Funktionsparametern

Du kannst Array Destructuring auch in **Funktionsargumenten** verwenden:

```typescript
function printCoordinates([x, y]: [number, number]) {
  console.log(`X: ${x}, Y: ${y}`);
}

const coordinates: [number, number] = [10, 20];
printCoordinates(coordinates); // X: 10, Y: 20
```

In diesem Beispiel erwartet die Funktion *printCoordinates* ein Array mit zwei Werten, und diese Werte werden direkt in die Parameter x und y dekonstruiert.

### Rest-Operator mit Array Destructuring

```typescript
function printFirstAndRest([first, ...rest]: number[]): void {
  console.log(`First number: ${first}`);
  console.log(`Rest of the numbers: ${rest}`);
}

const numbers = [1, 2, 3, 4, 5];
printFirstAndRest(numbers);
// Ausgabe:
// First number: 1
// Rest of the numbers: [2, 3, 4, 5]
```

Erklärung:
Die Funktion *printFirstAndRest* nimmt ein Array von Zahlen als Parameter.
Durch Array Destructuring wird das erste Element (*first*) aus dem Array extrahiert.
Der Rest-Operator (*...rest*) packt alle restlichen Elemente des Arrays in das Array rest.
Vorteil: Du kannst das erste Element separat behandeln und gleichzeitig den Rest des Arrays als separate Einheit behalten.

### Kombinieren von Object Destructuring und Array Destructuring

```typescript
interface User {
  name: string;
  hobbies: string[];
}

function printUserDetails({ name, hobbies }: User): void {
  const [firstHobby, ...otherHobbies] = hobbies;
  console.log(`Name: ${name}`);
  console.log(`First hobby: ${firstHobby}`);
  console.log(`Other hobbies: ${otherHobbies}`);
}

const user = {
  name: "Bob",
  hobbies: ["Reading", "Swimming", "Cycling"]
};

printUserDetails(user);
// Ausgabe:
// Name: Bob
// First hobby: Reading
// Other hobbies: [ 'Swimming', 'Cycling' ]
```

Erklärung:
Die Funktion *printUserDetails* verwendet Object Destructuring, um die Eigenschaften name und hobbies aus dem User-Objekt zu extrahieren.
Anschließend wird Array Destructuring verwendet, um das erste Hobby (*firstHobby*) und die restlichen Hobbys (*otherHobbies*) aus dem Array hobbies zu extrahieren.
Vorteil: Durch die Kombination von Object und Array Destructuring kannst du komplexe Datenstrukturen effizient verarbeiten und die verschiedenen Teile separat behandeln.

### Zusammenfassung

Array Destructuring ermöglicht es, Werte aus einem Array basierend auf ihrer Position zu extrahieren.
Du kannst Werte überspringen, Standardwerte setzen und den Rest-Operator verwenden, um den Rest des Arrays zu erfassen.
Es funktioniert ähnlich wie Object Destructuring, ist aber positionsbasiert.
Das macht den Code oft lesbarer und effizienter, besonders bei der Arbeit mit Arrays und Tupeln in TypeScript.

