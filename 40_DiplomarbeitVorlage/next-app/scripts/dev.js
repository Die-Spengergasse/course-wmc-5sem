const { execSync } = require("child_process");
const fs = require("fs");
const path = require("path");

// Funktion zum Löschen von Verzeichnissen
function rimraf(dir) {
  if (fs.existsSync(dir)) {
    fs.rmSync(dir, { recursive: true, force: true });
  }
}

// Funktion zur Ausgabe einer Nachricht in gelber Farbe
function logWarning(message) {
  console.log(`\x1b[33m${message}\x1b[0m`); // ANSI-Farbcode für Gelb
}

// Hauptfunktion
function main() {
  try {
    // Überprüfung, ob die .env.local-Datei existiert
    if (!fs.existsSync(".env.local")) {
      logWarning("Fehler: Die Datei .env.local wurde nicht gefunden. Bitte erstellen Sie die Datei und versuchen Sie es erneut.");
      process.exit(1);
    }
        
    // Installiere Abhängigkeiten
    console.log("Installing dependencies...");
    execSync("npm install", { stdio: "inherit" });
        
    // Datenbank und Migrationen löschen
    console.log("Cleaning up database and migrations...");
    rimraf("database");
    rimraf("prisma/migrations");

    // Prisma Migration ausführen
    console.log("Running prisma migrations...");
    execSync("npx prisma migrate dev --name init", { stdio: "inherit" });

    // Next.js starten
    console.log("Starting Next.js development server...");
    execSync("npx next dev", { stdio: "inherit" });
  } catch (error) {
    console.error("Error during execution:", error.message);
    process.exit(1);
  }
}

main();
