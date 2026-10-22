// CommonJS: the old module system of Node.js (before 2015).
// Run it with: node legacy/maths.cjs
const pi = Math.PI;

function absolute(value) {
    return value < 0 ? -value : value;
}

// Export: assign an object to module.exports.
module.exports = { pi, absolute };

// Import: require() is a normal function call and can appear anywhere.
// require.main === module is true if this file was started directly with node.
if (require.main === module) {
    const { pi: myPi, absolute: abs } = require("./maths.cjs");
    console.log(`CommonJS: pi = ${myPi}, |-3| = ${abs(-3)}`);
}
