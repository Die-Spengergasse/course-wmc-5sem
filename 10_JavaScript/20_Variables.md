# Variablen und Datentypen

## Deklaration

In Sprachen wie Java oder C#, die static Typing umsetzen, erfolgt die Deklaration einer Variable
stets mit dem Datentyp. Auch das Schlüsselwort *var*, welches in Java oder C# existiert, legt
den Typ fest. Nur wird dieser Typ vom Compiler bestimmt.

JavaScript ist "weakly typed" oder "untyped". JavaScript legt den internen Typ anhand der Werte
fest, die sich in der Variable befinden. Diese können jederzeit verändert werden.

Früher wurde mit dem Schlüsselwort *var* eine Variable deklariert:

```javascript
var i = 0;
i = false;   // not possible in Java or C#
```

**Diese Art der Deklaration wird heute nicht mehr verwendet!** Betrachten wir folgenden Code:

```javascript
console.log(j);                // undefined, but no runtime exception
for(var i = 0; i < 10; i++) {
    var j = i;
}
console.log(j);                // 9
```

Die Ausgabe von 9 verwundert, denn die Variable wurde innerhalb der Schleife deklariert. Also müsste
sie nur dort gültig sein. Bei *var* führt JavaScript sogenanntes *hoisting*, die Deklaration wird
quasi an die Spitze des Codes gestellt. Dadurch können wir j auch vor der Schleife ausgeben.

Gerade letzteres - nämlich das Zurückgeben von *undefined* ohne Laufzeitfehler - ist kritisch. Deswegen
wurde im ECMAScript 6 Standard nachgeschärft und 2 Schlüsselworte wurden eingeführt: *let* und *const*.

*let* definiert veränderbare Variablen, die allerdings nur - wie erwartet - im Block, wo sie
deklariert wurden, zugreifbar sind.

```javascript
console.log(j);                // ReferenceError: j is not defined
for(let i = 0; i < 10; i++) {
    let j = i;
}
console.log(j);                // ReferenceError: j is not defined
```

Wenn Variablen nicht verändert werden sollen, dann können wir *const* verwenden. Eine neue Zuweisung
zu *step* würde einen Laufzeitfehler verursachen. *const* braucht einen Initialwert.
```javascript
const step = 10;
for(let i = 0; i < 100; i += step) {
    console.log(i);           // 0, 10, 20, ..., 90
}
step++;                       // TypeError: Assignment to constant variable.
```

## Interne Datentypen

ECMAScript definiert folgende Datentypen, die in Variablen und Literalen (fixe Werte im Code)
auftreten können:<sup>vgl. https://262.ecma-international.org/6.0/#sec-ecmascript-language-types</sup>
- Undefined
- Null
- Boolean
- String
- Number
- Object

Der Typ *Undefined* ist für Variablen, die keinen zugewiesenen Wert haben.

### Typeof

Oft wollen wir bestimmen, welcher Typ konkret als Wert der Variable vorliegt. Der *typeof*
Operator liefert den Typ als String zurück. Beachte, dass der Typ von *null* als *object*
geliefert wird.

| Type of val                                       | Result      |
| ------------------------------------------------- | ----------- |
| Undefined                                         | "undefined" |
| Null                                              | "object"    |
| Boolean                                           | "boolean"   |
| Number                                            | "number"    |
| String                                            | "string"    |
| Symbol                                            | "symbol"    |
| Object (ordinary and does not implement [[Call]]) | "object"    |
| Object (implements [[Call]])                      | "function"  |

<sup>Vgl. https://262.ecma-international.org/6.0/#sec-typeof-operator</sup>

```javascript
let step;
console.log(step);         // "undefined"
step = 1;
console.log(typeof step);  // "number"
step = true;
console.log(typeof step);  // "boolean"
step = "1";
console.log(typeof step);  // "string"
step = null;
console.log(typeof step);  // "object"
```

### Implizite Typkonversionen

#### Vergleiche mit ==

Auch in Sprachen wie C# können Werte mit unterschiedlichen Datentypen verglichen werden. So
liefert `if (1.0 == 1)` true, obwohl es ein Vergleich zwischen einem *double* und einem *int*
Wert ist. C# ist jedoch sehr streng, was Vergleiche betrifft. So kann *false* nicht mit *0*
verglichen werden. Dies wird mit einem Compilerfehler enden.

In JavaScript treten (fast) nie Laufzeitfehler bei Vergleichen auf. Bei einem Vergleich wird
eine sehr großzügige Typumwandlung erlaubt. Numerisch interpretierbare Strings können mit
Zahlen verglichen werden (1 und 2). Treffen Strings und Zahlen aufeinander, die nicht umwandelbar
sind, wird einfach false geliefert (3). Bei boolean Werten wird 0 als false und 1 als true
angenommen (4 - 7). Der Vergleich mit null liefert false, außer zwei null Werte werden
verglichen (8 - 9).

