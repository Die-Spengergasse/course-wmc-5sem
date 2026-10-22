# Lehrunterlagen für Webprogrammierung und Mobile Computing – HTL Spengergasse

Origins des Repos

- https://github.com/Die-Spengergasse/course-wmc-5sem (fetch)
- https://github.com/Die-Spengergasse/course-wmc-5sem (push)

Ein authentifiziertes `gh` command line tool steht zur Verfügung.

## Spezifikationen und Vorlagen

| Datei                        | Bedeutung                                                                                                                                    |
|------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------|
| specs/chapter_spec.md        | Spezifikation zum Aufbau der einzelnen Kapitel.                                                                                              |
| specs/lehrplan_informatik.md | Lehrplan der Höheren Abteilung für Informatik. Der Gegenstand Webprogrammierung und Mobile Computing befindet sich in den Zeilen 936 - 1069  |
| specs/examples/02_DRY.adoc   | Beispiel für ein Kapitel aus einem anderen Kurs (.NET Entwicklung)                                                                           |
| specs/examples/02_DRY.html   | Beispiel für interaktives HTML Zusatzmaterial aus einem anderen Kurs (.NET Entwicklung)                                                      |
| webdevelopment.yml           | Template für die PDF Aufgabe von AsciiDoc Dateien.                                                                                           |

## Zielgruppe

Lehrunterlagen für den Unterricht in Webprogrammierung und Mobile Computing der HTL Spengergasse, Spengergasse 20, Wien (www.spengergasse.at).
Zielgruppe: Schüler und Studierende.
Vermittelt wird Praxiswissen mit theoretischem Hintergrund, keine Uni-Vorlesung.

### Vorwissen der Studenten

Die Studenten haben zuvor HTML 5, CSS 3 und Vanilla JavaScript und Manipulation des DOM Trees (document.getElementById, ...) gelernt.
Sie haben aber noch nicht Typescript oder ein SPA Framework kennen gelernt.
Die Studenten beherrschen auch SQL und Java.

## Technologien

Wir vermitteln die Lehrinhalte auf Basis von Typescript und React 19.
Zur Entwicklung einer full stack Application kommt das Framework Next.js 16 zum Einsatz.
Als Build System verwenden wir node.js.

## Ordnerstruktur

- Die Hauptkapitel sind Ordner in diesem Repo (10_JavaScript, 20_Typescript, ...)
- Darin sind die adoc Dateien mit den Kapiteln. Auf sie wird in der README.md verlinkt.
- HTML Zusatzmaterialien heißen wie die adoc Datei mit der Endung `html`.
- In den Ordnern der Hauptkapitel gibt es einen Unterordner `Uebungen`.
    - In diesen Ordner sind die einzelnen Solutions, die den Studenten als Vorlage der Übung dienen.
    - Erstelle aus der Vorlage eine 7z Datei, damit die Studenten die Solution herunterladen können.
- Beispielprojekte liegen im Unterordner `Beispiele`.
    - Sie dienen dazu, dass die Studenten den Unterricht anhand einer fertigen App besser verfolgen können.
      Sonst neigen sie dazu, Code abzutippen.
    - Erstelle aus dem Beispielprojekt eine 7z Datei, damit die Studenten die Solution herunterladen können.

## Sprache

- Deutsch, Anrede „du“. Viele Leser haben nicht Deutsch als Muttersprache: kurze Sätze, keine
  Redewendungen, Fachbegriffe beim ersten Auftreten erklären. Gängige englische Fachbegriffe
  (Component, Interface, arrow function) bleiben englisch.
- Code ist konsequent Englisch: Bezeichner, Kommentare, Ausgabetexte, Assert-Messages.

## Angaben und sonstige Dateien: als Markdown

- Regel: Alles, das nicht in README.md des Hauptordners (Inhaltsverzeichnis) referenziert wird, wird als Markdown-File erstellt.
  Dies umfasst Übungsangaben, Inhaltsverzeichnisse des Kurses, etc.
- Verwende GitHub Flavored Markdown, da die Markdowns auf github betrachtet werden.

## HTML-Zusatzmaterial

- Interaktive Präsentation: Folien mit Animation (Schritt, Play/Pause), danach Übungsteil mit
  sofortigem Feedback.
- Läuft per Doppelklick über `file://`: CSS/JS inline, kein CDN, keine Web-Fonts, keine
  ES-Module, kein `fetch`.
- Heißt wie das zugehörige Kapitel und liegt daneben. Hell/Dunkel über `prefers-color-scheme`
  (im Dark Mode CI-Grundfarben direkt, Rot als `#f5b4c7`). localStorage nur in `try/catch`.
- Link im Markdown auf GitHub Pages, nicht auf die Repo-Datei:
  `https://die-spengergasse.github.io/course-wmc-5sem/<Pfad mit %20>`.

