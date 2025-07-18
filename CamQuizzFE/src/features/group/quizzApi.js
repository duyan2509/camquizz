import axios from 'axios';
import apiClient from '../../services/ApiClient';
export const getGroupQuizzes = async (groupId, credentials) => {
    const response = await apiClient.get(`/group/${groupId}/quizz`, {
        params: {
            Page: credentials.page,
            Size: credentials.size,
            Keyword: credentials.keyword
        }
    });
    console.log('Group quizzes response:', response.data);
    return response.data;
};

export const updateVisible = async (groupId, quizId, visible) => {
    const response = await apiClient.put(`/group/${groupId}/quizz/${quizId}`, {
        params: {
            visible
        }
    });
    console.log('Update visible quizzes response:', response.data);
    return response.data;
};

export const removeFromGroup = async (groupId, quizId) => {
    const response = await apiClient.delete(`/group/${groupId}/quizz/${quizId}`);
    console.log('Remove from group response:', response.data);
    return response.data;
};