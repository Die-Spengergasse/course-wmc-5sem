import { Dispatch, SetStateAction, useEffect, useState } from "react";
import ModalDialog from "../components/ModalDialog";
import { Category } from "../types/Category";
import { createEmptyErrorResponse, ErrorResponse, isErrorResponse } from "../utils/apiClient";
import { deleteCategory } from "./categoryApiClient";

type CategoryDeleteProps = {
    category: Category;
    onCancel: () => void;
    onDeleted: () => void;
}
async function handleSubmit(
    categoryGuid: string,
    setError: Dispatch<SetStateAction<ErrorResponse>>,
    onDeleted: () => void
) {
    const response = await deleteCategory(categoryGuid);
    if (isErrorResponse(response)) {
        setError(response);
    } else {
        onDeleted();
    }
}


export default function CategoryDelete({ category, onCancel, onDeleted }: CategoryDeleteProps) {
    const [error, setError] = useState<ErrorResponse>(createEmptyErrorResponse());
    useEffect(() => {
        if (error.message) {
            alert(error.message);
        }
    }, [error]);
    return (
        <div>
            <ModalDialog
                title={`Delete Category ${category.name}`}
                onCancel={onCancel}
                onOk={() => handleSubmit(category.guid, setError, onDeleted)}>
                <p>Möchtest du die Kategorie {category.name} wirklich löschen?</p>
            </ModalDialog>
        </div>
    );

}