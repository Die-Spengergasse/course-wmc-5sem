# Musterapp für eine Diplomarbeit

> **[Download der App](./next-app20241212.zip)**

Im Verzeichnis `next-app` befindet sich ein Musterprogramm, das als Basis für die eigene Implementierung dienen kann.
Es beinhaltet

- Next.js Version 15
- Auth.js (next-auth) Version 5
- Prisma OR Mapper

## Ausführen des Programmes

### Für Azure AD: app registration

Voraussetzung: Du brauchst in `next-app/.env.local` die IDs des Azure App Service.
Sie hat folgenden Aufbau:

```
AZURE_AD_CLIENT_ID=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
AZURE_AD_CLIENT_SECRET=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
AZURE_AD_TENANT_ID=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
NEXTAUTH_SECRET=123
```

> Unter https://htlspengergasse-my.sharepoint.com/:u:/g/personal/schletz_spengergasse_at/EbqeiuHgjMVGtKc2T7qm71kBDh6JibTL5mBJKf2vTDtr-w?e=RuQVb0
> kannst du eine `.env.local` Datei laden, die eine Anmeldung im Active Directory mit dem Schulaccount ermöglicht.
> Benenne die Datei nach dem Download um, es wird der Name `env.download` für den Download generiert.

Wenn du andere Authentifizierungsprovider in Auth.js nutzen willst, benötigst du diese Datei natürlich nicht.
Löchte dann die Überprüfung, ob diese Datei existiert, aus `next-app/scripts/dev.js` heraus.

### Starten des Programmes

Gehe in das Verzeichnis `next-app` und führe den folgenden Befehl aus:

```
npm run dev
```

## Upgrade der Pakete

Mit `npm run upgrade` werden alle Pakete (auch major versions!) aktualisiert.
Prüfe die Applikation danach, ob noch alles funktioniert.

## Erstellen der Datenbank (mit Prisma)

Mit `npm run init_db` wird die SQLite Datenbank in `next-app/database/app-data.db` erzeugt.
Es wird auch das Seed Skript in `next-app/prisma/seed.ts` aufgerufen, sodass die Datenbank mit Musterdaten befüllt wird.
Du kannst sie mit jedem Datenbankeditor bzw. SQLite Viewer betrachten.

## Weitere Infos

In [nextjs_config.adoc](doku/nextjs_config.adoc) sind die Schritte erklärt, die in diesem Programm eingebaut wurden.
