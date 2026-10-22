# Spezifikation zum Aufbau der einzelnen Kapitel

Verwende den Skill /asciidoc, um Informationen zum Erzeugen von AsciiDoc Dokumenten abzurufen.

## Aufbau der Kapitel

- Ein adoc-File = ein Kapitel ≈ 4 UE, **Soft-Limit: 500 Zeilen** inkl. Code aber ohne Übungsangaben. Längere Themen teilen. Überschreitungen nur, wenn es unbedingt für das Verständnis nötig ist.
- Schüler lesen ungern lange Texte: Abschnitt mit konkretem Problem/Beispiel beginnen, Tabellen
  und Listen statt langer Absätze, ein SVG pro zentralem Konzept. Admonitions
  (NOTE, TIP, IMPORTANT, CAUTION, WARNING) sparsam verwenden.
- Gibt es ein HTML, wirbt ein `[TIP]` direkt nach der Einleitung dafür:
  fetter Titel „Zum Selbst-Erarbeiten: …“, Leerzeile, dann ein
  sprechender Linktext und kurz, was man sieht und übt und warum sich das lohnt.
- Kapitel mit Code beginnen mit

  ```` 
  [.lead]
  _Link zum Programm: link:{root_dir}/Beispiele/(Name.7z)[Filename.7z], im Repo unter (Pfad zur App)._
  ```` 

  falls es eine App gibt, die die Studenten laden können, um den Unterricht besser zu verfolgen zu können.

- Am Ende `== Übungen`. **Keine Lösungen und keine Lösungshinweise**, sonst wird abgeschrieben.
- Übungsangaben mit `tag::pdf_exclude[]` aus dem PDF ausschließen.
- Neue Kapitel ins Inhaltsverzeichnis von `README.md` eintragen (Leerzeichen als `%20`).

## Template

Die Variablen `root_dir` und `github_url` werden später verwendet, um ein Masterdokument erstellen zu können, das die einzelnen asciidoc Dateien der Übungen mit include einbindet.
Dies setzt dann die Variablen, sodass ein PDF sowoso aus der Einzeldatei aus auch über das Masterdokument erstellt werden kann. 
Die einzelnen adoc Dateien beginnen mit folgendem Header:

```
= (Titel)
:source-highlighter: rouge
:icons: font
:lang: DE
:hyphens:
:figure-caption!:
ifndef::root_dir[:root_dir: .]
ifndef::github_url[:github_url: ..]
ifndef::env-github[:icons: font]
ifdef::env-github[]
:caution-caption: :fire:
:important-caption: :exclamation:
:note-caption: :paperclip:
:tip-caption: :bulb:
:warning-caption: :warning:
endif::[]

// Examples for github_url
.link:{github_url}/Eventmanager/Program.cs[→ Eventmanager/Program.cs]
[source,csharp]
----
public class Program { ... }
----

// Examples for root_dir
image::{root_dir}/rest_example_2055.jpg[]

```

## Links über Codeblöcken

Füge mit

`.link:{github_url}/Eventmanager/Program.cs[→ Eventmanager/Program.cs]`

einen Link auf die origiale Quelldatei ein, wenn du Source Code aus einer Datei zitierst.
Der Link wird auch ins PDF geschrieben, wo bequemer das Original abgerufen werden kann.
Der Link steht natürlich erst nach einem git push zur Verfügung, da er auf das online Repo verweist.

Hinweis: Codeblöcke brechen nach ca. 80 Zeichen im PDF um, beachte dies beim Erstellen.

## Ausschluss aus dem PDF Export

Mit `tag::pdf_exclude[]` können Blöcke aus dem PDF ausgeschlossen werden.
Es wird als Kommentar in der Asciidoc Syntax angegeben, bei Include wird der Tag aber gelesen.

```
// tag::pdf_exclude[]

Content

// end::pdf_exclude[]
```

## PDF Aufgabe eines Kapitels prüfen

Prüfe nach der Erstellung oder Änderung die PDF Aufgabe der AsciiDoc-Datei.

