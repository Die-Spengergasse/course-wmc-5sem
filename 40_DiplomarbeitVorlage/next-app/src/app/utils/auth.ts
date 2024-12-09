import NextAuth from "next-auth";
import getServerSession, { NextAuthConfig, Session } from "next-auth";
import MicrosoftEntraId from 'next-auth/providers/microsoft-entra-id';

const config: NextAuthConfig = {
  providers: [
    MicrosoftEntraId({
      clientId: process.env.AZURE_AD_CLIENT_ID,
      clientSecret: process.env.AZURE_AD_CLIENT_SECRET,
      issuer: `https://login.microsoftonline.com/${process.env.AZURE_AD_TENANT_ID}/v2.0`,
      authorization: {
        params: {
          scope: "openid profile email User.Read",
          prompt: 'select_account'
        },
      }
    })
  ],
  callbacks: {
    async jwt({ token, account }) {
      if (account?.access_token) {
        // Rufe zusätzliche Benutzerinformationen von Microsoft Graph API ab
        const graphResponse = await fetch("https://graph.microsoft.com/v1.0/me", {
          headers: {
            Authorization: `Bearer ${account.access_token}`,
          },
        });

        const data = await graphResponse.json();
        token.mobilePhone = data.mobilePhone;
      }
      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.user.mobilePhone = String(token.mobilePhone);
      }
      return session;
    }
  }
}

export const { handlers, auth, signIn, signOut } = NextAuth(config);

declare module "next-auth" {
  interface User {
      // Add your additional properties here:
      mobilePhone: string;
  }
  interface Session {
      // Add your additional properties here:
      mobilePhone: string;
  }    
}
