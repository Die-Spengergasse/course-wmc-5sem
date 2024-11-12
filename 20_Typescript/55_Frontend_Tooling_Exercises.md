# FrontEnd Tooling Questions and Exercises

## TypeScript

- **Q1.** Why do we have our own `tsconfig.json` file in the project folder?
- **Q2.** What is the purpose of the `"lib"` field in `tsconfig.json`?
- **Q4.** Try to provoke a TypeScript error and observe the error message according to the `tsconfig.json` configuration.


## ESBuild

- **Q1.** What is a bundler, and why do we use ESBuild for bundling in this project?
- **Q2.** What is a minifier, and how does ESBuild's minification improve the efficiency of our code?
- **Q3.** What is the difference between a transpiler and a compiler, and where does `tsc` and `ESBuild` fit in?
- **Q4.** Why are we using ESBuild instead of the TypeScript compiler `tsc` for building?


## ESM Resolution

- **Q1.** What are ES modules and why are they used in modern JavaScript development?
- **Q2.** How are ES modules resolved in the browser and Node.js?
- **Q3.** Why do we need to add the `.js` extension in import statements when using TypeScript compiler (`tsc`)?
- **Q4.** Why can we remove the `.js` extension in import statements when using ESBuild to bundle into a single file?
- **Q5.** Why do we need to add the `.js` extension in import statements when using ESBuild to bundle into multiple files?


## ESLint

- **Q1.** What is a linting tool, and why do we use ESLint in this project?
- **Q2.** What do we have a `.eslint.config.js` file in the project folder?
- **Q3.** What are custom rules in ESLint, and how can they help maintain code quality?


## Prettier

- **Q1.** What is a code formatter, and why do we use Prettier in this project?
- **Q2.** What do we have a `.prettier.config.js` file in the project folder?


## Jest

- **Q1.** What is a testing framework, and why do we use Jest in this project?
- **Q2.** What do we have a `jest.config.js` file in the project folder?
- **Q3.** What is a test suite, and how do you create one in Jest?
- **Q4.** What is a test case, and how do you write one in Jest?
- **Q6.** What is describe and it in Jest, and how do they help organize tests?
- **Q6.** What is an expectation, and how do you write one in Jest?
- **Q7.** What is a matcher, and how do you use it in Jest?
- **Q8.** What is BDD (Behavior-Driven Development), and how does it relate to Jest?


## TypeScript Project
The project should demonstrate all the tooling and testing concepts covered in the frontend tooling guide.
  - Create README.md: Describe the project, how to run it, and how to run tests.
  - `src/user/user-models.ts`: Implement the interfaces for `User`, `Profile`, `Account`, and `Media` from the typescript exercises.
  - `src/user/user-utils.ts`: Implement user functions as described below.
  - `src/main.ts`: Import the functions from `user-utils.ts` and use them to demonstrate their functionality.
  - `test/user-utils.test.ts`: Write tests for the functions in `user-utils.ts`.
  - Init git repository and commit the initial project structure to GitHub.
  - After each tooling setup step (ESBuild, ESLint, Prettier, Jest), commit the changes to the repository.
  - BONUS: Add an `index.html` file to demonstrate the project in the browser.
  - BONUS: What are pre-commit hooks, and why are they useful?
  - BONUS: Add pre-commit hooks with a library called `Husky` to run ESLint and Prettier before committing called `lint-staged`.
  - BONUS: Add GitHub Actions for ESLint, Prettier, and Jest to run on pull requests or pushes to the main branch.
  ```typescript
  // Add avatar to the user if not already present
  export function addMediaToUser(user: User, media: Media): User {
    // TODO
  }
  
  // Filter users by location
  export function filterUsersByLocation(users: User[], location: string): User[] {
    // TODO
  }
  
  // Calculate the total age of users
  export function calculateTotalAge(users: User[]): number {
  // TODO
  }
  ```

