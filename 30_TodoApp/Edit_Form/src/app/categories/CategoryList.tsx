"use client";
import React, { useRef, useState } from "react";
import { Category } from "@/app/types/Category";
import ModalDialog from "@/app/components/ModalDialog";
import CategoryEdit, { CategoryEditRef } from "./CategoryEdit";
import styles from "./CategoryList.module.css";

export default function CategoryList({ categories }: { categories: Category[] }) {
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
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
                onClick={() => setSelectedCategory(category)}
                title="Edit"
              >
                ✏️
              </span>
            </div>
            <p>{category.description}</p>
          </li>
        ))}
      </ul>

      {selectedCategory && (
        <ModalDialog title={`Edit ${selectedCategory.name}`}
          onOk={() => categoryEditRef.current?.startSubmit()}
          onCancel={() => setSelectedCategory(null)}>
          <CategoryEdit category={selectedCategory}
            ref={categoryEditRef}
            onSubmitted={() => setSelectedCategory(null)} />
        </ModalDialog>
      )}
    </div>
  );
}
