import "./style.css";
import menuData from "./data/menu.json";
import { getElement } from "./dom.ts";
import { createDishItem, createTrayItem, formatPrice } from "./views.ts";
import type { Dish, Menu } from "./types.ts";

// tsc checks that the JSON file matches the interface Menu.
const menu: Menu = menuData;
const tray: Dish[] = [];

const title = getElement("#title", HTMLHeadingElement);
const week = getElement("#week", HTMLParagraphElement);
const filterForm = getElement("#filterForm", HTMLFormElement);
const searchInput = getElement("#search", HTMLInputElement);
const veggieCheckbox = getElement("#veggieOnly", HTMLInputElement);
const dishList = getElement("#dishList", HTMLUListElement);
const trayList = getElement("#trayList", HTMLOListElement);
const total = getElement("#total", HTMLElement);

/** Shows all dishes that match the search text and the checkbox. */
function renderDishes(): void {
    const search = searchInput.value.trim().toLowerCase();
    const visible = menu.dishes.filter(dish =>
        dish.name.toLowerCase().includes(search)
        && (!veggieCheckbox.checked || dish.vegetarian));
    // replaceChildren removes the old items and appends the new ones.
    dishList.replaceChildren(...visible.map(createDishItem));
}

/** Shows the dishes on the tray and the total price. */
function renderTray(): void {
    trayList.replaceChildren(...tray.map(createTrayItem));
    const sum = tray.reduce((result, dish) => result + dish.price, 0);
    total.textContent = formatPrice(sum);
}

// The form fires "input" for every change in the search field or the checkbox.
filterForm.addEventListener("input", renderDishes);
// Enter in the search field would submit the form and reload the page.
filterForm.addEventListener("submit", event => event.preventDefault());

// One listener for all buttons in the list (event delegation).
dishList.addEventListener("click", event => {
    if (!(event.target instanceof HTMLButtonElement)) {
        return;
    }
    const id = Number(event.target.dataset.id);
    const dish = menu.dishes.find(d => d.id === id);
    if (dish !== undefined) {
        tray.push(dish);
        renderTray();
    }
});

trayList.addEventListener("click", event => {
    if (event.target instanceof HTMLButtonElement) {
        tray.splice(Number(event.target.dataset.index), 1);
        renderTray();
    }
});

title.textContent = menu.canteen;
week.textContent = `Menu from ${menu.week}`;
renderDishes();
renderTray();
