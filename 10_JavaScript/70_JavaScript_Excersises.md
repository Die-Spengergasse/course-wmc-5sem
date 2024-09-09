# JavaScript Questions and Exercises

## Basics
- **Q1.** What is the difference between `==` and `===`?
- **Q2.** What is the difference between `let`, `const` and `var`?
- **Q3.** What does hoisting mean?
- **Q4.** What is the difference between `null` and `undefined`?
- **Q5.** What is the output of `console.log(null == undefined)` and `console.log(null === undefined)`?
- **Q6.** What is the output of `const x = undefined; if (x) { console.log('yes'); } else { console.log('no') }`?
- **Q7.** What primitive types javascript has?
- **Q8.** What is the object type in javascript?
- **Q9.** What are truthy and falsy values in javascript?
- **Q10.** What does `!!x` do?
- **Q11.** What is the difference between `const x = 1`, `const x = Number(1)`, `const x = new Number(1)`?
- **Q12.** What is the difference between `parseInt('5x')` and `Number('5x')`?
- **Q13.** `const x = { a: 1 }; const y = { a: 1 }; console.log(x === y);` What will be the output?
- **Q14.** `const x = { a: 1 }; const y = x; console.log(x === y);` What will be the output?
- **Q15.** `const arr = [1, 2, 3]; arr[0] = 4; console.log(arr);` What will be the output?
- **Q16.** `const obj = { a: 1, b: 2 }; obj.a = 3; console.log(obj)`; What will be the output?
- **Q17.** What is the output of `console.log(1 + '2')`?
- **Q18.** What is the string template `Hello, my name is ${name} and I like ${hobby}` doing?
- **Q19.** How to check if a variable is from a specific type?
- **Q20.** What is the difference between `for`, `for...in`, `for...of`?
- **Q21.** What is the `typeof` operator in javascript?
- **Q22.** What is type coercion in javascript?
- **Q23.** What is `NaN`? When does it occur? How to check if a value is `NaN`?
- **Q24.** What is the `strict mode` in javascript, and how to enable it? Is it still relevant?
- **E1.** Write a function `isNumber` that takes a parameter and returns `true` if the parameter is a number, otherwise `false`. 

## Short Circuit Evaluation (&&), Ternary Operator (?:)
- **Q1.** What does this function do?
```javascript
function render(isOpen) {
    return isOpen ? `<p>Open</p>` : `<p>Close</p>`;
}
```
- **Q2.** What does this function do?
```javascript
function render(isOpen) {
    return `<footer>${isOpen && `<p>Open</p>`}</footer>`;
}
```
- **Q3.** In what scenarios is the `&&` operator useful?
- **Q4.** In what scenarios is the `?:` operator useful?

- **E1.** Write a function `displayCart` that takes a cart object `{ items: Array } and returns a footer displaying the total amount if there are items in the cart, otherwise "Your cart is empty."

## Optional chaining (?.), Nullish coalescing operator (??)
- **Q1.** How does the optional chaining operator `?.` work? 
- **Q2.** How does the nullish coalescing operator `??` work?
- **Q3.** What will be the output? Explain the lines.
```javascript
const user = { name: 'Alice', address: { city: 'Wonderland' } };

console.log(user.profile);
console.log(user.profile.age);
console.log(user?.profile.age);
console.log(user?.profile?.age);
console.log(user.profile?.age);
console.log(user?.profile?.age ?? 'Unknown');
console.log(user.profile?.age ?? 0);

console.log(user?.address.city);
console.log(user?.address?.city ?? 'Unknown');
console.log(user.address?.city ?? 'Vienna');
```

- **Q4.** Explain the lines.
```javascript
let user;
user.greet();
user?.greet();

