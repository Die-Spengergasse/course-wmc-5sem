// Vite can import CSS files. In the build, they end up in a separate .css file.
import "./style.css";
import { getElement } from "./dom.js";
import helloWorld from "./hello.js";
import { round } from "./maths.js";
import { area, createCircle, createRectangle, goldenRectangle } from "./shapes/index.js";
import type { Shape } from "./shapes/index.js";

const greeting = getElement<HTMLHeadingElement>("#greeting");
const form = getElement<HTMLFormElement>("#shapeForm");
const kindSelect = getElement<HTMLSelectElement>("#kind");
const sizeInput = getElement<HTMLInputElement>("#size");
const shapeList = getElement<HTMLUListElement>("#shapeList");
const reportButton = getElement<HTMLButtonElement>("#reportButton");
const reportOutput = getElement<HTMLPreElement>("#report");

const shapes: Shape[] = [createCircle(1), goldenRectangle(10)];

/** Creates a shape from the values in the form. */
function shapeFromForm(): Shape {
    const size = sizeInput.valueAsNumber;
    switch (kindSelect.value) {
        case "circle":
            return createCircle(size);
        case "golden":
            return goldenRectangle(size);
        default:
            return createRectangle(size, size);
    }
}

/** Shows all shapes with their area in the list. */
function renderShapes(): void {
    shapeList.replaceChildren();
    for (const shape of shapes) {
        const item = document.createElement("li");
        item.textContent = `${shape.kind}: area ${round(area(shape))}`;
        shapeList.append(item);
    }
}

form.addEventListener("submit", event => {
    event.preventDefault();
    try {
        shapes.push(shapeFromForm());
        renderShapes();
    } catch (error) {
        alert(error instanceof Error ? error.message : String(error));
    }
});

reportButton.addEventListener("click", async () => {
    // report.ts is loaded on the first click. In the build, it is a separate file (chunk).
    const { default: createReport } = await import("./report.js");
    reportOutput.textContent = createReport(shapes).join("\n");
});

greeting.textContent = helloWorld("5AAIF");
renderShapes();
