# TypeScript Questions and Exercises

## Basics
- **Q1.** What is TypeScript, and how does it differ from JavaScript?
- **Q2.** Does TypeScript code run natively in the browser?
- **Q3.** What are the benefits of using TypeScript over JavaScript?
- **Q4.** How does TypeScript help prevent runtime errors compared to JavaScript?
- **Q5.** What is a type in TypeScript, and how is it useful?
- **Q6.** What is type inference in TypeScript?
- **Q7.** How do you declare a variable with a specific type in TypeScript?
- **Q8.** What happens when no type is specified for a variable in TypeScript?
- **Q9.** What happens when a wrong type is assigned to a variable in TypeScript?
- **Q10.** How do you compile TypeScript code to JavaScript?
- **Q11.** What is the difference between `interface` and `type` in TypeScript?
- **Q12.** What is a union type in TypeScript, and how is it useful?
- **Q13.** What is an intersection type in TypeScript, and how is it useful?
- **Q14.** What are literal types in TypeScript?
- **Q15.** What is a type guard in TypeScript, and how does it help with type checking?
- **Q16.** What is type assertion, and when should you use it and when to avoid it?
- **Q17.** What is the difference between `unknown` and `any` in TypeScript, and when should each be used?
- **Q18.** What does `readonly` do in TypeScript, and how does it differ from `const`?
- **Q19.** What is the `?` do in TypeScript, and how does it relate to optional properties?
- **Q20.** What is `tsconfig.json`, and why is it important in a TypeScript project?
- **Q21.** What is `d.ts` file in TypeScript, and how is it used?


- **E1.** Write a function `addNumbers` that takes two parameters of type `number` and returns their sum.
  - Use type annotations to specify the parameter and return types.
  - Call the function with two numbers and log the result to the console.
  - Call the function with a number and a string and observe the error.
  
    
- **E2.** Create an `interface` called `Person` with properties `name` of type `string` and `age` of type `number`.
  - Create a variable `person` of type `Person` and assign it an object with `name` and `age` properties.
  - Try to assign a new object with only `name` property to `person` and observe the error.


- **E3.** Define a union type called `ID` that can be either a `string` or a `number`.
  - Write a function `printID` that takes an `ID` and prints it to the console.


## Functions
- **Q1.** What is a type guard in TypeScript?
- **Q2.** How can you create a type guard function?
- **Q3.** How do rest parameters work in TypeScript functions, and how can they be typed effectively?
- **Q4.** How does TypeScript handle default parameter values in functions, and what are the advantages?
- **Q5.** What are callback functions, and how can you type them properly in TypeScript?
- **Q6.** How can you use function types to define signatures for callbacks or higher-order functions?
- **Q7.** How can `this` be typed in TypeScript functions? Provide an example where explicitly typing `this` is useful.
- **Q8.** How does TypeScript's strict mode affect function parameter types and return types?
- **Q11.** How do you declare a function that never returns?
- **Q12.** What are optional parameters in TypeScript?
- **Q13.** How do you implement method overloading in TypeScript, providing a simple example?
- **Q13.** Does JavaScript support method overloading?


- **E1.** Write a function `add` which takes either `number` or `string` inputs and returns the sum.
  - Use a type guard to differentiate between `number` and `string` inputs.
  - Use function overloading to define both behaviors.


- **E2.** Write a function `processBlob` that can either take a single `Blob` object or an array of `Blob` objects.
  - If a single `Blob` is provided, log its size and type.
  - If an array of `Blob` objects is provided, log the size and type for each blob.
  - Ensure that image Blobs (`image/*` MIME type) are identified with a special message.
  - Use a type guard to differentiate between a single `Blob` and an array of `Blob` objects.
  - Use function overloading to define both behaviors.
  - Blob Link: https://developer.mozilla.org/en-US/docs/Web/API/Blob


## Object Types
- **Q1.** How do you define an object type using an `interface`?
- **Q2.** How can you extend an existing `interface` in TypeScript?
- **Q3.** How can you define a method in an `interface`?
- **Q4.** How can you use optional properties in an `interface`?
- **Q5.** How do you define a `readonly` property in an `interface`?
- **Q6.** What is the use of index signatures in TypeScript?
- **Q7.** What is declaration merging in TypeScript and what does trying to solve?
- **Q8.** When to use `interface` and `type` alias when defining objects in TypeScript?
- **Q9.** How can you use intersection types to combine multiple types?
- **Q10.** What is a union type, and how does it compare to intersection types?
- **Q11.** What is a discriminated union in TypeScript, and how is it useful?
- **Q10.** How can you use type assertions to convert an object to a specific type?