- Jede Zeile `asciidoctor: WARNING` oder `ERROR` in der Ausgabe ist ein Fehler im Kapitel
  (z. B. ein Bild, das unter `{root_dir}` nicht gefunden wird).
- Seiten mit Grafiken, Tabellen und Code mit Ghostscript als PNG rendern und ansehen:
  `C:\ghostscript\bin\gswin64c.exe -q -sDEVICE=png16m -r70 -dFirstPage=1 -dLastPage=3 -dNOPAUSE -dBATCH -sOutputFile=<scratchpad>/seite%d.png <scratchpad>/kapitel.pdf`
- Relative Links: Im einzelnen Kapitel ist `{github_url}` relativ (`..`), daher hier im Quelltext prüfen.
  Jedes `link:` muss auf `{github_url}/…` oder eine absolute URL (`https://…`) zeigen.
- Das Kapitel wird mit dem Theme `webdevelopment.yml` gerendert.
  Achte auch auf Sonderzeichen wie →, ∈, q₀ oder n² (Fallback-Schriften in `webdevelopment.yml`).
- Teste die Links im PDF, die relativ sind und nicht funktionieren.
- Teste die Spaltenbreiten im PDF und passe sie ggf. an den Inhalt an.
- Teste auf schlecht gesetzte Seitenumbrüche.
- Teste, ob die Grafiken lesbar sind. Der User kann zwar im PDF zoomen, der Text in der Grafik sollte aber nicht kleiner als 80% der Fließtestgröße sein.

## Grafiken

Lesbarkeit hat Vorrang vor Optik: hoher Kontrast, und Farbe nie als einziges Merkmal (viele
Menschen sind farbenblind). Bedeutung zusätzlich über Beschriftung, Symbol (✓/✗) oder Linienart.

SVG statt PNG, direkt geschrieben; PlantUML (`.puml` + `.svg`) für UML-Diagramme erlaubt.
Rendern über Kroki (< 1 s, liefert weißen Hintergrund):
`curl -s -X POST -H "Content-Type: text/plain" --data-binary @x.puml https://kroki.io/plantuml/svg -o x.svg -w "%{http_code}"`.
Nur `--data-binary`: `-d` entfernt die Zeilenumbrüche, Kroki liefert dann trotz HTTP 200 die
Seite „Welcome to PlantUML!“. Fürs Prüfen `/plantuml/png` rendern und ansehen.
Neue `.puml` bekommen nach `@startuml` diesen Stil (Sichtbarkeit als Text `+ - # ~` statt
farbiger Icons, Klassenart über den Buchstaben C/A/I/E):

```text
skinparam shadowing false
skinparam classAttributeIconSize 0
skinparam defaultFontSize 15
skinparam ArrowThickness 1.5
skinparam class {
  BackgroundColor #ffffff
  HeaderBackgroundColor #ededed
  BorderColor #1d1d1b
  BorderThickness 1.5
  FontStyle bold
}
skinparam stereotypeCBackgroundColor #d1ebf7
skinparam stereotypeABackgroundColor #fdebd1
skinparam stereotypeIBackgroundColor #e5f4f1
skinparam stereotypeEBackgroundColor #e5f1d9
```

GitHub zeigt SVGs als `<img>`, `prefers-color-scheme` darin ist unzuverlässig. Daher weiße Karte
(`rx`, Rand `#9d9d9c`), damit sie im Dark Mode lesbar bleibt. Schrift als `<text>`. Palette
(CI der Schule), Text immer `#1d1d1b`:

| Rolle | Linie | Füllung | dunkle Textvariante |
|---|---|---|---|
| Rand, Hilfslinien | `#9d9d9c` | `#ededed` | `#5f5f5e` |
| Normal | `#0090d4` | `#d1ebf7` | `#00628f` |
| Aktiv, hervorgehoben | `#f39000` | `#fdebd1` | `#a35f00` |
| Richtig, Lösung | `#71af2e` | `#e5f1d9` | `#4a7a1c` |
| Fehler, abgelehnt | `#cc0935` | `#f6d3db` | `#a8072c` |
| Zweite Kategorie | `#6cc1b0` | `#e5f4f1` | `#2f7d6e` |
