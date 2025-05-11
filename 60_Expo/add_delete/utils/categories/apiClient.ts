import { Category, isCategory } from "@/types/Category";
import { axiosInstance, createErrorResponse, ErrorResponse } from "@/utils/apiClient";

export type NewCategoryFormData = {
  name: string;
  description: string;
  isVisible: boolean;
  priority: "Low" | "Medium" | "High";
};

export async function getCategories(): Promise<Category[] | ErrorResponse> {
    try {
        const categoriesResponse = await axiosInstance.get<Category[]>("api/categories");
        return categoriesResponse.data.filter(isCategory);
    }
    catch (e) {
        return createErrorResponse(e);
    }
}

export async function deleteCategory(guid: string): Promise<undefined | ErrorResponse> {
    try {
        const categoriesResponse = await axiosInstance.delete(`api/categories/${guid}`);
    }
    catch (e) {
        return createErrorResponse(e);
    }
}

export async function addCategory(data: NewCategoryFormData): Promise<undefined | ErrorResponse> {
    try {
        const categoriesResponse = await axiosInstance.post("api/categories", data);
    }
    catch (e) {
        return createErrorResponse(e);
    }    
}