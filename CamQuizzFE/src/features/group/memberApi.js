import axios from 'axios';
import apiClient from '../../services/ApiClient';
export const getAllMembers = async (groupId, credentials) => {
    const response = await apiClient.get(`/group/${groupId}/members`, {
        params: {
            Page: credentials.page,
            Size: credentials.size,
        }
    });
    console.log('Members response:', response.data);
    return response.data;
};

export const createMember = async (groupId, email) => {
    const response = await apiClient.post(`/group/member`, {
        groupId,
        email
    });
    console.log('Create member response:', response.data);
    return response.data;
}

export const deleteMember = async (groupId, userId) => {
    const response = await apiClient.delete(`/group/${groupId}/member/${userId}`);
    console.log('Delete member response:', response.data);
    return response.data;
}

export const leave = async (groupId) => {
    const response = await apiClient.post(`/group/leave`, {
        groupId
    });
    console.log('Leave member response:', response.data);
    return response.data;
}