"use client";

import { useState, useEffect } from "react";
import { useTodoAppState } from "../context/TodoAppContext";
import styles from "./ErrorViewer.module.css";

export default function ErrorViewer() {
    const todoAppState = useTodoAppState();
    const [visible, setVisible] = useState(false);

    useEffect(() => {
        if (todoAppState.error) {
            setVisible(true);

            // Fehlernachricht nach 5 Sekunden automatisch ausblenden
            const timer = setTimeout(() => {
                setVisible(false);
                todoAppState.actions.setError("");
            }, 5000);

            return () => clearTimeout(timer);
        }
    }, [todoAppState.error]);

    return (
        <footer className={`${styles.errorViewer} ${visible ? styles.show : styles.hide}`}>
            <div className={styles.errorContent}>
                <p>{todoAppState.error}</p>
                <button
                    type="button"
                    className={styles.errorButton}
                    onClick={() => {
                        setVisible(false);
                        todoAppState.actions.setError("")
                    }}
                >
                    OK
                </button>
            </div>
        </footer>
    );
}
