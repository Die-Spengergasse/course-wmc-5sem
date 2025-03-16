"use client";

import React, { createContext, useContext, useState, ReactNode } from 'react';

/**
 * Typ für die Funktionen zur Änderung des globalen States.
 */
type TodoAppStateActions = {
    /**
     * Setzt die Fehlermeldung im globalen State.
     */
    setError: (value: string) => void;

    /**
     * Setzt den aktiven Benutzer im globalen State.
     */
    setActiveUser: (value: string) => void;
};

/**
 * Typ für den globalen State der Todo-App.
 */
type TodoAppState = {
    /** Enthält eine mögliche Fehlermeldung. */
    error: string;

    /** Speichert den aktuellen aktiven Benutzer. */
    activeUser: string;
};

/**
 * Der vollständige Typ für den Context, der den State und die Actions enthält.
 */
type TodoAppContextType = TodoAppState & { actions: TodoAppStateActions };

/** Erstellt einen Context für den globalen Todo-App-Status. */
const TodoAppContext = createContext<TodoAppContextType | undefined>(undefined);

/**
 * Globaler State-Provider für die Todo-App.
 * Stellt den State und Funktionen zur Aktualisierung zur Verfügung.
 */
export function TodoAppStateProvider({ children }: { children: ReactNode }) {
    const [state, setState] = useState<TodoAppState>({ error: "", activeUser: "" });

    /**
     * Aktualisiert die Fehlermeldung im State.
     * @param {string} value - Die neue Fehlermeldung.
     */
    const setError = (value: string) => setState(prev => ({ ...prev, error: value }));

    /**
     * Aktualisiert den aktiven Benutzer im State.
     * @param {string} value - Der Benutzername des aktiven Nutzers.
     */
    const setActiveUser = (value: string) => setState(prev => ({ ...prev, activeUser: value }));

    return (
        <TodoAppContext.Provider value={{ ...state, actions: { setError, setActiveUser } }}>
            {children}
        </TodoAppContext.Provider>
    );
}

/**
 * Hook zum Zugriff auf den globalen Todo-App-Status.
 *
 * @throws {Error} Falls der Hook außerhalb eines `TodoAppStateProvider` verwendet wird.
 * @returns {TodoAppContextType} Der aktuelle State und die Actions.
 */
export function useTodoAppState() {
    const context = useContext(TodoAppContext);
    if (!context) {
        throw new Error('useTodoAppState must be used within a TodoAppStateProvider.');
    }
    return context;
}
