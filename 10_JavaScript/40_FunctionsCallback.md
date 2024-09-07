# Funktionen 1: Callbacks und Closures

## Definition einer Funktion

Funktionen können mit den Schlüsselwort *function* definiert werden. Oberflächlich betrachtet
erscheint an der nachfolgenden Deklaration nichts ungewöhnliches:
```javascript
function add(x, y) {
    return x + y;
}

console.log(add(1, 2));    // 3
```

Beim genaueren Hinsehen erkennen wir allerdings Unterschiede zu anderen Sprachen mit static
Typing wie Java oder C#.
- Es wird kein Typ für den Rückgabewert definiert. Ob eine Funktion etwas zurückliefert oder nicht
  ist nicht erkennbar. Es gibt keine expliziten *void* Funktionen.
- x und y haben keinen definierten Datentyp.

Der nachfolgende Code wird ohne Fehler ausgeführt, liefert aber ungewöhnliche Ergebnisse:

```javascript
console.log(add(1, "2"));       // 12
console.log(add(1, 2, 3, 4));   // 3, no runtime exception
console.log(add(1));            // no runtime exception, result is NaN
console.log(add());             // no runtime exception, result is NaN
```

Wir können uns bei einer Funktion nicht darauf verlassen, dass der Aufrufer numerische Werte oder
überhaupt Werte angibt. Daher ist es oft notwendig, die Argumente zu prüfen. Mit dem *typeof*
Operator kann der Typ bestimmt werden. Fehlende Argumente haben den typ *undefined*.

Wir erkennen auch, dass die Funktion einmal nichts (im Fehlerfall) und einmal einen numerischen
Wert zurückgeben kann.

```javascript
function add(x, y) {
    if (typeof x !== "number") { return; }
    if (typeof y !== "number") { return; }
    return x + y;
}

console.log(add(1, 2));     // 3
console.log(add(1, "2"));  // undefined
console.log(add(1));       // undefined
console.log(add());        // undefined
```

Möchten wir die Argumente wenn möglich umwandeln, können die Umwandlungsfunktionen wir *Number()*
oder *Boolean()* in ECMAScript verwendet werden. Das Ergebnis im Fehlerfall ist NaN. Es ist ein
spezieller numerischer Wert. Mit *isFinite()* kann geprüft werden, ob eine "echte" Zahl zurückgegeben
wurde.

```javascript
function add(x, y) {
    x = Number(x);
    y = Number(y);
    return x + y;
}

const result = add("A", 1);
if (!isFinite(result)) {
    console.log("Invalid number.")
}
```

## Funktionen als Objekte

Etwas seltsam wirkt die nachfolgende Definition der add Funktion. Sie wird nun ohne Namen definiert,
aber in eine Variable geschrieben. In JavaScript sind Funktionen nichts anderes als Objekte, die
über eine Referenz gespeichert werden können. Bei der Definition erfolgt noch kein Aufruf. Erst
beim Aufruf von *add()* wird die Funktion durchlaufen.

```javascript
const add = function (x, y) {
    x = Number(x);
    y = Number(y);
    return x + y;
};

console.log(typeof add);     // "function"
console.log(add(1, 2));      // 3
```

### Anwendung: Callback Funktion

Wenn Funktionen nichts anderes als Objekte sind, dann können wir sie auch einfach als Argument
übergeben. Das nachfolgende Beispiel zeigt ein sehr fundamentales Konzept in JavaScript: Callback
Funktionen. 

Unsere *add()* Funktion soll im Fehlerfall Aktionen durchführen. Anfänger würden das Fehlerverhalten
direkt in die Funktion implementieren:

```javascript
function add(x, y, logger) {
    x = Number(x);
    y = Number(y);
    const result = x + y;
    if (!isFinite(result)) {
        console.log("Invalid arguments.");
    }
    return result;
};
```

Wenn nun ein anderer Aufbau des Logeintrages erforderlich ist, müssten wir die add Funktion ändern.
Dies widerspricht den sogenannten *open–closed principle*: 
*"software entities (classes, modules, functions, etc.) should be open for extension, but closed
for modification"*.

Das Logging hat nichts mit der Aufgabe der Funktion zu tun, daher übergeben wir einfach eine
Logmethode. Natürlich sollte geprüft werden, ob das Argument eine Funktion ist. Falls nicht,
weisen wir eine leere Funktion zu, damit der Aufruf des Loggers in add keinen Laufzeitfehler verursacht.

```javascript
function consoleLogger(message) {
    console.log(`${message}`);
}

function dateLogger(message) {
    console.log(`${new Date().toISOString()} ${message}`);
}

function add(x, y, logger) {
    x = Number(x);
    y = Number(y);
    logger = typeof (logger) === "function" ? logger : function () { };
    const result = x + y;
    if (!isFinite(result)) {
        logger("Invalid arguments.");
    }
    return result;
};

const result = add(1, "x", consoleLogger);  // Invalid arguments.
const result2 = add(1, "x", dateLogger);    // 2021-01-14T10:36:03.864Z Invalid arguments.
const result3 = add(1, "x", "a string");    // no logging, but no runtine exception
```

## Closures

Mit dem Wissen, dass Funktionen lediglich Objekte sind, können wir natürlich Funktionen auch als
Rückgabewert verwenden. Diese zurückgegebenen Funktionen haben eine Besonderheit: Sie können auf
Variablen, die in der übergeordneten Funktion definiert sind, zugreifen. Somit ist folgendes
Beispiel umsetzbar:

