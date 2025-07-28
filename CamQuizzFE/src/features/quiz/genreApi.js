import axios from 'axios';
import apiClient from '../../services/ApiClient';
export const getGenres = async () => {
    const response = await apiClient.get(`/genre`,);
    console.log('All genre response:', response.data);
    return response.data;
};