```javascript
console.log(1 == "1");            // (1) true
console.log(1 == "   1 ");        // (2) true
console.log("ABCD" == 3.14159);   // (3) false
console.log(0 == false);          // (4) true
console.log(1 == true);           // (5) true
console.log("false" == false);    // (6) false
console.log("0" == false);        // (7) true
console.log(0 == null);           // (8) false
console.log(null == null);        // (9) true
```

#### Vergleiche mit ===

Dieses Vergleichsverhalten hat natürlich seine Geschichte. Über HTTP werden Formulardaten als
String übertragen. Somit ist das Verhalten, Strings mit Zahlen als Zahl zu behandeln, oft von
Vorteil. Manchmal möchte man aber strenger sein. Der *===* Operator liefert nur true, wenn auch
die Typen gleich sind. Daher liefern die folgenden Beispiele alle false (keine Gleichheit):

```javascript
console.log(1 === "1");            // false
console.log(1 === "   1 ");        // false
console.log(0 === false);          // false
console.log(1 === true);           // false
console.log("0" === false);        // false
```

!== verhält sich invers, liefert also bei den Beispielen immer true.

#### Mathematische Operationen

Auch bei mathematischen Operationen wie +, -, * und / wird die Stringumwandlung durchgeführt.
Da + aber auch die Stringverknüpfung ist, liefert das Beispiel (1) einen verknüpften String.
Die anderen Operationen (-, * und /) sind nur für Zahlen definiert. Daher werden Strings - wenn
möglich - als Zahl interpretiert (Beispiele 2 bis 4).

Die schnellste Umwandlung von Strings in Zahlen ist also die Multiplikation mit 1. Die Division
von 2 ganzen Zahlen liefert eine Dezimalzahl und nicht wie in C# eine Ganzzahl.  

```javascript
console.log(1 + "1");       // (1) 11
console.log(1 + 1 * "1");   // (2) 2
console.log(1 - "1");       // (3) 0
console.log("1"/"2");       // (4) 0.5
```

#### if mit Ausdrücken, die nicht boolean sind

In C# oder Java muss ein Ausdruck, der in einem *if* Statement verwendet wird, einen *boolean*
Wert (true oder false) zurückliefern. In JavaScript muss dies nicht so sein. So können z. B.
auch Variablen, die keinen Wert vom Typ *boolean* enthalten, mit *if* abgefragt werden.

```javascript
let count;
if (!count) /* returns true */ {
    console.log("!count is true because it is undefined.")
}
count = 0;
if (!count) /* returns true */ {
    console.log("!count is true because it is 0 (number).")
}
count = "0";
if (!count) /* returns FALSE! */ {
    console.log("!count is true because it is \"0\" (string).");
}
```

Die Ausgabe liefert beim ersten und zweiten Statement den erwarteten Wert, das 3. Statement liefert
jedoch nicht true:
```text
!count is true because it is undefined.
!count is true because it is 0 (number).
```

Die Erklärung liegt im ECMAScript Sprachstandard. Strings sind nur *false*, wenn sie leer sind.

| Argument Type | Result                                                                                    |
| ------------- | ----------------------------------------------------------------------------------------- |
| Undefined     | Return false.                                                                             |
| Null          | Return false.                                                                             |
| Boolean       | Return argument.                                                                          |
| Number        | Return false if argument is +0, −0, or NaN; otherwise return true.                        |
| String        | Return false if argument is the empty String (its length is zero); otherwise return true. |
| Symbol        | Return true.                                                                              |
| Object        | Return true.                                                                              |

<sup>vgl. https://262.ecma-international.org/6.0/#sec-toboolean</sup>

Gerade die fehlende Unterscheidung zwischen 0 und *undefined* macht es oft nötig, den === Operator
zu nutzen. So liefert `if (count === undefined)` auch nur true, wenn *count* nicht zugewiesen wurde.

### Umgang mit undefined

Bei Funktionsargumenten ist es oft notwendig zu prüfen, ob eine Variable überhaupt zugewiesen wurde.
Folgende Statements sind weit verbreitet:

```javascript
let val;
if (!val) { return; }                        // (1)
if (typeof val === "undefined") { return; }  // (2)
val = val || 0;                              // (3)
val = val ?? 0;                              // (4)
```

