import { Category, isCategory } from "@/types/Category";
import { axiosInstance, createErrorResponse, ErrorResponse } from "@/utils/apiClient";

export async function getCategories(): Promise<Category[] | ErrorResponse> {
    try {
        const categoriesResponse = await axiosInstance.get<Category[]>("api/categories");
        return categoriesResponse.data.filter(isCategory);
    }
    catch (e) {
        return createErrorResponse(e);
    }
}
