/**
 * Finds an element with querySelector and checks its type at runtime.
 * The second argument is the class of the element, e.g. HTMLInputElement.
 * The function throws if the element does not exist or has a different type.
 * So the caller gets the type T instead of Element | null.
 * @example const input = getElement("#search", HTMLInputElement);
 */
export function getElement<T extends Element>(selector: string, type: new () => T): T {
    const element = document.querySelector(selector);
    if (element === null) {
        throw new Error(`Element ${selector} not found.`);
    }
    // instanceof narrows Element to T, so no type assertion (as) is needed.
    if (!(element instanceof type)) {
        throw new Error(`Element ${selector} is not a ${type.name}.`);
    }
    return element;
}
