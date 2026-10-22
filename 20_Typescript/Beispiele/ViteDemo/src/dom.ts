/**
 * Finds an element with querySelector and throws if it does not exist.
 * So the caller gets the type T instead of T | null.
 * @example const input = getElement<HTMLInputElement>("#size");
 */
export function getElement<T extends Element>(selector: string): T {
    const element = document.querySelector<T>(selector);
    if (element === null) {
        throw new Error(`Element ${selector} not found.`);
    }
    return element;
}
