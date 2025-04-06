import axios from 'axios';

export function getAxiosInstance() {
    return axios.create({
        baseURL: 'https://localhost:5443',
        withCredentials: true, // notwendig für Cookies
        headers: {
            'Content-Type': 'application/json',
        }
    });
}
