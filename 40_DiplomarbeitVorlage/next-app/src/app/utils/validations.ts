/**
 * @fileoverview
 * Modul für die Validierung von Daten, die mit Prisma in die DB geschrieben werden sollen.
 * Es wird das Paket zod verwendet, um die Validierung zu definieren.
 * Der Typ ValidationError wird verwendet, um Fehlermeldungen im Frontend zu verarbeiten.
 * Siehe pages/persons/add für die Verwendung.
 */
import { z } from "zod";

/**
 * Typ für die Validierungsfehler, die beim Einfügen eines Entities vom Typ T in Prism auftreten können.
 * T ist z. B. Person aus dem Prisma Client.
 * Somit kann über fieldErrors typsicher auf die Felder des Entities zugegriffen werden.
 */
export type ValidationError<T> = {
    message: string;
    fieldErrors: Record<keyof T, string>;
}

/**
 * Typeguard für ValidationError.
 */
export function isValidationError<T>(value: unknown): value is ValidationError<T> {
    if (typeof value !== 'object' || value === null) return false;
    const validationError = value as ValidationError<T>;
    if (typeof validationError.message !== 'string') return false;
    if (typeof validationError.fieldErrors !== 'object' || validationError.fieldErrors === null) {
        return false;
    }
    for (const key in validationError.fieldErrors) {
        if (typeof validationError.fieldErrors[key] !== 'string') {
            return false;
        }
    }
    return true;
}

/**
 * Erstellt aus einem Error einen ValidationError, den wir in unseren Formularen anzeigen können.
 */
export function createValidationError<T>(error?: unknown): ValidationError<T> {
    if (error instanceof z.ZodError) {
        const flattened = error.flatten();
        const message = flattened.formErrors.join(", ");
        const fieldErrors = Object.keys(flattened.fieldErrors)
            .reduce((acc, field) => {
                acc[field as keyof T] = String(flattened.fieldErrors[field]?.join(", "));
                return acc;
            }, {} as Record<keyof T, string>);
        return { message: message, fieldErrors: fieldErrors };
    }
    if (error instanceof Error)
        return { message: error.message, fieldErrors: {} as Record<keyof T, string> };    
    return { message: "", fieldErrors: {} as Record<keyof T, string> };
}

export const PersonValidations = z.object({
    firstname: z.string().nonempty(),
    lastname: z.string().nonempty(),
    birthDate: z.date().optional().refine((date) => {
        if (!date) return true; // Falls `birthDate` optional ist, lassen wir `undefined` zu.

        const now = new Date();
        const minDate = new Date(now.getFullYear() - 120, now.getMonth(), now.getDate());
        const maxDate = now;

        return date >= minDate && date <= maxDate;
    }, {
        message: "BirthDate must be between 120 years ago and today."
    }),
});
