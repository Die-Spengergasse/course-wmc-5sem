export interface TodoItem {
    guid: string;
    title: string;
    description: string;
    categoryGuid: string;
    categoryName: string;
    categoryPriority: string;
    categoryIsVisible: boolean;
    isCompleted: boolean;
    dueDate: string;
    createdAt: string;
    updatedAt: string;
  }
  
  export function isTodoItem(item: any): item is TodoItem {
    return (
      typeof item === "object" &&
      "guid" in item &&
      "title" in item &&
      "description" in item &&
      "categoryGuid" in item &&
      "categoryName" in item &&
      "categoryPriority" in item &&
      "categoryIsVisible" in item &&
      "isCompleted" in item &&
      "dueDate" in item &&
      "createdAt" in item &&
      "updatedAt" in item
    );
  }
  