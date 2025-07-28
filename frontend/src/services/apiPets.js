import axios from 'axios'
import { getAccessToken, refreshToken } from './apiUser'


export const getAllPets = async (searchParams) => {
    try {
        const response = await axios.get(`http://91.197.97.151/Pets`,{
            params: searchParams
        })
        return response.data
    }
    catch(e){
        throw e
    }
}
export const addImage = async(imageFile) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
    const formData = new FormData();
    formData.append('file', imageFile);
    try {
        const token = getAccessToken();
        const response = await axios.post('http://91.197.97.151/Image', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'Authorization': `Bearer ${token}`
            },
        });
        return response.data; 
    } catch (e) {
        
        throw e
    }
    }
}
export const addPet = async (ResponsePets) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
    const token = getAccessToken(); 
    try {
        const response = await axios.post(`http://91.197.97.151/Pets`, ResponsePets, {
            headers: {
                'Authorization': `Bearer ${token}`
            }})
        return response.data;
    }
    catch(e) {
        throw e
    }
}
}
export const deletePet = async (petId) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
    const token = getAccessToken();
    try {
      const response = await axios.delete(`http://91.197.97.151/Pets/${petId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      return response.data; 
    } 
    catch (error) {
      throw error;
    }
}
return false;
}
export const deleteImage = async (imageKey) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
      const token = getAccessToken();
      try {
          const response = await axios.delete(`http://91.197.97.151/Image?key=${imageKey}`, { 
              headers: {
                  'Authorization': `Bearer ${token}`,
                  'Content-Type': 'multipart/form-data',
              },
          });
          return response.data; 
      } 
      catch (error) {
          throw error;
      }
    }
}
export const updateImage = async (file, key) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
    const token = getAccessToken();
    const formData = new FormData();
    formData.append('file', file);
    try {
        const response = await axios.put(`http://91.197.97.151/Image?key=${key}`, formData, { 
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'multipart/form-data',
            }, 
        });
        return response.data; 
    } catch (error) {
        throw error;
    }
}
}
export const updatePet = async (id, petData) => {
    const isRefreshed = await refreshToken();
    if (isRefreshed) {
    const token = getAccessToken();
    try {
        const response = await axios.put(`http://91.197.97.151/Pets/${id}`, petData, {  
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',  
            },
        });
        return response.data; 
    } catch (error) {
        throw error;
    }
}
}
