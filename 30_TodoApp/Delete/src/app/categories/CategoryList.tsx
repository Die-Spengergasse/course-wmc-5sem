"use client";
import React, { useReducer, useRef } from "react";
import { Category } from "@/app/types/Category";
import ModalDialog from "@/app/components/ModalDialog";
import CategoryEdit, { CategoryEditRef } from "./CategoryEdit";
import styles from "./CategoryList.module.css";
import CategoryDelete from "./CategoryDelete";

// Discriminated unions in typescript
type ReducerAction =
  | { resetState: true }
  | { resetState?: false; intent: "edit" | "delete"; category: Category };
type CategoryListState =
  | { dialogType: "" }
  | { dialogType: "error"; error: string }
  | { dialogType: "edit" | "delete"; category: Category };

function reducer(
  state: CategoryListState,
  action: ReducerAction): CategoryListState {
  if (action.resetState) return { dialogType: "" }
  switch (action.intent ) {
    case "edit":
      if (action.category.isVisible)
        return { category: action.category, dialogType: "edit" };
      else
        return { dialogType: "error", error: "You cannot edit an invisible category." }
    case "delete":
      return { category: action.category, dialogType: "delete" };
    default:
      return { dialogType: "" };
  }
}
export default function CategoryList({ categories }: { categories: Category[] }) {
  const [state, dispatcher] = useReducer(reducer, { dialogType: "" });
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
                onClick={() => dispatcher({ intent: "edit", category: category })}
                title="Edit"
              >
                ✏️
              </span>
              <span
                className={styles.editIcon}
                onClick={() => dispatcher({ intent: "delete", category: category })}
                title="Delete"
              >
                🗑️
              </span>
            </div>
            <p>{category.description}</p>
            <p>Visible: {category.isVisible ? "yes" : "no"}</p>
          </li>
        ))}
      </ul>

      {state.dialogType == "error" && (
        <ModalDialog title="Error"
          onOk={() => dispatcher({ resetState: true })}
          onCancel={() => dispatcher({ resetState: true })}>
          {state.error}
        </ModalDialog>

      )}
      {state.dialogType == "edit" && (
        <ModalDialog title={`Edit ${state.category.name}`}
          onOk={() => categoryEditRef.current?.startSubmit()}
          onCancel={() => dispatcher({ resetState: true })}>
          <CategoryEdit category={state.category}
            ref={categoryEditRef}
            onSubmitted={() => dispatcher({ resetState: true })} />
        </ModalDialog>
      )}
      {state.dialogType == "delete" && (
        <CategoryDelete category={state.category}
          onCancel={() => dispatcher({ resetState: true })}
          onDeleted={() => dispatcher({ resetState: true })} />
      )}
    </div>
  );
}
