"use server";
import { ErrorResponse, axiosInstance, createErrorResponse } from "@/app/utils/apiClient";
import { revalidatePath } from "next/cache";
import { Category, isCategory } from "../types/Category";

export async function getCategories(): Promise<Category[] | ErrorResponse> {
    try {
        const categoriesResponse = await axiosInstance.get<Category[]>("categories");
        return categoriesResponse.data.filter(isCategory);
    }
    catch (e) {
        return createErrorResponse(e);
    }
}

/**
 * Fügt eine neue Kategorie über die API hinzu.
 *
 * Diese Funktion sendet ein Formular mit den Kategorieninformationen an die API 
 * und gibt entweder eine Erfolgs- oder Fehlerantwort zurück.
 *
 * @param {FormData} formData - Die Formulardaten mit den Kategorieninformationen.
 * @returns {Promise<SuccessResponse | ErrorResponse>} 
 *    - Eine `SuccessResponse`, wenn die API die Kategorie erfolgreich verarbeitet hat.
 *    - Eine `ErrorResponse`, wenn ein Fehler aufgetreten ist.
 */
export async function addCategory(formData: FormData): Promise<ErrorResponse | undefined> {
    // Extrahiere Daten aus dem Formular
    const data = {
        name: formData.get("name"),
        description: formData.get("description"),
        isVisible: !!formData.get("isVisible"),     // converts null to false.
        priority: formData.get("priority"),
    };

    try {
        // Sende einen POST-Request an die API
        await axiosInstance.post("categories", data);
        revalidatePath("/categories");
    } catch (e) {
        return createErrorResponse(e);
    }
}
