// The public interface of the folder "shapes".
// Other modules import from "./shapes/index.js" and do not need to know the files inside.
export type { Circle, Rectangle, Shape } from "./shape.js";
export { createCircle } from "./circle.js";
export { createRectangle, goldenRectangle } from "./rectangle.js";
export { area } from "./area.js";
