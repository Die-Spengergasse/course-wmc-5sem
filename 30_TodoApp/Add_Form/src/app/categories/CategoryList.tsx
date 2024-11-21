import { Category } from "../types/Category";
import styles from "./CategoryList.module.css";

export default function CategoryList({categories}: {categories: Category[]}) {
    return (
        <div className={styles.categories}>
            <ul>
                {categories.map(item => (
                    <li
                        key={item.guid}
                    >
                        <h2>{item.name}</h2>
                        <p>{item.description}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
}
