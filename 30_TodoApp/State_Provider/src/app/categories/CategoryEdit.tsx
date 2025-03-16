import React, { Dispatch, FormEvent, useEffect, useImperativeHandle, useRef, useState, SetStateAction } from "react";
import { createEmptyErrorResponse, ErrorResponse, isErrorResponse } from "@/app/utils/apiClient";
import { Category } from "@/app/types/Category";
import { editCategory } from "./categoryApiClient";
import styles from "./CategoryEdit.module.css";

export type CategoryEditRef = {
  startSubmit: () => void;
}

type CategoryEditProps = {
  category: Category;
  onSubmitted: () => void;
  ref?: React.Ref<CategoryEditRef>; // Ref as prop
}

async function handleSubmit(
  event: FormEvent,
  setError: Dispatch<SetStateAction<ErrorResponse>>,
  onSubmitted: () => void
) {
  event.preventDefault();
  const response = await editCategory(new FormData(event.target as HTMLFormElement));
  if (isErrorResponse(response)) {
    setError(response);
  } else {
    onSubmitted();
  }
}

export default function CategoryEdit(props: CategoryEditProps) {
  const { category, onSubmitted, ref } = props;
  const formRef = useRef<HTMLFormElement>(null);
  const [error, setError] = useState<ErrorResponse>(createEmptyErrorResponse());

  // UseImperativeHandle for custom methods exposed to the parent
  useImperativeHandle(ref, () => ({
    startSubmit: () => {
      formRef.current?.requestSubmit();
    },
  }));

  useEffect(() => {
    if (error.message) {
      alert(error.message);
    }
  }, [error]);

  return (
    <div>
      <form
        onSubmit={(e) => handleSubmit(e, setError, onSubmitted)}
        ref={formRef}
        className={styles.categoryEdit}
      >
        <input type="hidden" name="guid" value={category.guid} />
        <div>
          <div>Name</div>
          <div>
            <input type="text" name="name" defaultValue={category.name} required />
          </div>
          <div>
            {error.validations.name && (
              <span className={styles.error}>{error.validations.name}</span>
            )}
          </div>
        </div>
        <div>
          <div>Description</div>
          <div>
            <textarea name="description" defaultValue={category.description} required />
          </div>
          <div>
            {error.validations.description && (
              <span className={styles.error}>{error.validations.description}</span>
            )}
          </div>
        </div>
        <div>
          <div>Visible?</div>
          <div>
            <input type="checkbox" name="isVisible" defaultChecked={category.isVisible} />
          </div>
          <div>
            {error.validations.isVisible && (
              <span className={styles.error}>{error.validations.isVisible}</span>
            )}
          </div>
        </div>
        <div>
          <div>Priority</div>
          <div>
            {["Low", "Medium", "High"].map((p) => {
              const id = `priority_${p}`.toLowerCase();
              return (
                <label key={p} htmlFor={id}>
                  {p}
                  <input
                    type="radio"
                    id={id}
                    name="priority"
                    value={p}
                    defaultChecked={category.priority === p}
                    required
                  />
                </label>
              );
            })}
          </div>
          <div>
            {error.validations.priority && (
              <span className={styles.error}>{error.validations.priority}</span>
            )}
          </div>
        </div>
      </form>
    </div>
  );
}
