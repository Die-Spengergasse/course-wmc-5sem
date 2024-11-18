# Erstellen einer Todo-App mit Next.js und TypeScript

![](./todo_first_app_1748.png)

> Download der App: [First_App20241118.zip](./First_App20241118.zip).
> Im Repo ist die App im Ordner *30_TodoApp/First_App*.
> Vergiss nicht, *npm install* nach dem Download im Ordner, wo die Datei *package.json* ist, auszuführen.

Im Kapitel Typescript haben wir bereits auf unser [Backend](01_Backend.md) zugegriffen, um Daten zu laden.
Stelle daher sicher, dass es läuft.
Nun wollen wir die erste SPA (single page app) schreiben, die die Ausgabe in den Browser bringt.

## Was ist Next.js und React?

*React* wurde 2013 von Facebook (jetzt Meta) veröffentlicht, ursprünglich entwickelt, um die wachsenden Anforderungen an interaktive und dynamische Benutzeroberflächen in ihren Anwendungen wie Facebook und Instagram zu bewältigen. React brachte mit der Einführung des virtuellen DOM und des komponentenbasierten Ansatzes eine Revolution in der Art und Weise, wie Entwickler UIs erstellen. Es ermöglichte effizientes Rendern von Änderungen und erleichterte die Wartung großer Anwendungen. Mit der Zeit entwickelte sich React zu einem der beliebtesten JavaScript-Frameworks für die Frontend-Entwicklung und bildet heute die Basis für viele moderne Webanwendungen weltweit.

*Next.js* wurde 2016 von Vercel (früher ZEIT) entwickelt und veröffentlicht, um das Arbeiten mit React zu vereinfachen und zu erweitern. Es wurde geschaffen, um die Lücken in React zu schließen, indem es serverseitiges Rendering (SSR) und statische Seitengenerierung (SSG) "out of the box" bietet. Next.js hat schnell an Beliebtheit gewonnen, weil es die Entwicklung von performanten und SEO-freundlichen Webanwendungen erleichtert, ohne dass Entwickler zusätzliche Konfigurationen für Routing oder Rendering vornehmen müssen. Seit seiner Veröffentlichung hat Next.js zahlreiche neue Funktionen hinzugefügt, darunter API-Routen, Bildoptimierung und integrierte Unterstützung für TypeScript, was es zu einem der führenden React-Frameworks für moderne Webanwendungen gemacht hat.

## Erstellen eine neuen Next.js App

Erstelle mit dem folgendem Befehl in der Konsole ein neues Next.js Projekt mit TypeScript.
Es wird automatisch ein Ordner *todo-app* erstellt, d. h. du führst den Befehl in der Konsole im Verzeichnis darüber aus.

Um das Skript *create-next-app* zur Verfügung zu haben, muss einmalig das Paket global installiert werden:

```
npm install -g create-next-app
```

Danach kann im Zielordner mit der Erstellung der ersten App begonnen werden.

```bash
npx create-next-app@latest todo-app --typescript
```

Beantworte die Fragen zur Einrichtung wie folgt.
Wir wollen nicht den App Router, sondern den Router von Next.js verwenden.
Deswegen beantworten wir die Frage mit *No*.

```
npx create-next-app@latest todo-app --typescript
√ Would you like to use ESLint? ...                               No / Yes <-- YES
√ Would you like to use Tailwind CSS? ...                         No / Yes <-- YES
√ Would you like to use *src/* directory? ...                     No / Yes <-- YES
√ Would you like to use App Router? (recommended) ...             No / Yes <-- YES
√ Would you like to use Turbopack for next dev? ...               No / Yes <-- NO
√ Would you like to customize the default import alias (@/*)? ... No / Yes <-- NO
```

### Überblick über das Next.js Projekt

Nach der Initialisierung findest du die folgenden Dateien und Ordner:

- **src/app/page.tsx**: Dies ist die Indexpage, sie wird aufgerufen, wenn die Root Adresse (/) aufgerufen wird.
- **src/app/globals.css**: Dies ist das Haupt Stylesheet der Applikation.
- **src/app/layout.tsx**: Hier wird das Grundlayout definiert.
  Es ist der "HTML Boilerplate Code".
  In *{children}* werden die einzelnen Seiten eingesetzt.
- **public/**: Statische Dateien wie Bilder, Icons oder andere Ressourcen werden hier abgelegt.
- **next.config.js**: Diese Datei enthält die Konfiguration von Next.js. Hier kannst du spezifische Einstellungen für dein Projekt anpassen.
- **tsconfig.json**: Hier werden die TypeScript-Konfigurationen festgelegt.
- **package.json**: Diese Datei listet alle Abhängigkeiten des Projekts sowie Skripte zum Bauen, Starten und Entwickeln.

### Installieren der notwendigen Pakete

Installiere *axios*, um HTTP-Anfragen an das Backend zu stellen:

```bash
npm install axios
```

## Konfiguration des Linters

Wie in unseren Typescript Projekten wollen wir die Option *no-explicit-any* zentral setzen.
Das können wir in der Datei *.eslintrc.json* machen:

**.eslintrc.json**
```json
{
  "extends": [
    "next/core-web-vitals",
    "next/typescript"
  ],
  "rules": {
    "@typescript-eslint/no-explicit-any": "off"
  }
}
```

## Erstellen der TypeScript Interfaces

Erstelle im Ordner *src/app/types* eine Datei *TodoItem.ts* für die Todo-Items:

**src/app/types/TodoItem.ts**
```typescript
export interface TodoItem {
  guid: string;
  title: string;
  description: string;
  categoryName: string;
  categoryPriority: string;
  categoryIsVisible: boolean;
  isCompleted: boolean;
  dueDate: string;
  createdAt: string;
  updatedAt: string;
}

export function isTodoItem(item: any): item is TodoItem {
  return (
    typeof item === "object" &&
    "guid" in item &&
    "title" in item &&
    "description" in item &&
    "categoryName" in item &&
    "categoryPriority" in item &&
    "categoryIsVisible" in item &&
    "isCompleted" in item &&
    "dueDate" in item &&
    "createdAt" in item &&
    "updatedAt" in item
  );
}
```

Erstelle eine weitere Datei *Category.ts* für die Kategorien:

**src/app/types/Category.ts**
```typescript
export interface Category {
  guid: string;
  name: string;
  description: string;
  isVisible: boolean;
  priority: string;
  ownerName: string;
}

export function isCategory(item: any): item is Category {
  return (
    typeof item === "object" &&
    "guid" in item &&
    "name" in item &&
    "description" in item &&
    "isVisible" in item &&
    "priority" in item &&
    "ownerName" in item
  );
}
```

### Erklärung des TypeScript Codes

- **TodoItem Interface**: Dieses Interface definiert die Struktur eines Todo-Items, die wir vom Backend erhalten.
- **isTodoItem Type Guard**: Dies ist ein sogenannter "Type Guard", der sicherstellt, dass ein Objekt den Typ *TodoItem* hat.
- **Category Interface**: Dieses Interface beschreibt die Struktur einer Kategorie. Auch hier nutzen wir einen "Type Guard" mit *isCategory*.

## Erstellen der Datei layout.tsx

Wie bei jeder Webseite beginnen wir mit dem HTML Grundgerüst.
Die Funktion *RootLayout()* liefert dieses Gerüst zurück.
In *{children}* werden die einzelnen *Pages* eingesetzt, in unserem Fall die Datei *page.tsx*.

**src/app/layout.tsx**
```tsx
import { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "My first Next.js App"
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        {children}
      </body>
    </html>
  );
}
```

## Erstellen der Indexpage der App

Öffne *src/app/page.tsx* und füge den folgenden Code ein:

**src/app/page.tsx**
```tsx
"use client"
import { useEffect, useState } from "react";
import axios from "axios";
import https from "https";
import { TodoItem, isTodoItem } from "./types/TodoItem";
import { Category, isCategory } from "./types/Category";

export default function Home() {
  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>("");

  // Wenn wir im Dropdownfeld eine Kategorie auswählen, sollen nur die Todo Items dieser Kategorie angezeigt werden.
  const filteredTodoItems = selectedCategory
    ? todoItems.filter(item => item.categoryName === selectedCategory)
    : todoItems;

  useEffect(() => {
    // Da wir axios.get mit await verwenden, muss diese Funktion async sein.
    async function fetchData() {
      const agent = new https.Agent({
        rejectUnauthorized: false
      });

      try {
        // Todo Items abrufen
        const todoResponse = await axios.get("https://localhost:5443/api/TodoItems", { httpsAgent: agent });
        const filteredTodos = todoResponse.data.filter(isTodoItem);
        setTodoItems(filteredTodos);

        // Kategorien abrufen, um das Dropdownfeld zu befüllen.
        const categoryResponse = await axios.get("https://localhost:5443/api/Categories", { httpsAgent: agent });
        const filteredCategories = categoryResponse.data.filter(isCategory);
        setCategories(filteredCategories);
      } catch (error) {
        console.error(error);
      }
    };
    // Die Funktion wird ohne await aufgerufen. Bei useEffect können wir keine async Funktion übergeben.
    // Siehe https://react.dev/reference/react/useEffect#fetching-data-with-effects
    fetchData();
  }, []);

  return (
    <div>
      <h1>Todo Liste</h1>
      <select onChange={(event)=>setSelectedCategory(event.target.value)}>
        <option value="">Alle Kategorien</option>
        {categories.map(category => (
          <option key={category.guid} value={category.name}>
            {category.name}
          </option>
        ))}
      </select>

      <ul>
        {filteredTodoItems.map(item => (
          <li key={item.guid}>
            <h2>{item.title}</h2>
            <p>{item.description}</p>
            <p>Kategorie: {item.categoryName}</p>
            <p>Fällig am: {new Date(item.dueDate).toLocaleDateString()}</p>
            <p>Status: {item.isCompleted ? "Abgeschlossen" : "Ausstehend"}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
```

### Erläuterung des Codes

- **useState**: Mit diesem Hook erstellen wir lokale State-Variablen. *todoItems* speichert die Liste der Todo-Items, *categories* die Kategorien und *selectedCategory* speichert die aktuell ausgewählte Kategorie.
- **useEffect**: Dieser Hook führt seiteneffektreiche Aktionen aus, wie z.B. das Abrufen von Daten. Beim Laden der Seite ruft *useEffect* die Daten von den beiden API-Endpunkten ab und speichert sie im State.
- **axios.get()**: Hiermit werden HTTP GET-Anfragen an die API gesendet. Die Antworten werden in den jeweiligen State-Variablen gespeichert.
- **handleCategoryChange**: Diese Funktion wird aufgerufen, wenn der Benutzer eine Kategorie aus dem Dropdown-Menü auswählt. Die ausgewählte Kategorie wird im State gespeichert und die Liste der Todo-Items wird entsprechend gefiltert.
- **isTodoItem & isCategory**: Diese Type Guards stellen sicher, dass die vom Backend erhaltenen Daten die erwarteten Strukturen haben, bevor sie in den State gespeichert werden.
- **use client**: Standardmäßig werden Komponenten von Next.js am Server gerendert.
  Wollen wie Userinteraktion, muss die Komponente allerdings am Client gerendert werden.
  Details sind im nächsten Kapitel zu finden.

#### Wie funktioniert useState<TodoItem[]>([])?

Der React-Hook useState wird verwendet, um den internen State einer Komponente zu verwalten. Wenn wir *useState<TodoItem[]>([])* verwenden, initialisieren wir den State als ein leeres Array, das vom Typ *TodoItem[]* ist – also ein Array von TodoItem-Objekten.

```typescript
const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
```

- **todoItems:** Dies ist die State-Variable, die die Todo-Items enthält.
- **setTodoItems:** Diese Funktion wird verwendet, um den Wert von todoItems zu aktualisieren.

Der Hook erwartet den Typ der State-Variable, also ein Array von TodoItem. Der Startwert ist ein leeres Array *[]*.
Durch die Verwendung des Typs *TodoItem[]* weiß TypeScript, dass wir in der todoItems-Variable eine Liste von TodoItem-Objekten speichern.
Dies stellt sicher, dass wir später sicher auf die Eigenschaften dieser Objekte zugreifen können, z.B. item.title oder item.description.

#### Was macht useEffect genau?

Der Hook useEffect wird in React verwendet, um Nebenwirkungen (engl. "side effects") in funktionalen Komponenten zu behandeln, wie z.B. das Abrufen von Daten, das Abonnieren von Ereignissen oder das direkte Manipulieren des DOMs, die nicht innerhalb der normalen Ausführung des Renderings stattfinden sollten.


```typescript
useEffect(() => {
  // Abrufen von Todo Items und Kategorien
}, []);
```

#### Wann wird useEffect aufgerufen?

useEffect wird immer dann aufgerufen, wenn die Komponente gerendert wird. In unserem Fall haben wir ein leeres Abhängigkeitsarray [] als zweiten Parameter angegeben, was bedeutet, dass der Effekt nur einmal beim ersten Rendern der Komponente ausgeführt wird.
Wenn wir bestimmte Variablen in das Abhängigkeitsarray aufnehmen (z.B. *[selectedCategory]*), würde useEffect bei jeder Änderung dieser Variablen erneut ausgeführt.
Wofür wird useEffect in unserem Fall genutzt?

Es wird verwendet, um die Todo-Items und Kategorien von der API zu laden, wenn die Seite das erste Mal gerendert wird. Die Daten werden dann im State der Komponente gespeichert.

## CSS Layout für die App

Um das Layout der App zu verbessern, erstellen wir eine einfache CSS-Datei. Gehe zu *src/app/globals.css* und füge den folgenden Inhalt ein:

**src/app/globals.css**
```css
body {
  font-family: Arial, sans-serif;
  margin: 0;
  padding: 0;
  background-color: #f4f4f9;
}

h1 {
  text-align: center;
  color: #333;
}

select {
  display: block;
  margin: 20px auto;
  padding: 10px;
  font-size: 16px;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  background-color: #fff;
  margin: 10px;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

li h2 {
  margin: 0;
  font-size: 20px;
  color: #0070f3;
}

li p {
  margin: 5px 0;
  color: #666;
}
```

## Die fertige Dateistruktur

Am Ende soll die Dateistruktur so aussehen.
Achte darauf, dass im Ordner *src* nur der Ordner *app* ist.

```
todo-app
  + .eslintrc.json
  + next.config.ts
  + package.json
  + public
  + src
  |  + app
  |  |  + globals.css
  |  |  + layout.tsx
  |  |  + page.tsx
  |  |  + types
  |  |  |  + Category.ts
  |  |  |  + TodoItem.ts
  + tailwind.config.ts
  + tsconfig.json
```

## Starte des Entwicklungsservers

Führe den folgenden Befehl aus, um den Entwicklungsserver zu starten:

```bash
npm run dev
```

Öffne deinen Browser und gehe zu *http://localhost:3000*. Du solltest nun eine Todo-Liste sehen, die Daten vom Backend abruft und eine Filterfunktion nach Kategorien bietet.


## Exportieren der App

Editiere die Datei **next.config.ts** und füge folgenden Inhalt ein:

```javascript
import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  reactStrictMode: true,
  output: "export"
};

export default nextConfig;
```

Die Option *output: "export"* legt ein *out* Verzeichnis mit der App an. Mit

```
npm run build
```

kann die App nun erstellt und in dieses Verzeichnis geschrieben werden.
Es kann nun auf einem Webserver gehostet werden.
