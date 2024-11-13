# FrontEnd Tooling Guide

## 1. Tooling Overview

<p float="left">
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/Node.js_logo_2015.svg/2560px-Node.js_logo_2015.svg.png" style="margin-left: 50px; margin-top: 20px; margin-bottom: 20px" width="150"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/d/db/Npm-logo.svg/1200px-Npm-logo.svg.png" style="margin-left: 20px; margin-top: 20px; margin-bottom: 20px" width="75"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/Typescript_logo_2020.svg/2048px-Typescript_logo_2020.svg.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
  <img src="https://cdn.icon-icons.com/icons2/3914/PNG/512/esbuild_logo_icon_248931.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/ESLint_logo.svg/546px-ESLint_logo.svg.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
  <img src="https://prettier.io/icon.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
  <img src="https://cdn.freebiesupply.com/logos/large/2x/jest-logo-png-transparent.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
  <img src="https://playwright.dev/img/playwright-logo.svg" style="margin-left: 20px; margin-top: 0px; margin-bottom: 10px" width="75"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/f/f1/Vitejs-logo.svg/410px-Vitejs-logo.svg.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="50"/>
</p>

### JavaScript Runtimes
- **Node**: A JavaScript runtime that allows you to run JavaScript outside the browser environment.
- **Deno**: A JavaScript/TypeScript runtime improving Node, focusing on security and developer experience.


### Package Managers
- **NPM**: A package manager for managing dependencies and packages in JavaScript projects.
- **PNPM**: A disk-space-efficient package manager that uses hard links and symlinks to save space.


### Languages, Compilers and Bundlers
- **TypeScript**: A statically typed superset of JavaScript providing type safety at compile time.
- **ESBuild**: A fast JavaScript and TypeScript compiler, bundler and minifier.


### Linters and Formatters
- **ESLint**: A tool for identifying and reporting on problematic patterns in JavaScript code, ensuring code quality.
- **Prettier**: An opinionated code formatter that enforces consistent code style across the codebase.


### Testing Frameworks
- **Jest**: A testing framework for writing unit and integration tests.
- **Playwright**: A testing framework that automates browsers for end-to-end testing.


### Build Tools
- **Vite**: A fast build tool for modern JavaScript projects with a built-in development server.


## 2. Node and NPM

<p float="left">
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/Node.js_logo_2015.svg/2560px-Node.js_logo_2015.svg.png" style="margin-left: 50px; margin-top: 20px; margin-bottom: 20px" width="400"/>
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/d/db/Npm-logo.svg/1200px-Npm-logo.svg.png" style="margin-left: 50px; margin-top: 20px; margin-bottom: 20px" width="200"/>
</p>