user = "Lisa";
user.greet();
```

- **Q5.** What does this function do?
```javascript
function getNumberOfProducts(shop) {
    return shop?.products?.length ?? 0;
}
```

- **Q6.** What does this function do?
```javascript
function getProducts(shop) {
    return shop?.products ?? [];
}
```
- **Q7.** When does the error `Uncaught TypeError: xxx is undefined` occur? Make an example.
- **Q8.** When does the error `Uncaught TypeError: xxx.yyy is not a function` occur? Make an example.
- **E1.** Write a function `getCity` that takes a user object `{ address: { city: String } }` and returns the city if it exists, otherwise "Unknown".

## Functions
- **Q1.** What the difference between functional and object-oriented programming?
- **Q2.** What is the difference between a function declaration and a function expression?
- **Q3.** What is the difference between a regular function and an arrow function (lambda)?
- **Q4.** What is the arity of a function?
- **Q5.** What is a variadic function?
- **Q6.** What is a higher-order function? Make an example with `map` or `filter`
- **Q7.** What is a callback function? Make an example with `setTimeout` or `addEventListener`.
- **Q8.** What is the concept of a closure by explaining the following code?
  https://stackoverflow.com/questions/36636/what-is-a-closure
```javascript
function outerFunction() {
    let counter = 0;

    function innerFunction() {
        counter++;
        console.log(counter);
    }

    return innerFunction;
}
const increment1 = outerFunction();
const increment2 = outerFunction();
increment1();  // Output: ?
increment1();  // Output: ?
increment2();  // Output: ?
```

- **E1.** Create a function `add` that adds 2 numbers in three ways: by function declaration, function expression, and arrow function (lambda).
- **E2.** Create a higher-order function `repeat` that takes a function and a number n, and calls that function n times.
- **E3/BONUS.** Create a function `once` that takes a function `fn` that returns a new function that ensures that `fn` can only be called once. If the function is called more than once, it should return an error or do nothing.
```javascript
const saySusiOnce = once((name) => console.log(`Hello ${name}!`));
saySusiOnce('Susi');  // Output: "Hello Susi!"
saySusiOnce('Susi');  // No output or Error, function can only be called once
```

## Objects
- **Q1.** What are the key differences between objects and arrays?
- **Q2.** What is the type of the key in an object?
- **Q3.** What are possible types of the value in an object?
- **Q4.** What are the dot notation and bracket notation?
- **Q5.** How to iterate over an object? Make an example.
- **Q6.** How can you check if a property exists in an object?. Make example.
- **Q7.** What are getters and setters in an object? Make an example.
- **Q8.** How do you add a new property to an object? Make an example.
- **Q9.** How do you remove a property from an object? Make an example.
- **Q10.** How do you merge two objects together? Make an example.
- **Q11.** What does serialization mean? https://en.wikipedia.org/wiki/Serialization
- **Q12.** What is the difference between an object and JSON? https://www.json.org/json-en.html
- **Q13.** How to serialize an object to a JSON? Make an example.
- **Q14.** What is the difference between a shallow and deep copy?
- **Q15.** How to shallow copy an object? Make an example.
- **Q16.** How to deep copy an object? Make an example.
- **Q17.** What is the difference between `Object.keys`, `Object.values`, `Object.entries`? Make an example.
- **Q18/BONUS** What is the problem `structuredClone` is solving ? https://developer.mozilla.org/en-US/docs/Web/API/structuredClone
- **E1.** Create an object `person` with properties `name` and `age` and a method `greet` that returns a greeting message.
- **E2.** How to iterate over an object by keys, values or both? Take the example from E1 and iterate over it in three ways.
- **E3.** Write a function `merge` that takes two objects and returns a new object with the properties of both objects.
- **E4/BONUS.** Write a function `deepMerge` that takes two objects and returns a new object with the properties of both. Use recursion to merge nested objects. 
- **E5/BONUS.** Write a function `delete` that takes an object and a key and returns a new object without the key.

## Arrays
- **Q1.** What are the key differences between an objects and an arrays?
- **Q2.** What is an iterable?
- **Q3.** What is the type of the key in an array?
- **Q4.** What are possible types of the value in an array?
- **Q5.** What is a sparse array? Does JavaScript have sparse arrays?
- **Q6.** What is an array-like object? And how to convert it to an array?
- **Q7.** How to iterate over an array? Make an example.
- **Q8.** How to iterate over an array with the index? Make an example.
- **Q9.** What is the difference between `map`, `filter`, `reduce`? Make an example.
- **Q10.** How to shallow copy an array? Make an example.
- **Q11.** How to deep copy an array? Make an example.

- **E1.** Write a function `merge` that merges two arrays together.
- **E2.** Create an array `arr` with elements `1, 2, 3` and add a new element `4` to the start/middle/end of the array **without modifying** the original array.
- **E3.** Create an array `arr` with elements `1, 2, 3` and add a new element `4` to the start/middle/end of the array by **modifying** the original array.
- **E4.** Create an array `arr` with elements `1, 2, 3` and remove the element `2` from the array **without modifying** the original array.
- **E5.** Create an array `arr` with elements `1, 2, 3` and remove the element `2` from the array by **modifying** the original array.
- **E6.** Write a function `mapDouble` that takes an array of numbers and returns a new array where each number is doubled using `map`.
- **E7.** Write a function `filterEven` that takes an array of numbers and returns a new array with only even numbers using `filter`.
- **E8.** Write a function `sum` that takes an array of numbers and returns the sum of them using `reduce`.
- **E9/BONUS.** Write a function `find` that takes an array of numbers and a number and returns the first number that is greater than the given number.
- **E10/BONUS.** Write a function `every` that takes an array of numbers and a function and returns `true` if all numbers pass the test.
- **E11/BONUS.** Write a function `some` that takes an array of numbers and a function and returns `true` if at least one number passes the test.

## Spread Syntax, Rest Parameter Syntax, Destructuring Assignment
- **Q1.** What is the spread syntax. Make an example.
- **Q2.** What is the rest parameter syntax. Make an example.
- **Q3.** What is the destructuring assignment. Make an example.
- **Q4/BONUS.** What is the named destructuring assignment. Make an example.
- **Q5/BONUS.** Look at the syntax section, do you understand most examples? https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Destructuring_assignment

- **E1.** Demonstrate the spread/rest syntax with the function `sum` receiving a variadic number of arguments and returning the sum.
- **E2.** Call the function `greet` with an object. Does `greet` use the destructuring assignment, rest parameter syntax, or spread syntax?
```javascript
function greet({ name, age }) {
    console.log(`Hello, my name is ${name} and I am ${age} years old.`);
}
```
- **E3/BONUS.** Enhance `greet` by providing default values.
- **E4/BONUS.** Enhance `greet` by rename the parameters.
- **E5/BONUS.** Enhance `greet` by providing default values and rename the parameters.

## Promises & Async/Await
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
- **Q1.** What is a promise?
- **Q2.** What is the problem promises are trying to solve?
- **Q3.** What is the problem async/await is trying to solve?
- **Q4.** What are the states of a promise?
- **Q5.** What is the difference between `Promise.resolve` and `Promise.reject`?
- **Q6.** What is the difference between `then` and `catch`?
- **Q7.** What is promise chaining?
- **Q8.** What is the difference between `setTimeout` and `Promise`?
- **Q9/BONUS.** What is the difference between `Promise.all` and `Promise.race`?
- **E1.** Write a function `resolve` that returns a promise that resolves with a value `42` after 3 seconds.
- **E2.** Write a function `reject` that returns a promise that rejects with an `Error` object after 3 seconds.
- **E3/BONUS.** Write a function `delay` that pauses for a specified amount of time by a given parameter.
- **E4/BONUS.** Write a function `waitOnKeyPress` that returns a promise that resolves when the enter key is pressed.

## Fetch API
https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API
- **Q1.** What is the Fetch API trying to solve?
- **Q2.** Why does the Fetch API use promises?

- **E1.** Write a function `getTodods` that makes an HTTP GET request to `https://jsonplaceholder.typicode.com` e.g. /todos and writes the result to the console. Consider error handling
- **E2.** Write a function `getTodos` that makes an HTTP GET request to `https://jsonplaceholder.typicode.com` e.g. /todos and writes the result to a list of DOM nodes. Consider error handling
- **E4/BONUS.** Write a function `postTodo` that makes an HTTP POST request to `https://jsonplaceholder.typicode.com` e.g. /todos with a body and writes the result to the console. Consider error handling
- **E3/BONUS.** Write above functions using async/await with try/catch.

