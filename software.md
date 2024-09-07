# Installation der benötigten Software

## Node.js

Lade von https://nodejs.org/en/download die LTS Version von Node.js. Beachte bei der Installation folgendes:
- Installiere den Server unter Windows in *C:\\nodejs* und nicht in einem tief verschachtelten Verzeichnis.
- Verwende für die Installation unter macOS die Software Homebrew (https://brew.sh/). Danach kann
  mit *brew install node* im Terminal Node.js installiert werden.

Am Ende der Installation sollte der Befehl *node --version* die aktuelle Version ausgeben:

```
C:\Users\MyUser>node --version
v20.xx.x
```

## Visual Studio Code

Zum Entwickeln von JavaScript Code gibt es natürlich viele IDEs und Editoren. Wir werden Visual
Studio Code verwenden. Lade daher von https://code.visualstudio.com/ den Editor für dein
Betriebssystem herunter.

> **Hinweis:** Aktiviere beim Setup die Option
>  *Add "Open with Code" action to Windows Explorer file context menu* und
>  *Add "Open with Code" action to Windows Explorer directory context menu*.
> Da Node.js Projekte nicht als Einzeldatei geöffnet werden können, ist diese Option sehr hilfreich!

Danach installiere über das Extension Menü die Extension *ESLint*.

Öffne nun die Einstellungen (Drücke *F1* oder *SHIFT+CMD+P* für die Menüzeile. Gib dann  
*settings* ein und wähle den Punkt *Preferences: Open User Settings (JSON)*. Füge die folgenden
Einstellungen in die Datei ein und speichere sie ab:

```json
{
    "editor.bracketPairColorization.enabled": true,    
    "security.workspace.trust.untrustedFiles": "open",
    "editor.minimap.enabled": false,
    "editor.rulers": [
        100
    ],    
    "terminal.integrated.defaultProfile.windows": "Command Prompt",
    "extensions.ignoreRecommendations": true
}
```
