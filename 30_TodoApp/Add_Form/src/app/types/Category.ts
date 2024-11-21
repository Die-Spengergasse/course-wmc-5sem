export interface Category {
    guid: string;
    name: string;
    description: string;
    isVisible: boolean;
    priority: string;
    ownerName: string;
  }
  
  export function isCategory(item: any): item is Category {
    return (
      typeof item === "object" &&
      "guid" in item &&
      "name" in item &&
      "description" in item &&
      "isVisible" in item &&
      "priority" in item &&
      "ownerName" in item
    );
  }
  