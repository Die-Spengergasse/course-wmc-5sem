"use client"
import { useEffect, useState } from "react";
import axios from "axios";
import https from "https";
import { TodoItem, isTodoItem } from "./types/TodoItem";
import { Category, isCategory } from "./types/Category";

export default function Home() {
  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>("");

  // Wenn wir im Dropdownfeld eine Kategorie auswählen, sollen nur die Todo Items dieser Kategorie angezeigt werden.
  const filteredTodoItems = selectedCategory
    ? todoItems.filter(item => item.categoryName === selectedCategory)
    : todoItems;

  useEffect(() => {
    // Da wir axios.get mit await verwenden, muss diese Funktion async sein.
    async function fetchData() {
      const agent = new https.Agent({
        rejectUnauthorized: false
      });

      try {
        // Todo Items abrufen
        const todoResponse = await axios.get("https://localhost:5443/api/TodoItems", { httpsAgent: agent });
        const filteredTodos = todoResponse.data.filter(isTodoItem);
        setTodoItems(filteredTodos);

        // Kategorien abrufen, um das Dropdownfeld zu befüllen.
        const categoryResponse = await axios.get("https://localhost:5443/api/Categories", { httpsAgent: agent });
        const filteredCategories = categoryResponse.data.filter(isCategory);
        setCategories(filteredCategories);
      } catch (error) {
        console.error(error);
      }
    };
    // Die Funktion wird ohne await aufgerufen. Bei useEffect können wir keine async Funktion übergeben.
    // Siehe https://react.dev/reference/react/useEffect#fetching-data-with-effects
    fetchData();
  }, []);

  return (
    <div>
      <h1>Todo Liste</h1>
      <select onChange={(event)=>setSelectedCategory(event.target.value)}>
        <option value="">Alle Kategorien</option>
        {categories.map(category => (
          <option key={category.guid} value={category.name}>
            {category.name}
          </option>
        ))}
      </select>

      <ul>
        {filteredTodoItems.map(item => (
          <li key={item.guid}>
            <h2>{item.title}</h2>
            <p>{item.description}</p>
            <p>Kategorie: {item.categoryName}</p>
            <p>Fällig am: {new Date(item.dueDate).toLocaleDateString()}</p>
            <p>Status: {item.isCompleted ? "Abgeschlossen" : "Ausstehend"}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
