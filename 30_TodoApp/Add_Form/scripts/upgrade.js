const { execSync } = require("child_process");
const readline = require("readline");

// Funktion für die Pause (Benutzereingabe)
function pause(message = "Press ENTER to continue...") {
  return new Promise((resolve) => {
    const rl = readline.createInterface({
      input: process.stdin,
      output: process.stdout,
    });
    rl.question(message, () => {
      rl.close();
      resolve();
    });
  });
}

// Hauptfunktion
async function main() {
  try {
    console.log("Running npm-check-updates...");
    execSync("npx npm-check-updates -u", { stdio: "inherit" });

    // Pause nach dem Update-Befehl
    await pause();

    console.log("Installing updated dependencies...");
    execSync("npm install", { stdio: "inherit" });

    // Pause nach der Installation
    await pause();
  } catch (error) {
    console.error("Error during upgrade process:", error.message);
    process.exit(1);
  }
}

main();
