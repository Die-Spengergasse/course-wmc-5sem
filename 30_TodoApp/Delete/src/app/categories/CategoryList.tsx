"use client";
import React, { useReducer, useRef } from "react";
import { Category } from "@/app/types/Category";
import ModalDialog from "@/app/components/ModalDialog";
import CategoryEdit, { CategoryEditRef } from "./CategoryEdit";
import styles from "./CategoryList.module.css";
import CategoryDelete from "./CategoryDelete";

// Discriminated unions in typescript
type EditDeleteReducerAction =
  | { type: null; category: null }                   // Wenn actionType null ist, ist category null
  | { type: "edit" | "delete"; category: Category }; // Wenn actionType "edit" oder "delete" ist, ist category vom Typ Category
type EditDeleteState =
  | { actionType: null; category: null }
  | { actionType: "edit" | "delete"; category: Category };

function editDeleteReducer(
  state: EditDeleteState,
  action: EditDeleteReducerAction): EditDeleteState {
  switch (action.type) {
    case "edit":
      return { category: action.category, actionType: "edit" };
    case "delete":
      return { category: action.category, actionType: "delete" };
    default:
      return { category: null, actionType: null };
  }
}
export default function CategoryList({ categories }: { categories: Category[] }) {
  const [selectedCategory, selectedCategoryDispatch] = useReducer(editDeleteReducer, { category: null, actionType: null });
  const categoryEditRef = useRef<CategoryEditRef>(null);
  return (
    <div className={styles.categories}>
      <ul>
        {categories.map(category => (
          <li key={category.guid}>
            <div className={styles.categoryHeader}>
              <h2>{category.name}</h2>
              <span
                className={styles.editIcon}
                onClick={() => selectedCategoryDispatch({ type: "edit", category: category })}
                title="Edit"
              >
                ✏️
              </span>
              <span
                className={styles.editIcon}
                onClick={() => selectedCategoryDispatch({ type: "delete", category: category })}
                title="Delete"
              >
                🗑️
              </span>
            </div>
            <p>{category.description}</p>
          </li>
        ))}
      </ul>

      {selectedCategory.actionType == "edit" && (
        <ModalDialog title={`Edit ${selectedCategory.category.name}`}
          onOk={() => categoryEditRef.current?.startSubmit()}
          onCancel={() => selectedCategoryDispatch({ type: null, category: null })}>
          <CategoryEdit category={selectedCategory.category}
            ref={categoryEditRef}
            onSubmitted={() => selectedCategoryDispatch({ type: null, category: null })} />
        </ModalDialog>
      )}
      {selectedCategory.actionType == "delete" && (
        <CategoryDelete category={selectedCategory.category}
          onCancel={() => selectedCategoryDispatch({ type: null, category: null })}
          onDeleted={() => selectedCategoryDispatch({ type: null, category: null })} />
      )}
    </div>
  );
}
