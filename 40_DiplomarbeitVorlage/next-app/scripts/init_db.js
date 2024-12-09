const { execSync } = require("child_process");
const fs = require("fs");
const path = require("path");

// Funktion zum Löschen von Verzeichnissen
function rimraf(dir) {
  if (fs.existsSync(dir)) {
    fs.rmSync(dir, { recursive: true, force: true });
  }
}

// Hauptfunktion
function main() {
  try {
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
  } catch (error) {
    console.error("Error during execution:", error.message);
    process.exit(1);
  }
}

main();
