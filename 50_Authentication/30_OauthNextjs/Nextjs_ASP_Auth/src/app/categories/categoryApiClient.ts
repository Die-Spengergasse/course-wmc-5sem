"use server";
import { ErrorResponse, createErrorResponse } from "@/app/utils/apiClient";
import { revalidatePath } from "next/cache";
import { Category, isCategory } from "@/app/types/Category";
import { getAxiosInstance } from "../utils/getAxiosInstance-server";


export async function getCategories(): Promise<Category[] | ErrorResponse> {
    try {
        const axiosInstance = await getAxiosInstance();
        const categoriesResponse = await axiosInstance.get<Category[]>("/api/categories");
        return categoriesResponse.data.filter(isCategory);
    }
    catch (e) {
        return createErrorResponse(e);
    }
}

export async function getCategory(guid: string): Promise<Category | ErrorResponse> {
    try {
        const axiosInstance = await getAxiosInstance();
        const categoryResponse = await axiosInstance.get<Category>(`/api/categories/${guid}`);
        if (isCategory(categoryResponse.data)) {
            return categoryResponse.data;
        }
        return createErrorResponse(new Error("Invalid category data"));
    }
    catch (e) {
        return createErrorResponse(e);
    }
}

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
        const axiosInstance = await getAxiosInstance();
        await axiosInstance.post("/api/categories", data);
        revalidatePath("/categories");
    } catch (e) {
        console.error(e);
        return createErrorResponse(e);
    }
}

export async function editCategory(formData: FormData): Promise<ErrorResponse | undefined> {
    // Extrahiere Daten aus dem Formular
    const guid = formData.get("guid");
    if (!guid) {
        return createErrorResponse(new Error("Invalid guid"));
    }
    const data = {
        guid: guid,
        name: formData.get("name"),
        description: formData.get("description"),
        isVisible: !!formData.get("isVisible"),     // converts null to false.
        priority: formData.get("priority"),
    };

    try {
        // Sende einen PUT-Request an die API
        const axiosInstance = await getAxiosInstance();
        await axiosInstance.put(`/api/categories/${guid}`, data);
        revalidatePath("/categories");
    } catch (e) {
        return createErrorResponse(e);
    }
}

export async function deleteCategory(guid: string): Promise<ErrorResponse | undefined> {
    try {
        const axiosInstance = await getAxiosInstance();
        await axiosInstance.delete(`/api/categories/${guid}`);
        revalidatePath("/categories");
    } catch (e) {
        return createErrorResponse(e);
    }
}
