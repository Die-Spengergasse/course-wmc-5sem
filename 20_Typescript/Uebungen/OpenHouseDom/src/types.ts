/** A station on the open house day, e.g. a workshop in a lab. */
export interface Station {
    id: number;
    title: string;
    department: string;
    room: string;
    /** Start time as "HH:MM", e.g. "09:30". */
    start: string;
    /** End time as "HH:MM", e.g. "10:15". */
    end: string;
}

/** The program of the open house day, as stored in data/openHouse.json. */
export interface OpenHouse {
    event: string;
    school: string;
    /** Date as "YYYY-MM-DD". */
    date: string;
    stations: Station[];
}
