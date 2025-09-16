# Backend für die Todo App

Das Modell der Todo-App besteht aus mehreren Klassen, die in Beziehung zueinander stehen und verschiedene Aspekte der Aufgabenverwaltung darstellen. Im Zentrum steht die Klasse TodoItem, die durch die Felder *Title* (Aufgabentitel), *Description* (Beschreibung der Aufgabe), *DueDate* (optional festgelegtes Fälligkeitsdatum), und *IsCompleted* (Status der Fertigstellung) beschrieben wird. Jede Aufgabe gehört zu einer *Category*, die durch *Name*, *Description*, *IsVisible* (Sichtbarkeit), *Priority* (vom Typ Enum: Low, Medium, High) und *Owner* (der Benutzer, der die Kategorie erstellt hat) definiert ist. Zu jeder Aufgabe können zudem mehrere *TodoTask*-Objekte gehören, die Felder wie *Title*, *IsCompleted*, *DueDate* enthalten und somit Unteraufgaben abbilden. Dieses Modell erlaubt eine klare Strukturierung von Aufgaben, Kategorien, Prioritäten und Benutzern in einer flexiblen Todo-Anwendung.

[Zum Bild](https://www.plantuml.com/plantuml/svg/nLHHQzim47xthpZyr5AdRAzXopeIQ0FTLbYwFQlHBHQn92ET2MNqlqzAvBCMbmp2e7hntNSwST_toRhn91nrkYbg9H5Gj_GC6gcsqXT5AzPRiK4eLy9lVGc_2mYhmLy4PC6fJKqWbSCrAMlsDJITXWUdh5FqdFR7TMzt77z1gJhDwLquPf-yf1Cejiu5uSQw_8m9B2LCGHdAeDjEHJ1-ClIhq1XGJqwPJOOlqA-TQkEuhvMN_eVowgc9lK_MDbc9EKqvpAhVTWPTA3rwT1ayToW8vMSAfKM8LafZLdvivOdoPCBn5SjnSPWPJ21OGKhamAhYdZW_g9vGDH1eKr3jfmKRHxyS6x2oqYu9zNa6h1GrvpZ0jayv_CkVkTLjWuHoxD2YmoXYfkMd_sN5NepzFn3AzyhJXnwOlUuNgzVxHL6FquOnzNKAgjv39ZriPeeW4-sFOBgc6Hl9ivzd95-3AOSdR7vpft8JcFa5aw0KL93xmhAE1V-dqCLBaNIEVrFIEDTthEtuXKhOTz7GCVOWlByJlXxmz23KkDx6uiuwsT-oDkMNkUPNS8i6dI2C4_vKL1-tTG7d1-NudDWKI8R55tGoF8S3MBwxlZPbgCsWV5yUr9Z2xF0q5kp0qJPk3rivANhZZdTdbt76IN63HdQw-GC0)

<sup>[PlantUML Source](https://www.plantuml.com/plantuml/uml/nLNHQjim57ttLrpyr5AdRAzXopeIQ0FTLbYwFQlHNImYIqQw9fJHVv-KoDTQN1O8W_ebL-T8ufvp9EN6at3KDRgebKJ0fgLdQ91gAtrHj6Az5Hk4SYNyrfVmku3mQVn5G1QSsjG4KZbSax9cpw7f6dXmoZf5psb-_ExjZkilA5NNyF0kRBCVlAGTA2wT2yADTVaU4rXAc88ob46tdOfW_CdGhwB1W7foocmoV85-xLGRstsDNleVogid7TS-MQipYZLDkSpORpk3Bb2F7bq6pnqAGlcP0hK8rKercD6FhVm7FPbmVSLsN1nw1XC4Lf06Sb1LyPhOFwXUK3K8Q5DHxRi56rk_70Umij8k2Plp0bWfqkSamB5FkVmhdxbjMes4SfpGeiDeOkRbf_yLnTwC_Hq8zBtozA47Pg_dnUxrUP4YVI8mbhvEHTLdoA9vyY0HP9Bz4KodzJX8xlsy8FaOJBay7VFR9PM7mCelM0Ibee8y5xPrRFX9eukN8laSRrFoEFTtxEtuXKhOTw7f67iGtjy9NmzuUX1gN6yZkRDEzgUiJUFBtFChk4M3JX363dygge-NTG7d1-LudxWeY0mV7z39y1mEOFdk-jgKWPj1-Ruyk365sU5fB3Y1esdPVP_neEY3EvwT7SOP3ueRD3B-W_q1)</sup>

## Starten der App

Klone das Repo mit folgendem Befehl auf deinen Rechner:

```bash
git clone https://github.com/Die-Spengergasse/course-wmc-5sem
```

### Start ohne Docker

Wenn du die .NET 8 SDK installiert hast, kannst du einfach die Datei *startServer.cmd* **(Windows)** im Verzeichnis *30_TodoApp/TodoBackend* ausführen.
Unter **macOS** gehst du im Terminal in das Verzeichnis *30_TodoApp/TodoBackend* und führst die Datei *./start_server.sh* aus.
Beide Skripts führen vorher *git pull* durch, um sicherzustellen, dass du den aktuellen Sourcecode hast.
Nun kannst du die API mit *https://localhost:5443/swagger* im Browser ansehen.

> **Hinweis:** Beende die API mit *CTRL+C* in der Konsole, damit der Port wieder freigegeben wird.

### Start als Docker Container

Wenn du keine .NET 8 SDK hast, oder die App als Docker container starten möchtest, gibt es ein Shellscript [create_container.sh](TodoBackend/create_container.sh) im Ordner *30_TodoApp/TodoBackend*.
Führe es unter **Windows** mit Doppelklick aus. Das Programm *git bash* muss dafür installiert sein.
Unter **macOS** starte das Skript im Terminal mit *./create_container.sh*
