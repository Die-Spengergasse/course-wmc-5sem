"use client"

import { createEmptyErrorResponse, ErrorResponse, isErrorResponse } from "@/app/utils/apiClient";
import { addCategory } from "./categoryApiClient"
import styles from "./CategoryAdd.module.css"
import { Dispatch, FormEvent, RefObject, SetStateAction, useEffect, useRef, useState } from "react";


async function handleSubmit(event: FormEvent, setError: Dispatch<SetStateAction<ErrorResponse>>,
    formRef: RefObject<HTMLFormElement>) {
    event.preventDefault();
    const response = await addCategory(new FormData(event.target as HTMLFormElement));
    if (isErrorResponse(response))
        setError(response);
    else
        formRef.current?.reset();
}


export default function CategoryAdd() {
    // Für den Renderer brauchen wir ein leeres ErrorResponse object.
    // Sonst würde bei error.validations.name auf ein undefined Objekt zugegriffen werden.
    const [error, setError] = useState<ErrorResponse>(createEmptyErrorResponse());
    const formRef = useRef<HTMLFormElement>(null);
    // Sonst wird alert mehrfach angezeigt, wenn die Komponente mehrmals gerendert wird.
    useEffect(() => { 
        if (error.message) { alert(error.message);} 
    }, [error]);

    return (
        <div>
            <form onSubmit={e => handleSubmit(e, setError, formRef)} ref={formRef} className={styles.categoryAdd}>
                <div>
                    <div>Name</div>
                    <div><input type="text" name="name" required /></div>
                    <div>{error.validations.name && <span className={styles.error}>{error.validations.name}</span>}</div>
                </div>
                <div>
                    <div>Description</div>
                    <div><input type="text" name="description" required /></div>
                    <div>{error.validations.description && <span className={styles.error}>{error.validations.description}</span>}</div>
                </div>
                <div>
                    <div>Visible?</div>
                    <div><input type="checkbox" name="isVisible" /></div>
                    <div>{error.validations.isVisible && <span className={styles.error}>{error.validations.isVisible}</span>}</div>
                </div>
                <div>
                    <div>Priority</div>
                    <div>
                        {["Low", "Medium", "High"].map(p => {
                            const id = `priority_${p}`.toLowerCase();
                            return <label key={p} htmlFor={id}>{p}<input type="radio" id={id} name="priority" value={p} required /></label>
                        })}
                    </div>
                    <div>{error.validations.priority && <span className={styles.error}>{error.validations.priority}</span>}</div>
                </div>
                <div>
                    <div>&nbsp;</div>
                    <div><button type="submit">Submit</button></div>
                </div>
            </form>
        </div>
    )
}
