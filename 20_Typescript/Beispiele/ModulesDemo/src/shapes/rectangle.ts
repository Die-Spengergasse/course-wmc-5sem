import { absolute, phi } from "../maths.js";
import type { Rectangle } from "./shape.js";

/**
 * Creates a rectangle. Negative values are turned into positive ones.
 * @param width The width of the rectangle.
 * @param height The height of the rectangle.
 */
export function createRectangle(width: number, height: number): Rectangle {
    return { kind: "rectangle", width: absolute(width), height: absolute(height) };
}

/**
 * Creates a "golden" rectangle: width / height is the golden ratio phi.
 * @param height The height of the rectangle.
 */
export function goldenRectangle(height: number): Rectangle {
    return createRectangle(height * phi, height);
}

/** Calculates the area of a rectangle: width * height. */
export function rectangleArea(rectangle: Rectangle): number {
    return rectangle.width * rectangle.height;
}
