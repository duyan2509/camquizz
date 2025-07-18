import axios from 'axios';
import apiClient from '../../services/ApiClient';
export const getAllGroups = async (credentials) => {
    const response = await apiClient.get(`/group/my-groups`, {
        params: {
            keyword: credentials.keyword || '',
            Page: credentials.page,
            Size: credentials.size,
            isOwner: credentials.isOwner,
        }
    });
    console.log('Groups response:', response.data);
    return response.data;
};
export const updateGroup = async (groupId, newName) => {
    const response = await apiClient.put(`/group/${groupId}`, {
        name: newName
    });
    console.log('Update group response:', response.data);
    return response.data;
}
export const createGroup = async (name) => {
    const response = await apiClient.post(`/group`, {
        name
    });
    console.log('Create group response:', response.data);
    return response.data;
}

export const deleteGroup = async (id) => {
    const response = await apiClient.delete(`/group/${id}`);
    console.log('Delete group response:', response.data);
    return response.data;
}

export const getGroup = async (id) => {
    const response = await apiClient.get(`/group/${id}`);
    console.log('Group response:', response.data);
    return response.data;
}

export const getConversations = async (credentials) => {
    const response = await apiClient.get(`/group/conversations`,{
        params: {
            Page: credentials.page,
            Size: credentials.size,
        }
    });
    console.log('Covnersations response:', response.data);
    return response.data;
}