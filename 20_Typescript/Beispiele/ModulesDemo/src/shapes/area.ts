import { circleArea } from "./circle.js";
import { rectangleArea } from "./rectangle.js";
import type { Shape } from "./shape.js";

/** Calculates the area of any shape. */
export function area(shape: Shape): number {
    switch (shape.kind) {
        case "circle":
            return circleArea(shape);
        case "rectangle":
            return rectangleArea(shape);
    }
}
