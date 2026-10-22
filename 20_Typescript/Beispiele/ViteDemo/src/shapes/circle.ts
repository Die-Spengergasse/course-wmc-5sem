import { pi } from "../maths.js";
import type { Circle } from "./shape.js";

/**
 * Creates a circle. A negative radius is not allowed.
 * @param radius The radius, must be 0 or greater.
 */
export function createCircle(radius: number): Circle {
    if (radius < 0) {
        throw new Error(`Invalid radius: ${radius}`);
    }
    return { kind: "circle", radius };
}

/** Calculates the area of a circle: pi * r². */
export function circleArea(circle: Circle): number {
    return pi * circle.radius ** 2;
}
