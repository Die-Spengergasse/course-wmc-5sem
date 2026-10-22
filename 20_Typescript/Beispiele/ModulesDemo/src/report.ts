import { round } from "./maths.js";
import { area, type Shape } from "./shapes/index.js";

/**
 * Creates a report with the area of every shape and the total area.
 * The module is loaded with import() only when the user asks for a report.
 * @returns One string per line of the report.
 */
export default function createReport(shapes: Shape[]): string[] {
    const lines = shapes.map((shape, index) => `${index + 1}. ${shape.kind}: ${round(area(shape))}`);
    const total = shapes.reduce((sum, shape) => sum + area(shape), 0);
    lines.push(`Total area: ${round(total)}`);
    return lines;
}
