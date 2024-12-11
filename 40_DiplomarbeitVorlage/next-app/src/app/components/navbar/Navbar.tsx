"use client"
import Link from "next/link";
import styles from "./Navbar.module.css";
import { signIn, useSession } from 'next-auth/react';
import { usePathname } from 'next/navigation';
import UserInfo from "./Userinfo";

export default function NavbarComponent() {
    const { data: session } = useSession();
    const pathname = usePathname(); // Aktuellen Pfad abrufen

    return (
        <nav className={styles.navbar}>
            <div className="container">
                <div className={styles.menu}>
                    <ul>
                        <li>
                            <Link href="/" className={pathname === '/' ? styles.active : ''}>Home</Link>
                        </li>
                        <li>
                            <Link href="/persons" className={pathname === '/persons' ? styles.active : ''}>Personen</Link>
                        </li>
                        <li>
                            <Link href="/persons/add" className={pathname === '/persons/add' ? styles.active : ''}>Neue Person</Link>
                        </li>                        
                        <li>
                            <Link href="/admin" className={pathname === '/admin' ? styles.active : ''}>Admin</Link>
                        </li>
                    </ul>
                </div>
                <div className={styles.userinfo}>
                    {session && session.user && <UserInfo user={session.user} />}
                    {!session && (
                        <div className={styles.notSignedIn}>
                            <div>Nicht angemeldet</div>
                            <div>
                                <a
                                    href={`/api/auth/signin`}
                                    className={styles.buttonPrimary}
                                    onClick={(e) => {
                                        e.preventDefault();
                                        signIn();
                                    }}>Anmelden</a>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
}