- **(1)** verwendet den "klassischen" Ansatz. Der Ausdruck *!val* versucht, val in einen Boolean
Wert umzuwandeln. Bei undefined ist dieser Wert *false*. Nachteil: Auch 0 oder false würde
hier zutreffen.
- **(2)** ist der exakte Weg, eine Prüfung auf *undefined* zu machen.
- **(3)** verwendet den logischen OR Operator. Dieser Operator liefert nicht notwendigerweise
einen boolean Wert. Er liefert den ersten Wert, der sich in true umwandeln lässt bzw. den
letzten Wert. Somit wird hier 0 geliefert. Dies wird häufig dazu verwendet, um Standardwerte
zu setzen. Nachteil: Leerstrings würden hier auch zu 0 werden.
- **(4)** In ECMAScript 11 (2020) wurde der nullish coalescing operator eingeführt. Er liefert
den ersten Wert, der ungleich *undefined* oder *null* ist
(<sup>vgl. https://262.ecma-international.org/11.0/#sec-binary-logical-operators-runtime-semantics-evaluation</sup>).

### Umwandlungsfunktionen und Typprüfungen

Gerade bei Funktionsargumenten möchte man manchmal Prüfen, ob auch der korrekte Wert übergeben wurde.
Der einfachste Weg der Prüfung ist der Operator *typeof*. Der folgende Code liefert jedoch keine
Ausgabe, da bei typeof der aktuell verwendete Typ und nicht der Inhalt betrachtet wird.

```javascript
let val = "1";
if (typeof(val === "number")) /* returns FALSE */ {
    console.log("val is a number.");
}
```

Möchte man prüfen, ob ein Argument numerisch interpretierbar ist und es umwandeln, kann die
Funktion *isFinite()* verwendet werden. Der Name erscheint zwar seltsam, in JavaScript gibt es
aber für infinite (unendlich) einen eigenen Wert. *isFinite()* liefert nur dann true, wenn die Zahl
umwandelbar und nicht infinite oder NaN ist.

```javascript
let val = "12";
if (!isFinite(val)) { return; }
val = Number(val);
console.log(val, typeof(val));  // 12  number
```

Mit *Boolean(val)* kann die entsprechende Umwandlung in einen boolean Wert durchgeführt werden.

## Datum und Zeit

Datums- und Zeitwerte können in Form eines Strings, eines Timestamps oder in Form eines
Date Objektes gespeichert werden.

Für Stringangaben ist folgender Aufbau zulässig:

- Für UTC Datums- und Zeitangaben: `YYYY-MM-DDTHH:mm[:ss[.sss]]Z`
- Für Datums- und Zeitangaben in Lokalzeit: `YYYY-MM-DDTHH:mm[:ss[.sss]]`
- Für Datums- und Zeitangaben mit Zeitzone: `YYYY-MM-DDTHH:mm[:ss[.sss]][(+/-)HH:mm]`
- Für reine Datumsangaben: `YYYY-MM-DD`

Das nachfolgende Beispiel liefert die internen JavaScript Zeitwerte zurück, die jeweils den
24.12.2021 um 16 Uhr speichern. Diese JavaScript Timestamps stellen die Anzahl der Millisekunden
seit dem 1. Jänner 1970 UTC dar. Sie lassen sich auch aus dem Unix Timestamp x 1000 berechnen.

```javascript
let xmasUtc = Date.parse("2021-12-24T16:00Z");
console.log(xmasUtc);     // 1640361600000
let xmasCet = Date.parse("2021-12-24T16:00:00+01:00");
console.log(xmasCet);     // 1640358000000
let xmasLocale = Date.parse("2021-12-24T16:00:00");
console.log(xmasLocale);  // 1640358000000
```

Die dritte Ausgabe ist abhängig von der eingestellten Zeitzone des Betriebssystems. Da der interne
Timestamp eine fortlaufende Nummerierung der Millisekunden ist, kann z. B. das Alter in Tagen leicht
berechnet werden. 1 Tag hat 86400 Sekunden, daher hat 1 Tag 86 400 000 Millisekunden.

```javascript
let end = Date.now();                    // Current Timestamp
let start = Date.parse("1978-04-07");
console.log("Age in days: ", (end - start) / 86_400_000);  // Age in days:  15967.425833275463
```

### Das Date Object

Möchte man nicht mit dem rohen Timestamp arbeiten, stellt das Date Object ein Objekt dar, welches
verschiedene Methoden zur Ausgabe, ... bereitstellt. Es kann auf 3 Arten erstellt werden:

- Erzeugen eines Datumsobjektes aus einzelnen Komponenten: `Date(year, month [,date [,hours [,minutes [,seconds [,ms]]]]])`
- Erzeugen eines Datumsobjektes aus einem Timestamp: `Date(value) `
- Erzeugen eines Datumsobjektes mit der aktuellen Systemzeit: `Date()`

```javascript
let xmasLocale = new Date(Date.parse("2021-12-24T16:00:00"));
console.log(xmasLocale.toDateString());  // Fri Dec 24 2021
console.log(xmasLocale.toISOString());   // 2021-12-24T15:00:00.000Z
console.log(xmasLocale.getMonth());      // 11 !!!
```

Eine genauere Beschreibung ist auf https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date
verfügbar.

> **Achtung:** `xmasLocale.getMonth()` liefert das Monat beginnend bei 0, also in diesem Fall den
> Wert 11. Dies ist ein häufiger Fehler.

