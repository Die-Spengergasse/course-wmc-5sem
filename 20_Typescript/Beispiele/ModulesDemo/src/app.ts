// Default import: we choose the name ourselves.
import helloWorld from "./hello.js";
// Named imports: the names must match the exports.
import { pi, phi, absolute } from "./maths.js";
// Rename an import with "as".
import { round as roundTo } from "./maths.js";
// Namespace import: all exports as properties of one object.
import * as maths from "./maths.js";
// Import from a folder via its index.ts (re-exports).
import { area, createCircle, createRectangle, goldenRectangle } from "./shapes/index.js";
import type { Shape } from "./shapes/index.js";

console.log(helloWorld("5AAIF"));
console.log(`pi = ${roundTo(pi, 4)}, phi = ${roundTo(phi, 4)}, |-3| = ${absolute(-3)}`);
console.log(`Same function via namespace: ${maths.round(maths.phi, 3)}`);

const shapes: Shape[] = [
    createCircle(1),
    createRectangle(-4, 2.5),
    goldenRectangle(10)
];

for (const shape of shapes) {
    console.log(`${shape.kind}: area ${roundTo(area(shape), 2)}`);
}

// Dynamic import: report.ts is only loaded when the program runs with --report.
if (process.argv.includes("--report")) {
    const { default: createReport } = await import("./report.js");
    createReport(shapes).forEach(line => console.log(line));
}