## Debugging
- **Q1.** How to open the developer tools in the browser?
- **Q2.** Where to check for errors in the browser?
- **Q3.** Demonstrate the use of the debugger in the browser.
- **Q4.** Demonstrate the use of the `debugger` statement.
- **Q5.** What is the difference between `console.log`, `console.warn`, `console.error`?
- **Q6.** How to inspect the DOM in the browser?
- **Q7.** How to inspect javascript files in the browser?
- **Q8.** How to inspect network requests in the browser?
- **Q9.** How to inspect the storage in the browser?
- **Q10.** How is `$0` useful in the browser's console?
- **Q11/BONUS.** How to inspect the performance of a website in the browser?

## Modules / Import & Export
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules
- **Q1.** What are Javascript modules?
- **Q2.** What is the difference between a Javascript module and a regular JavaScript script?
- **Q3.** Do Javascript scripts/modules have their own scope? What are the consequences?
- **Q4.** Do we need `use strict` directive in Javascript modules?
- **Q5.** How does import/export work in Javascript modules?
- **Q6.** Can JavaScript modules import other JavaScript scripts/modules?
- **Q7.** Can JavaScript modules import HTML files or CSS files?
- **Q8.** Can HTML files import other HTML files?
- **Q9.** Can HTML files import CSS files or Javascript scripts/modules?
- **Q10.** Can CSS files import other CSS files?
- **Q11.** Can CSS files import HTML files or Javascript scripts/modules?
- **Q12/BONUS.** Research the IIFE pattern and why is/was it used? Make an example.
- **Q13/BONUS.** Research the Revealing Module Pattern and why is/was it used? Make an example.
- **Q14/BONUS.** Research what are JavaScript bundlers and what are they used for?
- **E1.** Create a module `math` with functions `add`, `sub`, `mul`, `div` and export them. Import the module in another script and use the functions.