### Overview
- **Link:** https://nodejs.org/en/
- **Purpose**: Node is a JavaScript runtime that allows you to run JavaScript outside the browser environment.
- **Developed By**: Ryan Dahl in 2009
- **JS Conf 2009**: [Ryan Dahl's Original Node.js Presentation](https://www.youtube.com/watch?v=ztspvPYybIY)


### Key Components
- **Node**: A JavaScript runtime that allows you to run JavaScript outside the browser environment.
- **NPM**: A package manager for managing dependencies and packages in JavaScript projects.
- **NPX**: A tool to execute Node packages without installing them globally, ideal for one-off command-line uses.


### Installation
* **Install:** https://nodejs.org/en/download
* **Update:** https://stackoverflow.com/questions/6237295/how-can-i-update-node-js-and-npm-to-their-latest-versions

```sh
# Check Node version
node -v

# Check NPM version
npm -v

# Check NPX version
npx -v
```


### Node Basic Commands
```sh
# Start a Node.js REPL
node

# Exit the REPL
Ctrl + C twice

# Run a JavaScript file
node <file_name>.js
```


### NPM Essentials
- **NPM Package**: A collection of JavaScript files that can be reused in different projects.
- **Package Manager**: A command line tool that finds, installs, and manages packages.
- **Package Registry**: A storage location where packages are published and installed from.
- **Scripts and Automation**: npm can run custom scripts for tasks like testing, building, and more, helping automate project workflows.
- **Dependency Management**: npm manages package versions and dependencies through `package.json` and `package-lock.json`.


### NPM Packages
- **Global vs. Local Packages**: Global packages are installed once and can be used across projects, while local packages are installed per project.
- **package.json**: A file that lists project dependencies, scripts, and metadata.
- **package-lock.json**: A file that locks down the exact versions of dependencies to ensure consistent installations.
- **node_modules**: A folder that contains all the installed packages for a project, managed by npm.


### NPM Basic Commands
```sh
# Creates a new project folder in Linux/Mac
mkdir <project-name>
cd <project-name>

# Initialize a new `npm project` (creates `package.json`)
npm init

# Installs a specific package (updates `package.json` and `node_modules`)
npm install <package_name>

# Update a package in `package.json`
npm update <package_name>

# Uninstall a package from `package.json`
npm uninstall <package_name>

# Install a package globally to use as a command line tool
npm install -g <package_name>

# Run a script from package.json
npm run <script_name>

# Upgrade npm to the latest version
npm install -g npm@latest
```


### NPM Demo Project
```sh
# Initialize npm
npm init

# Install Jest as a development dependency
npm install jest --save-dev

# Create a `src` and `test` folder
mkdir src
mkdir test

# Create `src/main.js`
console.log("Hello, World!");

# Create `test/main.test.js`
test('sample test', () => {
  expect(1 + 1).toBe(2);
});

# Run the project using npm scripts
npm run start
npm run test
```


### The `package.json` file
```json
{
  "name": "my-project",
  "version": "1.0.0",
  "description": "Demo Project",
  "type": "module",
  "main": "src/main.js",
  "scripts": {
    "start": "node src/main.js",
    "test": "jest"
  },
  "devDependencies": {
    "jest": "^27.0.6"
  }
}
```

- **`name`**: The name of your project. It must be unique if published.
- **`version`**: The current version of your project, following semantic versioning.
- **`description`**: A brief description of what your project does.
- **`main`**: The entry point of your application which is executed when running `node`.
- **`type`**: The module type, either `module` or `commonjs`. Set it to `module` for EcmaScript Modules (ESM) which uses `import/export` syntax.
- **`scripts`**: Defines a set of commands that can be run using `npm run`, such as starting the application or running tests.
- **`dependencies`**: Lists the packages required for your project to run. These are installed when running `npm install`.
- **`devDependencies`**: Lists the packages only needed during development (e.g., testing or building tools).


### The `node_modules` folder
- **Purpose**: Stores all the packages installed for the project.
- **Version Control**: Excluded from version control (in `.gitignore`) since it can be re-created with `npm install`.

### The `node_modules` folder issues 🤔
<img src="https://miro.medium.com/v2/resize:fit:4800/format:webp/1*x9s00J9jmkdl8QG18F_iIw.png" style="margin-left: 50px; margin-top: 20px; margin-bottom: 10px" width="500"/>

- **Nested Dependencies**: Each package can have its own dependencies, leading to a large number of files.
- **Transitive Dependencies**: Dependencies of dependencies can also be installed, leading to a large tree of packages.
- **Duplication**: The same package can be installed multiple times at different versions, leading to duplication.
- **Further Reading**: [Why is node_modules so big?](https://alphacoder.xyz/node-modules/)


## 3. `npm init`, `npm create`, and `npx`

### What Are They?
- **`npm init`** and **`npm create`** are commands for initializing new projects in Node.
- **`npm init`** is used both for creating a basic `package.json` file and for initializing projects from a template.
- **`npm create`** is an alias of `npm init`.


### How Do They Work?
- `npm init` or `npm create` without an initializer will to create a basic `package.json` file for your project.
- `npm init` or `npm create` with an initializer (e.g., `react-app`), it prepends "create-" to the initializer name.
- Internally it is transformed to `npm exec create-<initializer>` (e.g. `npm exec create-react-app`).
- Additionally, npm **downloads** the initializer package (e.g., `create-react-app`) if it's not already downloaded and then **executes** the corresponding binary.
- `npx create` with an initializer **temporarily downloads** the package and then **executes** it, similar to npm create.


### Examples
- **Basic Initialization**
```sh
# Create a new plain npm project with a basic `package.json` file.
npm init
npm create
  ```
- **Create a Project Using a Template**
```sh
# Create a new project using a template.
npm init <initializer>
npm create <initializer>
npx create-<initializer>

# Create a new React project using `create-react-app` template.
# Requires the package `create-react-app` to be either installed globally or will install it permanently.
npm init react-app my-app
npm create react-app my-app

# Does not require a global install; installs the package temporarily for this use.
npx create-react-app my-app
```


### Overview

| Command                     | What It Does                                      | Internal Transformation                    |
|-----------------------------|---------------------------------------------------|--------------------------------------------|
| `npm init`                  | Initializes a new project (e.g., `package.json`). | No transformation.                         |
| `npm create`                | Alias of `npm init`, initializes a project.       | No transformation.                         |
| `npm init <initializer>`    | Creates a new project using a template.           | Turns into `npm exec create-<initializer>` |
| `npm create <initializer>`  | Same as `npm init <initializer>`.                 | Turns into `npm exec create-<initializer>` |
| `npx create-<initializer>`  | Runs a package directly without global install.   | Direct execution, no transformation.       |


## 4. Semantic Versioning

<img src="https://digitalcommunications.wp.st-andrews.ac.uk/files/2017/01/semver03.png" style="margin-left: 50px; margin-top: 20px; margin-bottom: 20px" width="400"/>

### Overview
- **Link:** https://semver.org/
- **Purpose**: Semantic versioning is a versioning scheme that helps manage project dependencies.
- **Format**: Versions are in the format `MAJOR.MINOR.PATCH` (e.g., `1.2.3`).


### Versioning Scheme

- **MAJOR (Breaking)**: Indicates breaking changes that are not backward compatible.
- **MINOR (Feature)**: Introduces new features that are backward compatible.
- **PATCH (Fix)**: Implements bug fixes that are backward compatible.
- **Label**: Additional labels like `alpha`, `beta`, or `rc` can be added for pre-release versions.
- **Example**: `1.2.3-rc.1` indicates version `1.2.3` release candidate 1.


### Versioning Operators

- **Exact Version:** Only version `27.0.6` is installed.
  ```sh
  "jest": "27.0.6"
  ```

- **Tilde (`~`)**: Updates to patch versions only.
  ```sh
  "jest": "~27.0.6"
  ```

- **Caret (`^`)**: Updates to minor and patch versions.
  ```sh
  "jest": "^27.0.6"
  ```


### Versioning Best Practices

- **Caret (^) for Libraries**: Use `^` for libraries to get new features and bug fixes.
- **Tilde (~) for Apps**: Use `~` for applications to get bug fixes but avoid new features.
- **Lockfile**: Use `package-lock.json` to ensure consistent installations across environments.
- **Update Regularly**: Update packages regularly to get security patches and new features.
- **Automated Testing**: Run tests after updating to catch any breaking changes early.
- **Audit**: Use `npm audit` to check for vulnerabilities in your dependencies.


### Updating Packages
- **Update all packages**: `npm update` updates all packages to their latest versions based on the versioning scheme.
- **Update a single package**: `npm update <package_name>` updates a specific package to its latest version based on the versioning scheme.


## 5. TypeScript Project Guide

- This project demonstrates setting up a TypeScript project with ESBuild, ESLint, Prettier, and Jest.
- We gradually add tooling to the project to improve code quality, maintainability, and testing.


### Project Setup
```sh
# Create a new project folder
mkdir my-typescript-project
cd my-typescript-project

# Initialize a new npm project
npm init

# Install Typescript locally in the project
npm install --save-dev typescript

# Initialize Typescript configuration (creates `tsconfig.json`)
npx tsc --init

# Create `src` and `test` folders
mkdir src
mkdir test
```


### Project Structure

```markdown
my-project/
├── node_modules/               # Dependencies
│   └── ...
├── dist/                       # Compiled output from ESBuild
│   └── ...
├── src/                        # Sources
│   ├── main.ts                 # Main Entry Point of the Application
│   └── ...
├── test/                       # Tests
│   ├── main.test.ts            # Jest Test
│   └── ...
├── package.json                # Project Config and Dependencies
├── tsconfig.json               # TypeScript Config
├── eslint.config.js            # ESLint Config
├── prettier.config.js          # Prettier Config
├── jest.config.js              # Jest Config
├── .gitignore                  # Git Ignore
└── README.md                   # Project Documentation
```


### Configuration (`tsconfig.json`)
- **Link**: https://www.typescriptlang.org/tsconfig

```json
{
  "compilerOptions": {
    /* Language and Environment */
    "target": "ES2020",                         // Specify ECMAScript target version for the compiled JavaScript
    "module": "ESNext",                         // Specify module code generation (use the latest module system)
    "lib": ["ES2020", "DOM", "DOM.Iterable"],   // Include library files for ES2020, DOM APIs, and DOM Iterables (for browser)
    //"lib": ["ES2020],                         // Include library files for ES2020 (for Node)
    "outDir": "./dist",                         // Specify the output directory for compiled JavaScript files
    "rootDir": "./src",                         // Specify the root directory of TypeScript source files

    /* Enable Strict Type-Checking */
    "strict": true,                             // Enable all strict type-checking options for TypeScript

    /* Additional Error Checks */
    "noUnusedLocals": true,                     // Report errors for variables declared but not used within the code
    "noUnusedParameters": true,                 // Report errors for parameters defined but not used in functions
    "noImplicitReturns": true,                  // Report errors if not all code paths in a function have return statements
    "noFallthroughCasesInSwitch": true,         // Report errors for fall-through cases in switch statements without a break
    "noUncheckedSideEffectImports": true,       // Report errors on imports with side effects that are unused
    "noUncheckedIndexedAccess": true,           // Report errors when accessing dynamic properties without handling undefined
    "noPropertyAccessFromIndexSignature": true, // Report errors if using dot notation to access dynamic properties

    /* Paths and Module Aliases */
    "paths": {},                                // Configure path mapping for module imports

    /* Compatibility */
    // "types": ["node"],                       // Automatically include type definitions (for Node)
    "esModuleInterop": true,                    // Enable interop compatibility for importing CommonJS modules (e.g. Jest)
    "useDefineForClassFields": true,            // Emit class fields with `define` semantics
    "forceConsistentCasingInFileNames": true,   // Ensure consistent casing in module imports
    "skipLibCheck": true,                       // Skip type-checking of declaration files (.d.ts) for faster builds
    
    /* Debugging */
    "sourceMap": true                           // Generate source maps for debugging TypeScript in the browser or IDE
    
    /* Declaration Files */
    // "declaration": true,                     // Generate declaration files (.d.ts) for TypeScript code
    // "declarationDir": "dist/types",          // Output directory for declaration files
  },
  "include": ["src/**/*"],                      // Include all TypeScript source files in the src directory
  "exclude": ["node_modules"]                   // Exclude the node_modules directory from compilation
}
```


### Initial `package.json`
```json
{
  "name": "my-typescript-project",
  "version": "1.0.0",
  "description": "Typescript Demo Project",
  "type": "module",
  "main": "dist/main.js",
  "scripts": {
    "build": "tsc",
    "start": "npm run build && node dist/main.js"
  },
  "devDependencies": {
    "typescript": "^5.6.3"
  }
}
```


### Example Code
```javascript
// src/utils.ts
export function greet(name) {
  console.log(`Hello, ${name}!`);
}

// src/main.ts
import { greet } from './utils.js';
greet("World");
```


### Run Scripts
```sh
# Run TypeScript compiler to compile the project into JavaScript (slow for large projects)
npm run build

# Build and run the JavaScript main.js file in the dist folder
npm run start
```


### Exploration Tasks
- After compiling the TypeScript code, explore the generated JavaScript code in the `dist` folder.
- Make errors in the TypeScript code to see how TypeScript catches them during compilation.
- What happens if you remove `.js` from the import statement in `main.ts`? (Hint: `ESM Resolution`)


## 6. ESBuild

<img src="https://cdn.icon-icons.com/icons2/3914/PNG/512/esbuild_logo_icon_248931.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="200"/>


### Overview
- **Link**: https://esbuild.github.io/
- **Purpose**: A fast JavaScript and TypeScript compiler, bundler, and minifier.
- **Developed By**: Evan Wallace and open-source contributors.


### Key Features
- **Speed**: Written in Go, ESBuild is significantly faster compared to traditional JavaScript bundlers and the TypeScript compiler `tsc`.
- **JavaScript Bundling**: Built-in support for JavaScript, TypeScript, JSX, and ESM/CommonJS modules.
- **CSS Bundling**: Supports bundling of CSS, including CSS modules, which makes managing styles in large applications more efficient.
- **Minifier**: Performs code minification to reduce bundle size, enabling better load performance for production deployments.


### Installation
```sh
# Install ESBuild locally in the project
npm install --save-dev esbuild
```

### Update `package.json`
```json
{
  // ..
  "main": "dist/bundle.js",
  "scripts": {
    "type-check": "tsc --noEmit",
    "build": "esbuild src/main.ts --bundle --minify --outfile=dist/bundle.js",
    // "build": "esbuild src/**/*.ts --outdir=dist"
    "start": "npm run build && node dist/bundle.js"
  },
  // ..
}
```


### Configuration
- **Link**: https://esbuild.github.io/getting-started/#build-scripts


### Run Scripts
```sh
# Run TypeScript compiler for type checking, but don't emit any files (slow for large projects)
npm run type-check

# Run ESBuild to transpile Typescript to JavaScript (fast for large projects)
npm run build
```


### Exploration Tasks
- Explore ESBuild as a `bundler` to produce a single JavaScript output file vs ESBuild as a `transpiler` to produce multiple JavaScript files.
- What happens if you remove `.js` from the import statement in `main.ts`? (Hint: `ESM Resolution`)


## 7. ESLint

<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/ESLint_logo.svg/546px-ESLint_logo.svg.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="200"/>

### Overview
* **Link**: https://eslint.org/
* **Purpose**: A tool for identifying and reporting on problematic patterns in JavaScript code, ensuring code quality.
* **Developed By**: Nicholas C. Zakas and open-source contributors.


### Key Features
- **Static Code Analysis**: ESLint performs static code analysis, identifying problematic patterns and potential errors in your code.
- **Customizable**: Allows you to customize rules to match your project's coding standards and best practices.
- **Supports**: Works with JavaScript, TypeScript, JSX, and can be extended with plugins.
- **Integration**: Works seamlessly with popular editors like VS Code, IntelliJ, and can be used with Git hooks.


### Installation
```sh
# Install ESLint locally in the project
npm install --save-dev eslint

# Initialize ESLint configuration (creates `eslint.config.js`)
npx eslint --init
```


### Update `package.json`
```json
{
  "scripts": {
    // ..
    "lint": "eslint \"{src,test}/**/*.ts\" --fix",
  },
}
```


### Configuration (`eslint.config.js`)
- **Link**: https://eslint.org/docs/latest/use/configure/configuration-files#configuration-objects

```javascript
import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";

/** @type {import('eslint').Linter.Config[]} */
export default [
  {files: ["**/*.{js,mjs,cjs,ts}"]},
  {languageOptions: { globals: globals.browser }},
  pluginJs.configs.recommended,         // JavaScript recommended rules
  ...tseslint.configs.recommended,      // TypeScript recommended rules

  // Add custom linting rules here
  {
    rules: {
      "semi": ["error", "always"],      // Enforce semicolons
      "quotes": ["error", "single"],    // Enforce single quotes
      "prefer-template": ["error"]      // Enforce using template literals
    },
  },
];
```


### Run Scripts
```sh
# Run ESLint to identify and fix linting issues in TypeScript files
npm run lint
```


### Exploration Tasks
- Make intentional linting errors in the TypeScript code to see how ESLint catches and fixes them.
- What if you remove the `--fix` flag from the ESLint script?


## 8. Prettier

<img src="https://prettier.io/icon.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="200"/>

### Overview
* **Link**: https://prettier.io/
* **Purpose**: An opinionated code formatter that enforces consistent style across projects.
* **Developed By**: James Long and contributors.


### Key Features
- **Consistent Code Style**: Ensures that all code in a project looks the same, reducing code review churn.
- **Supports**: Works with JavaScript, TypeScript, CSS, HTML, JSON, and more.
- **Integration**: Works seamlessly with popular editors like VS Code, IntelliJ, and can be used with Git hooks.


### Installation
```sh
# Install Prettier locally in the project
npm install --save-dev prettier
```

### Update `package.json`
```json
{
  "scripts": {
    // ..
    "format": "prettier --write \"{src,test}/**/*.ts\"",
  },
}
```


### Configuration (`prettier.config.js`)
- **Link**: https://prettier.io/docs/en/configuration.html

```javascript
/** @type {import("prettier").Config} */
export default {
  trailingComma: "all",   // Comma at the end of the line
  tabWidth: 2,            // 2 spaces for indentation
  semi: true,             // Enforce semicolons
  singleQuote: true,      // Single quotes for strings
};
```


### Run Scripts
```sh
# Run Prettier to format TypeScript files
npm run format
```


### Exploration Tasks
- Make intentional formatting errors in the TypeScript code to see how Prettier catches and fixes them.
- Explore the Prettier configuration options to customize the code formatting.
- What happens if you remove the `--write` flag from the Prettier script?


## 9. Jest

<img src="https://cdn.freebiesupply.com/logos/large/2x/jest-logo-png-transparent.png" style="margin-left: 20px; margin-top: 0px; margin-bottom: 20px" width="200"/>

### Overview

- **Link**: https://jestjs.io/
- **Purpose**: A delightful JavaScript testing framework with a focus on simplicity.
- **Developed By**: Facebook and open-source contributors.


### Key Features

- **Zero Configuration**: Works out of the box for most JavaScript projects with minimal configuration required.
- **Unit Testing**: Provides a simple and easy-to-use API for writing unit tests.
- **Snapshot Testing**: Allows you to take a snapshot of your components and test whether they remain the same over time.
- **Built-in Matchers**: Includes built-in matchers for common assertions like `toBe`, `toEqual`, and `toMatch`.
- **Code Coverage**: Automatically collects code coverage to see how well your tests are covering the codebase.


### Installation
```sh
# Install Jest locally to the project with TypeScript support
npm install --save-dev jest @types/jest ts-jest

# Initialize Jest configuration (creates `jest.config.js`)
npx ts-jest config:init
```


### Update `package.json`
```json
{
  "scripts": {
    // ..
    "test": "jest",
  },
}
```


### Configuration (`jest.config.js`)
- **Link**: https://jestjs.io/docs/configuration

```javascript
/** @type {import('ts-jest').JestConfigWithTsJest} **/
export default {
  preset: 'ts-jest',            // Typescript support
  testEnvironment: "node",      // Environment for running tests
  roots: ['<rootDir>/test'],    // Directories to search for tests
};
```

### Run Scripts
```sh
# Run Jest to execute tests in the `test` directory
npm run test
```


### Writing Tests

```typescript
// src/add.ts
export function add (a: number, b: number): number {
  if (a < 0 || b < 0) { throw new Error("Numbers must be positive"); }
  return a + b;
}
```

```typescript
// test/add.test.ts
import { add } from '../src/add.js';

describe('add function', () => {
  it('should return the correct sum of two positive numbers', () => {
    expect(add(2, 3)).toBe(5);
  });

  it('should throw an error for negative numbers', () => {
    expect(() => add(-2, 3)).toThrow("Numbers must be positive");
  });
});
```


### Explanation
- **`describe`**: Groups related tests. In this case, it groups tests for the `add` function.
- **`it`**: Defines individual test cases. Each `it` block describes a specific scenario to test.
- **`expect`**: This is the assertion that checks if the actual output (`result`) matches the expected output (`toBe(5)`, etc.).



### Exploration Tasks
- Make intentional errors in the test cases to see how Jest catches and reports them.
- Explore Jest's built-in matchers and assertions to understand how to write effective tests.