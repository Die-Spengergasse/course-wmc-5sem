"use server"
import https from "https"
import axios from 'axios';
import { cookies } from "next/headers";

const agent = new https.Agent({
    rejectUnauthorized: false
});

// Erstelle eine Axios-Instanz mit Basis-URL und anderen Optionen
export async function getAxiosInstance() {
    const cookieStore = await cookies(); // ⬅ Next.js API
    const cookie = cookieStore.toString(); // konvertiert alle Cookies zu Header-String
    return axios.create({
        baseURL: 'https://localhost:5443', // Basis-URL für alle Anfragen
        timeout: 10000, // Timeout für Anfragen (optional)
        headers: {
            Cookie: cookie,
            'Content-Type': 'application/json', // Standardheader
        },
        httpsAgent: agent,
        withCredentials: true // Cookies werden mitgesendet
    });
}
