# Die erste Typescript App

## Anlegen des Verzeichnisses und Erstellen der package.json

Bisher bestanden unsere Javascript "Programme" nur aus einer Datei, die wir mit *node* ausgeführt haben.
Wollen wir typescript und größere Projekte umsetzen, reicht dies aber nicht aus.
Daher müssen wir - ähnlich wie die Projekte in C# - ein Verzeichnis anlegen und das Projekt mit den
folgenden Befehlen initialisieren.
Hinweis: In der bash und unter macOS ist *md* durch *mkdir* zu ersetzen.

```bash
md first_app
cd first_app
npm init -y
```

*npm init* erstellt nur eine Datei mit dem Namen *package.json*.
Die *package.json* ist eine zentrale Konfigurationsdatei in einem Node.js-Projekt, die wichtige Metadaten über das Projekt enthält.
Dazu gehören der Name des Projekts, die Version, Abhängigkeiten, Skripte und weitere Konfigurationen.
Sie ermöglicht es, Node.js-Pakete und -Module zu verwalten, indem sie festlegt, welche Pakete (Abhängigkeiten) installiert werden müssen und welche Versionen davon kompatibel sind.
Außerdem können in der package.json Skripte definiert werden, die wiederkehrende Befehle wie das Starten des Servers oder das Testen des Codes automatisieren.
Sie ist also essenziell für die Verwaltung und Automatisierung von Node.js-Projekten.

Ersetze nun die erstellte *package.json* Datei durch diese Version.
Sie gibt an, dass wir ein Modul erstellen möchten.

**package.json**
```javascript
{
  "name": "first_app",
  "version": "1.0.0",
  "type": "module"
}
```

## Installieren von Typescript und des Linters (ESLint)

Wir brauchen nun Zusatzpakete.
Ähnlich wie *dotnet package* in .NET können wir über den node package manager (npm) auf fertige Pakete zugreifen und sie in unser Projekt einbinden.
Manche Pakete brauchen wir nur zur Entwicklung, deswegen verwenden wir die Option *--save-dev*.
Hier installieren wir den Typescript Compiler und den Linter (ESLint) in unser Projekt.
Technisch gesehen wird nur eine Zeile in die Datei *package.json* geschrieben.

```bash
npm install --save-dev typescript @types/node
npm install --save-dev eslint globals typescript-eslint 
```

Erstelle nun im Verzeichnis *first_app* die Konfigurationsdateien für den Typescript Compiler und den Linter.
Achte genau auf die Dateinamen.

**tsconfig.json**
```javascript
{
    "compilerOptions": {
        /* Language and Environment */
        "target": "ESNext", /* Set the JavaScript language version for emitted JavaScript and include compatible library declarations. */
        /* Modules */
        "module": "NodeNext", /* Specify what module code is generated. */
        "types": [
            "node"
        ], /* Specify type package names to be included without being referenced in a source file. */
        /* Interop Constraints */
        "esModuleInterop": true, /* Emit additional JavaScript to ease support for importing CommonJS modules. This enables 'allowSyntheticDefaultImports' for type compatibility. */
        "forceConsistentCasingInFileNames": true, /* Ensure that casing is correct in imports. */
        /* Type Checking */
        "strict": true, /* Enable all strict type-checking options. */
        /* Completeness */
        "skipLibCheck": true, /* Skip type checking all .d.ts files. */
        /* Debugger */
        "sourceMap": true
    }
}
```

**eslint.config.js**
```javascript
import globals from "globals";
import tseslint from "typescript-eslint";

export default [
  { files: ["**/*.{js,mjs,cjs,ts}"] },
  { languageOptions: { globals: globals.node } },
  ...tseslint.configs.recommended,
  { rules: { "@typescript-eslint/no-explicit-any": "off" } }
];
```

## Niemals node_moules einchecken

Die Module werden in den Ordner *node_modules* geschrieben.
Dieser Ordner darf nie in das Repo geladen werden, da jeder Client die Pakete selbst laden muss.
Stelle dies mit der Datei *.gitignore* im Verzeichnis *first_app* sicher:

**.gitignore**
```
**/node_modules
```

## Schreiben des source codes

Erstelle nun ein Verzeichnis *src* und erstelle darin eine Datei *app.ts*.
Füge den folgenden Code ein:

**src/app.ts**
```typescript
function add(x: number, y: number): number {
    return x + y;
}

const result = add(1,2);
const result2 = add(1,"x");   // error TS2345: Argument of type 'string' is not assignable to parameter of type 'number'.
console.log(result);
```

## Ausführen des Programmes

Möchten wir nun unser Programm ausführen, müssen wir 2 Befehle angeben.
Der Typescript Compiler übersetzt das Programm in JavaScript.
Das entstandene JavaScript wird dann wie gewohnt mit *node* ausgeführt.
*npx* kann node Pakete direkt starten.

```bash
npx tsc && node src/app.js
```

### Hinzufügen des Startscripts

Damit wir nicht immer diesen Befehl angeben müssen, können wir ein sogenanntes Startskript hinterlegen.
Dafür füge in der Datei *package.json* den Key *scripts* ein.
Achte darauf, dass die Datei gültig bleibt (Beistriche, etc.).

**package.json**
```javascript
    // ...
    "scripts": {
      "start": "tsc && node src/app.js"
    },
```

Nun kann die App im Verzeichnis *first_app* in der Konsole aufgerufen werden.

```bash
npm run start
```

### Laden aller Pakete

Wenn du den Code aus dem Repo klonst, gibt es keinen Ordner *node_modules*.
Die Pakete können mit folgendem Befehl geladen werden:

```bash
npm install
```
