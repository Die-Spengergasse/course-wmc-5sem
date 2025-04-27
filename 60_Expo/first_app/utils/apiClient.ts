import axios from 'axios';

// Erstelle eine Axios-Instanz mit Basis-URL und anderen Optionen
export const axiosInstance = axios.create({
    baseURL: process.env.EXPO_PUBLIC_API_URL, // Basis-URL für alle Anfragen
    timeout: 10000, // Timeout für Anfragen (optional)
    headers: {
        'Content-Type': 'application/json', // Standardheader
    }
});

/**
 * Beschreibt die Struktur einer Fehlerantwort von der API.
 * 
 * @property {number} status - Der HTTP-Statuscode.
 * @property {string} message - Die Fehlermeldung (falls vorhanden).
 * @property {Record<string, string>} validations - Validierungsfehler (Schlüssel-Wert-Paare).
 */
export type ErrorResponse = {
    status: number;
    message: string;
    validations: Record<string, string>;
};

/**
 * Überprüft, ob eine Antwort ein `ErrorResponse` ist.
 * 
 * @param {any} response - Die zu überprüfende Antwort.
 * @returns {boolean} `true`, wenn die Antwort ein gültiger `ErrorResponse` ist, sonst `false`.
 */
export function isErrorResponse(response: any): response is ErrorResponse {
    return (
        typeof response === 'object' &&
        response !== null &&
        typeof response.status === 'number' &&
        typeof response.message === 'string' &&
        typeof response.validations === 'object'
    );
}

/**
 * Wandelt eine `AxiosResponse` in eine `SuccessResponse` oder `ErrorResponse` um.
 * 
 * @template T - Der Typ der Nutzdaten (`data`) der Antwort.
 * @param {AxiosResponse} response - Die von Axios zurückgegebene Antwort.
 * @returns {SuccessResponse<T> | ErrorResponse} 
 *    - Eine `SuccessResponse` für erfolgreiche Statuscodes.
 *    - Eine `ErrorResponse` für Fehlerstatuscodes, einschließlich Validierungsfehlern von ASP.NET Core.
 */
export function createErrorResponse(error: unknown): ErrorResponse {
    if (!axios.isAxiosError(error))
        return {
            status: 0, message: error instanceof Error ? error.message : "", validations: {}
        }
    if (!error.response)
        return { status: 0, message: "Der Server ist nicht erreichbar.", validations: {} }
    const response = error.response as any;
    // Überprüft, ob es sich um ein ASP.NET Core Validierungsobjekt handelt
    const isAspValidation = response.data !== null && typeof response.data === 'object' && 'errors' in response.data;

    return {
        status: response.status,
        message: isAspValidation
            ? ""
            : typeof response.data === 'string' && response.data
                ? response.data
                : `Der Server lieferte HTTP ${response.status}.`,
        validations: Object.keys(isAspValidation ? response.data.errors : {}).reduce((acc, key) => {
            acc[key.toLowerCase()] = response.data.errors[key].join(" ");
            return acc;
        }, {} as Record<string, string>)
    }
}

/**
 * Erstellt eine leere Fehlerantwort, die als Initialwert in `useState` verwendet werden kann.
 * 
 * @returns {ErrorResponse} Eine `ErrorResponse` mit Status `0` und leeren Feldern.
 */
export function createEmptyErrorResponse(): ErrorResponse {
    return {
        status: 0,
        message: '',
        validations: {}
    };
}
