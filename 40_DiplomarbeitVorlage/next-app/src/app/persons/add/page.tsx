"use client"
import { Dispatch, FormEvent, SetStateAction, useState } from "react";
import { addPerson } from "../actions";
import { createValidationError, isValidationError, ValidationError } from "@/app/utils/validations";
import { redirect } from "next/navigation";
import styles from './page.module.css';
import { signIn, useSession } from "next-auth/react";
import { Person } from "@prisma/client";


async function handleSubmit(event: FormEvent<HTMLFormElement>, setError: Dispatch<SetStateAction<ValidationError<Person>>>) {
    event.preventDefault();
    const result = await addPerson(new FormData(event.currentTarget));
    if (isValidationError<Person>(result)) {
        setError(result);
    } else {
        redirect("/persons");
    }
}

export default function AddPersonPage() {
    // See https://authjs.dev/getting-started/session-management/get-session
    const { data: session } = useSession()

    if (!session || !session.user) {
        // SignIn von next-auth/react, da wir in einer clientseitigen Komponente sind.
        signIn();
    }

    const [error, setError] = useState<ValidationError<Person>>(createValidationError<Person>());
    return (
        <div className={styles.formContainer}>
            <form onSubmit={e => handleSubmit(e, setError)}>
                <div>
                    <label htmlFor="firstname">Vorname</label>
                    <input type="text" name="firstname" />
                    {error.fieldErrors.firstname && <div className={styles.errorMessage}>{error.fieldErrors.firstname}</div>}
                </div>
                <div>
                    <label htmlFor="lastname">Nachname</label>
                    <input type="text" name="lastname" />
                    {error.fieldErrors.lastname && <div className={styles.errorMessage}>{error.fieldErrors.lastname}</div>}
                </div>
                <div>
                    <label htmlFor="birthDate">Geburtsdatum</label>
                    <input type="date" name="birthDate" />
                    {error.fieldErrors.birthDate && <div className={styles.errorMessage}>{error.fieldErrors.birthDate}</div>}
                </div>
                <button type="submit">Speichern</button>
            </form>
            {error.message && <div className={styles.errorContainer}>{error.message}</div>}
        </div>
    );
}
