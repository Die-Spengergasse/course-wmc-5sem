import type { Metadata } from "next";
import "./globals.css";
import { ReactNode } from "react";
import Navbar from "./components/navbar/Navbar";
import { SessionProvider } from "next-auth/react";

export const metadata: Metadata = {
  title: "Create Next App",
  description: "Generated by create next app",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: ReactNode;
}>) {
  return (
    <SessionProvider>
      <html lang="de">
      <body>
        <Navbar />
        <main>
          <div className="container">{children}</div>
        </main>
      </body>
      </html>
    </SessionProvider>
  );
}