// Next.js erkennt - da wir die Daten mit Prisma direkt laden - nicht, dass es dynamische Inhalte sin.
// Bei npm run build würde daraus eine statische Seite generiert werden, wenn wir nicht force-dynamic setzen.
// Siehe https://nextjs.org/docs/app/building-your-application/rendering/server-components#dynamic-rendering
export const dynamic = 'force-dynamic'
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