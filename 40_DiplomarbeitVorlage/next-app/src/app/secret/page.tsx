import { auth } from "@/app/utils/auth"
import { redirect } from "next/navigation";

export default async function SecretPage() {
    const session = await auth();

    if (!session || !session.user) {
      // Redirect to login if the user is not authenticated
      redirect("/api/auth/signin");
    }

    return (
        <div>
            <h1>Secret page</h1>
            <p>Only available after authentication.</p>
        </div>
    );
}