## Set, Map
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Set
- **Q1.** What is a Set?
- **Q2.** What are common Set operations?
- **Q3.** What is the difference between a Set and an Array? When to use what?
- **Q4.** What is a Map?
- **Q5.** What are common Map operations?
- **Q6.** What is the difference between a Map and an Object? When to use what?
- **E1.** Write a function `unique` that takes an array and returns a new array with unique elements.

## Event Loop
https://www.youtube.com/watch?v=qz6yDqjMVfw
- **Q1.** What is the event loop?
- **Q2.** What means to block the event loop and what are the consequences?
- **Q3.** What is a task, microtask, and render step?
- **Q4.** How many frames per second does the browser render?
- **Q5.** What does `setTimeout(() => { console.log('I am back')}, 0);` do?
- **Q6.** What is the difference between `setTimeout` and `requestAnimationFrame`?
- **Q7.** What is the order of the output of the following code?
```javascript
console.log('script start');

setTimeout(function() {
  console.log('setTimeout');
}, 0);

Promise.resolve().then(function() {
  console.log('promise1');
}).then(function() {
  console.log('promise2');
});

console.log('script end');
```
- **E2/BONUS.** Write a function `render` that renders a div block from left to right using `setTimeout`.
- **E3/BONUS.** Write a function `render` that renders a div block from left to right using `requestAnimationFrame`.
- **E4/BONUS.** Place a GIF in an HTML and in your script call a function `block` that blocks the event loop when pressing a button. Explain what is happening.

## JavaScript language
https://developer.mozilla.org/en-US/docs/Web/JavaScript/JavaScript_technologies_overview
https://webreference.com/javascript/basics/versions/
- **Q1.** Who is in charge of the JavaScript language?
- **Q2.** Get an overview of JavaScript versions.
- **Q3.** What falls under the JavaScript core language (ECMAScript)?
- **Q4.** Can JavaScript be executed outside the browser? If so how?
- **Q5.** What is the JavaScript runtime environment?
- **Q6.** What is the difference between `window` and `globalThis`?
- **Q7.** Explore some properties and methods of `window` in the browser console.
- **E1.** Write a function `randomInt` that returns a random integer between start and end inclusive.
- **E2/BONUS.** Write a function `randomDate` that returns a random date between start and end inclusive.
- **E3/BONUS.** Explore the `this` keyword in JavaScript. Make an example.

## Web APIs
https://developer.mozilla.org/en-US/docs/Web/API
- **Q1.** Do Web APIs fall under the JavaScript core language (ECMAScript)?
- **Q2.** Name some famous Web APIs.
- **E1.** Write a function `getLocation` that returns the current location of the user.
- **E2.** Open two browser tabs (both www.orf.at), in one tab set a value with the `LocalStorage` API and read it in the other tab.
- **E3/BONUS.** Open two browser tabs (www.orf.at and www.google.com), repeat the experiment. What happens, try to explain it?

