import { getPersons } from "./actions";
import styles from './page.module.css';

export default async function PersonsPage() {
    const persons = await getPersons();

    return (
        <div className={styles.persons}>
            <h1>Personen</h1>
            <ul>
                {persons.map(p => (
                    <li key={p.guid}>
                        {p.firstname} {p.lastname}
                        {p.birthDate && (<small> (geb. am {p.birthDate.toISOString().split("T")[0]})</small>)}
                    </li>
                ))}
            </ul>
        </div>
    );
}