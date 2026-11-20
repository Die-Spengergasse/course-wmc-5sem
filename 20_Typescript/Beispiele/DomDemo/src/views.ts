import type { Dish } from "./types.ts";

const priceFormat = new Intl.NumberFormat("de-AT", { style: "currency", currency: "EUR" });

/** Formats a price in euros, e.g. 6.2 becomes "€ 6,20". */
export function formatPrice(price: number): string {
    return priceFormat.format(price);
}

/**
 * Creates the list item for a dish on the menu:
 * <li class="dish veggie"><span>Pumpkin soup</span>
 *   <span class="price">€ 2,90</span><button data-id="1">Add</button></li>
 */
export function createDishItem(dish: Dish): HTMLLIElement {
    const item = document.createElement("li");
    item.classList.add("dish");
    item.classList.toggle("veggie", dish.vegetarian);

    const name = document.createElement("span");
    name.textContent = dish.name;

    const price = document.createElement("span");
    price.classList.add("price");
    price.textContent = formatPrice(dish.price);

    const button = document.createElement("button");
    button.type = "button";
    button.textContent = "Add";
    // data-id: the click handler finds the dish with this id.
    button.dataset.id = String(dish.id);

    item.append(name, price, button);
    return item;
}

/** Creates the list item for a dish on the tray, with a button to remove it. */
export function createTrayItem(dish: Dish, index: number): HTMLLIElement {
    const item = document.createElement("li");
    item.textContent = `${dish.name} (${formatPrice(dish.price)}) `;

    const button = document.createElement("button");
    button.type = "button";
    button.textContent = "Remove";
    button.dataset.index = String(index);

    item.append(button);
    return item;
}
