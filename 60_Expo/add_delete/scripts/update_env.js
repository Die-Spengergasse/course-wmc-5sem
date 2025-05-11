const os = require ('os');
const fs = require('fs');
const path = require('path');

function getLocalIpAddress() {
  const interfaces = os.networkInterfaces();
  for (const name of Object.keys(interfaces)) {
    for (const iface of interfaces[name] || []) {
      if (iface.family === 'IPv4' && !iface.internal) {
        return iface.address;
      }
    }
  }
  return '127.0.0.1'; // fallback
}

function updateEnv() {
  const ip = getLocalIpAddress();
  const envContent = `EXPO_PUBLIC_API_URL=http://${ip}:5080\n`;

  const envPath = path.join(__dirname, '..', '.env');
  fs.writeFileSync(envPath, envContent);

  console.log(`✅ .env aktualisiert: ${envContent.trim()}`);
}

updateEnv();