- **E1.** Define an `interface` for a `User` object for a social media app.
  - The `User` interface has properties and some sub-interfaces: `Profile`, `Account`, and `Media`.
  - Think about what properties these sub-interfaces should have based on a typical social media user.
  - You can also include more properties as needed to create a comprehensive `User` interface.
  - `User` should at least include `id`, `email`, `createdAt`, `avatar`, `profile`, `account`.
  - `avatar` should be optional and from the `Media` type.
  - `Profile` interface could include interests and location.
  - `Account` interface could include status and verification.
  - `Media` interface could describe a media object with url, mime type, and size.
  - MimeType Link: https://developer.mozilla.org/en-US/docs/Web/HTTP/MIME_types


- **E2.** Create a `type` called `Machine` with a base type `{ brand: string; year: number; }` and a specialization types.
  - Specialization Types are `{ type: 'industrial'; capacity: number; }` or `{ type: 'transport'; speed: number; }`.
  - Write a function `calculateMachineOutput` that uses type guards and the Angular guard pattern with `never` to calculate either:
  - The yearly production for an industrial machine (`capacity * 365` minus a wear factor of 5% each year after the first year).
  - The yearly distance for a transport machine (`speed * 365`, but reduce the speed by 10% every year).
  - Ensure all cases are exhaustively handled by using the `never` type or the Angel guard pattern.


## Generics
- **Q1.** What are generics in TypeScript, and how do they help with type safety?
- **Q3.** Provide an example of a generic function in TypeScript.
- **Q4.** What is the purpose of type constraints with generics (`extends`)?
- **Q6.** What is the difference between a generic function and a generic class? Provide examples.
- **Q7.** How can you use multiple type parameters in a generic function?
- **Q8.** How can you use default type parameters in generics?
- **Q9.** What is a utility type, and how do you use `Partial` or `Readonly`?


- **E1.** How does TypeScript's generic type constraint (extends) help in the following function definition?
  - Explain how the extends constraint works in this context.
  - Provide an example of calling logLength with different types of arguments.

    ```typescript
    function logLength<T extends { length: number }>(item: T): void {
      console.log(item.length);
    }
    ```


- **E2.** Create a generic function `identity` that returns the value passed to it.
  - Use the generic type to ensure type safety.


- **E3.** Create a generic class `Box` that holds a value of any type.
    - Add methods to set and get the value.
  

- **E4.** Create a generic function `mergeObjects` that merges two objects of the same type.
    - The function should return a new object with the properties of both input objects.
    - Use type constraints to ensure that the input objects have the same type.
  

- **E5** Create a generic function `mergeObjects` that merges two objects of different types.
    - The function should return a new object with the properties of both input objects.
    - Use type constraints to ensure that the input objects have different types.
    - 

- **E4.** Create a generic function `filterByType` that filters an array of objects based on a property and value.
    - The function should take an array of objects, a property name, and a value to filter by.
    - The function should return a new array with only the objects that match the property and value.
    - Use type constraints to ensure that the property exists on the objects.


## Enums & Literal Types
- **Q1.** What is an `enum` in TypeScript, and how is it used?
- **Q2.** How do you define an `enum` with custom values?
- **Q3.** What are the pros and cons of using `enums` in TypeScript?
- **Q4.** What is a string literal type, and how is it different from an `enum`?
- **Q5.** How can you use literal types to create a type-safe function?
- **Q6.** What is a discriminated union and how does it benefit from literal types?
- **Q7.** How can you use literal types to create a type-safe switch statement?
- **Q8.** What is the benefit of using literal types over string literals in TypeScript?

- **E1.** Create an `enum` called `Color` with values `Red`, `Green`, `Blue`,
  - Write a function that takes a `Color` and prints a message based on the color.
  - Call the function with different colors.


- **E2.** Create a literal type for directions (`"left" | "right" | "up" | "down"`).
  - Write a function that takes a direction and logs a message based on the direction.
  - Call the function with different directions.


- **E3.** How do literal types help in creating discriminated unions in TypeScript. Demonstrate with an example:
    - Create an interface `Circle` with a `type` property of type `"circle"` and a `radius` property of type `number`.
    - Create an interface `Square` with a `type` property of type `"square"` and a `sideLength` property of type `number`.
    - Create a type `Shape` that is a discriminated union of `Circle` and `Square`.
    - Write a function `calculateArea` that takes a `Shape` and returns the area of the shape.
    - Use switch statement with literal types to handle the different shapes.
    - Ensure all cases are exhaustively handled by using the `never` type or the Angel guard pattern.