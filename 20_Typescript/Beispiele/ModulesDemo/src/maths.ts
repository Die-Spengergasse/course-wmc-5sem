/** Ratio of a circle's circumference to its diameter. */
export const pi = Math.PI;

/** The golden ratio, about 1.618. */
export const phi = (1 + Math.sqrt(5)) / 2;

// Not exported: other modules cannot see this constant.
const defaultDigits = 2;

/**
 * Returns the absolute value of a number.
 * @param value Any number, e.g. -3.
 * @returns The value without its sign, e.g. 3.
 */
export function absolute(value: number): number {
    return value < 0 ? -value : value;
}

/**
 * Rounds a number to a given number of decimal places.
 * @param value The number to round.
 * @param digits Number of decimal places, 2 if omitted.
 */
export function round(value: number, digits: number = defaultDigits): number {
    const factor = 10 ** digits;
    return Math.round(value * factor) / factor;
}
