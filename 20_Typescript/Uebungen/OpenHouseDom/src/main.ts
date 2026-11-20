import "./style.css";
import openHouseData from "./data/openHouse.json";
import { getElement } from "./dom.ts";
import type { OpenHouse } from "./types.ts";

// tsc checks that the JSON file matches the interface OpenHouse.
const openHouse: OpenHouse = openHouseData;

const eventTitle = getElement("#eventTitle", HTMLHeadingElement);
const eventInfo = getElement("#eventInfo", HTMLParagraphElement);

const date = new Date(openHouse.date);
eventTitle.textContent = openHouse.event;
eventInfo.textContent = `${openHouse.school}, ${date.toLocaleDateString("en-GB", { dateStyle: "full" })}`;

// TODO: Solve the exercise in the chapter "DOM Tree (Wiederholung)".
