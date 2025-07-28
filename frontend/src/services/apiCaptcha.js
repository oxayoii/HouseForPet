import axios from 'axios'
export const getCaptcha = async () => {
  try {
    const response = await axios.get(`http://91.197.97.151/User/new-captcha`);
    return response.data; 
  } catch (error) {
    console.error('Ошибка при получении капчи:', error);
    throw error;
  }
};