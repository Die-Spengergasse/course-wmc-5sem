import https from "https";
import TodosClient from "./TodosClient";
import { isTodoItem } from "../types/TodoItem";
import { isCategory } from "../types/Category";
import { axiosInstance } from "../utils/apiClient";

export default async function TodosPage() {
  const agent = new https.Agent({
    rejectUnauthorized: false
  });

  // Categories laden, um das Dropdown befüllen zu können.
  const categoriesResponse = await axiosInstance.get("categories", { httpsAgent: agent });
  const categories = categoriesResponse.data.filter(isCategory);

  // TodoItems laden, um die Items anzeigen zu können
  const todoItemsResponse = await axiosInstance.get("todoItems", { httpsAgent: agent });
  const todoItems = todoItemsResponse.data.filter(isTodoItem);


  return <TodosClient todoItems={todoItems} categories={categories} />;
}
