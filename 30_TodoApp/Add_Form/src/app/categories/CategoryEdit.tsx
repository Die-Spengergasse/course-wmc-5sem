import { Dispatch, FormEvent, SetStateAction, useImperativeHandle, useRef, useState } from "react";
import { createEmptyErrorResponse, ErrorResponse, isErrorResponse } from "../utils/apiClient";
import { editCategory } from "./categoryApiClient";
import styles from "./CategoryEdit.module.css";
import { Category } from "../types/Category";

export type CategoryEditRef = {
    startSubmit: () => void;
}

type CategoryEditProps = {
    category: Category;
    onSubmitted: () => void;
    ref?: React.Ref<CategoryEditRef>;
}

async function handleSubmit(
    event: FormEvent,
    setError: Dispatch<SetStateAction<ErrorResponse>>,
    onSubmitted: () => void
) {
    event.preventDefault();
    const response = await editCategory(new FormData(event.target as HTMLFormElement));
    if (isErrorResponse(response)) {
        setError(response);
    } else {
        onSubmitted();
    }
}

export default function CategoryEdit({ category, onSubmitted, ref }: CategoryEditProps) {
    const [error, setError] = useState<ErrorResponse>(createEmptyErrorResponse());
    const formRef = useRef<HTMLFormElement>(null);

    useImperativeHandle(ref, () => ({
        startSubmit: () => formRef.current?.requestSubmit()
    }));

    return (
        <div>
            <form
                ref={formRef}
                onSubmit={(e) => handleSubmit(e, setError, onSubmitted)}
                className={styles.categoryEdit}
            >
                <input type="hidden" name="guid" value={category.guid} />
                <div>
                    <div>Name</div>
                    <div>
                        <input type="text" name="name" defaultValue={category.name} required />
                    </div>
                    <div>
                        {error.validations.name && (
                            <span className={styles.error}>{error.validations.name}</span>
                        )}
                    </div>
                </div>
                <div>
                    <div>Description</div>
                    <div>
                        <textarea name="description" defaultValue={category.description} required />
                    </div>
                    <div>
                        {error.validations.description && (
                            <span className={styles.error}>{error.validations.description}</span>
                        )}
                    </div>
                </div>
                <div>
                    <div>Visible?</div>
                    <div>
                        <input type="checkbox" name="isVisible" defaultChecked={category.isVisible} />
                    </div>
                    <div>
                        {error.validations.isVisible && (
                            <span className={styles.error}>{error.validations.isVisible}</span>
                        )}
                    </div>
                </div>
                <div>
                    <div>Priority</div>
                    <div>
                        {["Low", "Medium", "High"].map((p) => {
                            const id = `priority_${p}`.toLowerCase();
                            return (
                                <label key={p} htmlFor={id}>
                                    {p}
                                    <input
                                        type="radio"
                                        id={id}
                                        name="priority"
                                        value={p}
                                        defaultChecked={category.priority === p}
                                        required
                                    />
                                </label>
                            );
                        })}
                    </div>
                    <div>
                        {error.validations.priority && (
                            <span className={styles.error}>{error.validations.priority}</span>
                        )}
                    </div>
                </div>
            </form>
        </div>
    );
}