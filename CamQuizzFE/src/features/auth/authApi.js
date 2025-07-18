import axios from 'axios';

const API_URL = 'https://localhost:5001/api/auth';

export const loginUser = async (credentials) => {
    console.log('Login credentials:', credentials);
    const response = await axios.post(`${API_URL}/login`, credentials);
    console.log('Login response:', response.data);
    return response.data; 
};

export const registerUser = async (data) => {
    const response = await axios.post(`${API_URL}/register`, data);
    console.log('Register response:', response.data);
    return response.data;
};
