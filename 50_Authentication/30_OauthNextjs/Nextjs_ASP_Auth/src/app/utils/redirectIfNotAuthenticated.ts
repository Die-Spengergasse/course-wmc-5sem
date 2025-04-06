'use server';

import { redirect } from 'next/navigation';
import { getAxiosInstance } from './getAxiosInstance-server';

export async function redirectIfNotAuthenticated(redirectUri: string) {
    const axiosInstance = await getAxiosInstance();
    try {
        await axiosInstance.get('/oauth/me'); // Cookies werden mitgeschickt
    }
    catch (error: any) {
        if (error.response?.status === 401)
            redirect(`https://localhost:5443/oauth/login?redirectUri=${redirectUri}`);
    }
}
