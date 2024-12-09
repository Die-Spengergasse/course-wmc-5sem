import { User } from "next-auth";
import Image from 'next/image';

import styles from "./Userinfo.module.css";
import { signOut } from "next-auth/react";

export default function UserinfoComponent({ user }: { user: User }) {
    return (
        <div className={styles.userinfo}>
            {user.image && <div className={styles.avatar}>
                <Image alt="avatar" src={user.image} width={50} height={50}
                    style={{
                        borderRadius: '50%',
                        overflow: 'hidden',
                        display: 'block',
                    }}
                /></div>}
            <div className={styles.info}>
                <div className={styles.name}>{user.name}</div>
                <div className={styles.email}>{user.email}</div>
                <div>
                    <a className={styles.signOutLink}
                        href={`/api/auth/signout`}
                        onClick={(e) => {
                            e.preventDefault();
                            signOut();
                        }}>Abmelden</a>
                </div>
            </div>
        </div>
    )
}