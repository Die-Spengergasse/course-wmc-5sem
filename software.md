# Installation der benötigten Software

Unter Windows installierst du die Software über `winget`, das auf einem Stock Windows 11
bereits vorinstalliert ist. Öffne dazu ein Terminal (PowerShell, Eingabeaufforderung oder
Windows Terminal) und führe die jeweiligen Befehle aus. Alle Befehle laufen unattended ab
(keine Klicks nötig), zeigen aber ein Installationsfenster mit Fortschrittsanzeige – so
siehst du, dass etwas passiert, ohne einen Assistenten durchklicken zu müssen. Falls Windows
per UAC-Dialog nach Administratorrechten fragt, bestätige diesen; das ist normal.

Solltest du bereits eine ältere Version installiert haben: `winget install` erkennt das
automatisch und aktualisiert auf die neueste Version, ein manuelles Deinstallieren ist nicht
nötig.

Für macOS verwenden wir stattdessen Homebrew (https://brew.sh/). Die passenden
`brew`-Befehle stehen jeweils direkt unter dem Windows-Befehl.

## Node.js

Installiere die aktuelle Version von Node.js – **nicht** die LTS-Version:

```
winget install --id OpenJS.NodeJS --exact --accept-package-agreements --accept-source-agreements --override "/passive /norestart"
```

Unter macOS installiert `brew install node` ebenfalls die aktuelle, nicht die LTS-Version:

```
brew install node
```

Am Ende der Installation sollte der Befehl *node --version* die aktuelle Version ausgeben:

```
C:\Users\MyUser>node --version
v26.x.x
```

## Playwright CLI

Installiere das globale npm-Tool `@playwright/cli` (https://www.npmjs.com/package/@playwright/cli).
Der Befehl ist unter Windows und macOS identisch, da er über `npm` läuft:

```
npm install -g @playwright/cli@latest
playwright-cli install --skills
```

## Python

Installiere die aktuelle Version von Python 3:

```
winget install --id Python.Python.3.14 --exact --accept-package-agreements --accept-source-agreements --override "/passive InstallAllUsers=1 PrependPath=1 Include_test=0"
```

> **Hinweis:** Die winget-Id enthält die Minor-Version (hier *3.14*). Falls zwischenzeitlich
> eine neuere Version erschienen ist, findest du die aktuelle Id mit
> `winget search Python.Python.3`.

Unter macOS:

```
brew install python
```

Am Ende der Installation sollte der Befehl *python --version* (unter macOS ggf.
*python3 --version*) die aktuelle Version ausgeben:

```
C:\Users\MyUser>python --version
Python 3.14.x
```

## Visual Studio Code

Zum Entwickeln von JavaScript Code gibt es natürlich viele IDEs und Editoren. Wir werden Visual
Studio Code verwenden.

```
winget install --id Microsoft.VisualStudioCode --exact --accept-package-agreements --accept-source-agreements --override "/SILENT /NORESTART /MERGETASKS=!runcode,addcontextmenufiles,addcontextmenufolders,associatewithfiles,addtopath"
```

Der Befehl aktiviert dabei automatisch die Optionen *Add "Open with Code" action to Windows
Explorer file context menu* und *Add "Open with Code" action to Windows Explorer directory
context menu* sowie das Hinzufügen von `code` zum PATH – ein manuelles Anhaken im Setup-Assistenten
entfällt.

Unter macOS:

```
brew install --cask visual-studio-code
```

### Extensions

Installiere anschließend über das Terminal die folgenden Extensions (der Befehl funktioniert
unter Windows und macOS identisch):

```
code --install-extension dbaeumer.vscode-eslint
code --install-extension yzhang.markdown-all-in-one
code --install-extension asciidoctor.asciidoctor-vscode
code --install-extension schletz.asciidoc-productivity
```

### Einstellungen

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

## Prompt

Statt die Befehle oben selbst einzutippen, kannst du auch einen KI-Coding-Assistenten (z. B.
Claude Code, OpenAI Codex, GitHub Copilot Chat) damit beauftragen. Verwende dazu folgenden Prompt:

```
Lade https://raw.githubusercontent.com/Die-Spengergasse/course-wmc-5sem/refs/heads/main/software.md
und installiere die dort beschriebene Software (Node.js, playwright-cli, Python 3 und Visual Studio Code
inklusive der dort gelisteten Extensions und Einstellungen) auf meinem Rechner.

Erkenne zuerst, ob ich unter Windows oder macOS arbeite, und führe ausschließlich die für
mein Betriebssystem passenden Befehle aus der Datei aus (winget unter Windows, Homebrew
unter macOS) – übernimm sie unverändert, ändere insbesondere keine der winget-Flags.

Auf meinem Rechner können bereits ältere Versionen der Software installiert sein; die
Befehle sollen trotzdem funktionieren und automatisch auf die aktuelle Version
aktualisieren.

Führe die Befehle direkt in einem Terminal aus. Bestätige am Ende mit den
Versionsausgaben von node, python und code, dass alles korrekt installiert wurde, und
melde dich, falls ein Befehl fehlschlägt oder eine manuelle Bestätigung (z. B. UAC unter
Windows) nötig ist.
```
