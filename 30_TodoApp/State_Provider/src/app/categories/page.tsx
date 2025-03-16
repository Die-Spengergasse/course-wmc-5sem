import CategoryList from "./CategoryList";
import CategoryAdd from "./CategoryAdd";
import { getCategories } from "./categoryApiClient";
import { isErrorResponse } from "../utils/apiClient";

export default async function CategoryPage() {
  const response = await getCategories();

  return (
    <div>
      <h1>Categories</h1>
      {!isErrorResponse(response) ? (
        <div>
          <CategoryList categories={response} />
          <h2>Add category</h2>
          <CategoryAdd />
        </div>
      )
        : <div className="errorbox">{response.message}</div>}

    </div>
  );
}
