import axios from "axios";
import { getAccessToken } from './apiUser'

export const addFav = async (id) => {
    const token = getAccessToken()
    if (!token) {
        console.error('Токен отсутствует. Пользователь не авторизован.');
        alert('Сначала войдите в приложение!');
        return null;
    }
    try {
        const response = await axios.post(`http://91.197.97.151/Favorites`,id,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
};
export const getUserFav = async () => {
    const token = getAccessToken();
    if (!token) {
        console.error('Токен отсутствует. Пользователь не авторизован.');
        return null;
    }
    try {
        const response = await axios.get(`http://91.197.97.151/Favorites`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        console.error('Ошибка при получении избранного:', error);
        throw error;
    }
};
export const deleteFav = async (id) => {
    const token = getAccessToken();
    if (!token) {
        console.error('Токен отсутствует. Пользователь не авторизован.');
        return null;
    }
    try {
        const response = await axios.delete(`http://91.197.97.151/Favorites/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );
        return response.data; 
    } catch (error) {
        console.error('Ошибка при удалении из избранного:', error);
        throw error;
    }
};