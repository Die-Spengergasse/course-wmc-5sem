/** A dish on the menu of the school canteen. */
export interface Dish {
    id: number;
    name: string;
    category: string;
    price: number;
    vegetarian: boolean;
}

/** The weekly menu, as stored in data/menu.json. */
export interface Menu {
    canteen: string;
    week: string;
    dishes: Dish[];
}
