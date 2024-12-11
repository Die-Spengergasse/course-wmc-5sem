import { auth, signIn } from "../utils/auth";

export default async function AdminPage() {
    const session = await auth()
 
    // Wir verwenden bei der serverseitigen Component die signIn Methode von utils/auth.
    if (!session?.user) await signIn();
    return (
        <div>
            <h1>Admin</h1>
            <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque totam recusandae similique vero quam repellat, provident nemo reprehenderit officia, quod harum ipsum corporis facilis optio, ducimus illo hic sequi culpa.</p>
            <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque totam recusandae similique vero quam repellat, provident nemo reprehenderit officia, quod harum ipsum corporis facilis optio, ducimus illo hic sequi culpa.</p>
            <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque totam recusandae similique vero quam repellat, provident nemo reprehenderit officia, quod harum ipsum corporis facilis optio, ducimus illo hic sequi culpa.</p>
            <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque totam recusandae similique vero quam repellat, provident nemo reprehenderit officia, quod harum ipsum corporis facilis optio, ducimus illo hic sequi culpa.</p>
            <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque totam recusandae similique vero quam repellat, provident nemo reprehenderit officia, quod harum ipsum corporis facilis optio, ducimus illo hic sequi culpa.</p>
        </div>
    );
}    
