import axios from "axios";

const ACCESS_TOKEN_KEY = 'access_token'; 
const REFRESH_TOKEN_KEY = 'refresh_token';

export const regUser = async (responseUser) => {
    try {
        const response = await axios.post(`http://91.197.97.151/User`, responseUser)
        return response.data
    }
    catch(e){
        throw e; 
    }
}
export const loginUser = async (responseUser) => {
    try{
        const response = await axios.post(`http://91.197.97.151/User/Auth`, responseUser)
        const { accessToken, refreshToken,  } = response.data;
        localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);

        return { accessToken, refreshToken};
    }
    catch(e){
        throw e
    }
}
export const refreshToken = async () => {
    const refreshToken = getRefreshToken();
    if (!refreshToken) {
        console.warn('Refresh token не найден. Требуется вход.');
        return false;
    }
    try {
        const accessToken = getAccessToken()
        const response = await axios.post('http://91.197.97.151/User/Refresh', null, {
            params: { RefreshToken: refreshToken, AccessToken : accessToken }
        });
        if (response.status === 200) {
            const { accessToken: newAccessToken } = response.data; 
            localStorage.setItem(ACCESS_TOKEN_KEY, newAccessToken);
            console.log('Access token успешно обновлен.');
            return true;
        } 
    } catch (error) {
        if (error.response) {
            if (error.response.status === 401) {
                localStorage.removeItem(ACCESS_TOKEN_KEY); 
                localStorage.removeItem(REFRESH_TOKEN_KEY); 
                window.location.href = '/avt'; 
                return false;
            }
        } else {
            console.error('Ошибка сети или ошибка сервера при обновлении.');
        }
        return false;
    }
}
export const logoutUser = async() =>{
    try {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
    }
    catch (e) {
        console.error("Error during logout:", e); 
        throw e;
    }
}
export const getRefreshToken = () => {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
}
export const getAccessToken = () => {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
}
export const getUserPermissions = async () => {
    try {
        const accessToken = getAccessToken();
        if (!accessToken) {
            throw new Error("Access token not found");
        }
        const response = await axios.get(`http://91.197.97.151/User`, { 
            headers: {
                Authorization: `Bearer ${accessToken}` 
            },
            params: { 
                token: accessToken
            }
        });

        return new Set(response.data);
    }
    catch(e) {
        throw e
    }
}