```javascript
function getAgeCalculator(now) {
    now = new Date(now);             // now can be accessed by the returned function.
    return function (date) {
        return (now - new Date(date)) / 86_400_000;
    };
};

const ageCalculator = getAgeCalculator("2021-12-15");
console.log(ageCalculator("2021-12-11"));  // 4
console.log(ageCalculator("2021-12-10"));  // 5
```

Natürlich können wir auch Funktionen als Properties eines JSON Objektes definieren. Somit können
wir mehrere Methoden zurückgeben, die auf *now* zugreifen können.

```javascript
function getAgeCalculator(now) {
    now = new Date(now);
    return {
        calculateAge(date) {
            return (now - new Date(date)) / 86_400_000;
        },
        isFullAged(date) {
            date = new Date(date);
            date.setFullYear(date.getFullYear() + 18);
            return now > date;
        }
    };
};

const ageCalculator = getAgeCalculator("2021-12-15");
console.log(ageCalculator.calculateAge("2002-12-10"));  // 6945
console.log(ageCalculator.isFullAged("2002-12-10"));    // true
console.log(ageCalculator.isFullAged("2004-12-10"));    // false
```

## Übung 1: Das Publish/Subscribe Pattern

Durch den Umstand, Funktionen auch in einem Array speichern zu können, kann ein Publish/Subscribe
Pattern leicht implementiert werden. Komponenten können durch eine Subscribe Methode eine Callback
Funktion übergeben. Diese Funktion wird aufgerufen, wenn mit *publish()* Daten gesendet werden.
Implementiere die Funktion *messageBus()*, sodass sie dieses Feature besitzt. 

### Aufgabe

1. Erstelle eine neue Datei mit dem Namen *functions_exercise01.js* und öffne sie in VS Code. Kopiere
   danach den untenstehenden Code hinein. Öffne danach die Konsole in VS Code und führe mit
   *node functions_exercise01.js* die Datei aus. Die Ausgabe muss exakt der vorgegebenen Ausgabe
   entsprechen.
2. Es ist nur Code innerhalb der Funktion *messageBus()* zu schreiben. Der Rest darf nicht
   verändert werden.

Beachte folgende Hinweise:
- Verwende intern ein Array, welches die subscriptions speichert. Mit *push()* können Elemente zu
  einem Array hinzugefügt werden.
- Überlege dir, wie die Funmtion *messageBus()* ein Objekt, welches 3 Methoden (nämlich *subscribe()*, *publish()*
  und *unsubscribe()* hat), zurückgeben kann. Beachte, dass das Objekt auf die Variablen in der Funktion zugreifen
  kann (closures).
- Als id kannst du den eingefügten Index in *subscribe()* zurückgeben. Das macht das Löschen in *unsubscribe()*
  besonders einfach.
- Du kannst mit *splice(index, 1)* ein Element in einem Array an der Position *index* löschen.

**functions_exercise01.js**
```javascript
function messageBus() {
    /* Your implementation */
}

function subscriber1(data) {
    console.log(`Subscriber1 1 received ${data}`);
}
function subscriber2(data) {
    console.log(`Subscriber1 2 received ${data}`);
}

const bus = messageBus();
bus.subscribe(subscriber1);
const id = bus.subscribe(subscriber2);
bus.publish("Hello!");
bus.unsubscribe(id);
bus.publish("Hello again!");
```

**Ausgabe**
```text
Subscriber 1 received Hello!
Subscriber 2 received Hello!
Subscriber 1 received Hello again!
```

## Übung 2: Publish/Subscribe Pattern mit Eventtyp

Im vorigen Beispiel empfing jede registrierte Callback Funktion die Daten. Nun soll anhand eines
Typs differenziert werden können. 

### Aufgabe

1. Erstelle eine neue Datei mit dem Namen *functions_exercise02.js* und öffne sie in VS Code. Kopiere
   danach den untenstehenden Code hinein. Öffne danach die Konsole in VS Code und führe mit
   *node functions_exercise02.js* die Datei aus. Die Ausgabe muss exakt der vorgegebenen Ausgabe
   entsprechen.
2. Es ist nur Code innerhalb der Funktion *messageBus()* zu schreiben. Der Rest darf nicht
   verändert werden.

Beachte dabei die folgenden Hinweise:
- Es ist weiterhin nur ein Array zur Speicherung der Callback Funktionen notwendig.
- Füge in der Funktion *on()* ein JSON Object mit den Properties *type* und *callback* hinzu.
- Rufe in der Funktion *publish()* nur die Callback Funktionen auf, die den richtigen subscription
  Typ haben.

**functions_exercise02.js**
```javascript
function messageBus() {
    /* Your implementation */
}

function subscriber1Click(data) {
    console.log(`Subscriber 1 received x = ${data.x} and y = ${data.y}.`);
}
function subscriber2Click(data) {
    console.log(`Subscriber 2 received x = ${data.x} and y = ${data.y}.`);
}
function subscriber3Keydown(data) {
    console.log(`Subscriber 3 received key ${data}.`);
}

const bus = messageBus();
bus.on("click", subscriber1Click);
const id = bus.on("click", subscriber2Click);
bus.on("keydown", subscriber3Keydown);

bus.publish("click", { x: 10, y: 20 });
bus.publish("keydown", "h");

bus.unsubscribe(id);
bus.publish("click", { x: 30, y: 40 });
```

**Ausgabe**
```text
Subscriber 1 received x = 10 and y = 20.
Subscriber 2 received x = 10 and y = 20.
Subscriber 3 received key h.
Subscriber 1 received x = 30 and y = 40.
```
