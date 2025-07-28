import axios from 'axios';
import apiClient from '../../services/ApiClient';
export const getAllQuizz = async (credential) => {
    const response = await apiClient.get('/quizz', {
        params: {
            Keyword: credential.keyword,
            Page: credential.page,
            Size: credential.size,
            CategoryId: credential.genreId,
            Popular: credential.popular,
            Newest: credential.newest
        }
    }); console.log('All quizz response:', response.data);
    return response.data;
};
export const getMyQuizz = async (credential) => {
    const response = await apiClient.get(`/quizz/my-quizzes`, {
        params: {
            Keyword: credential.keyword,
            Page: credential.page,
            Size: credential.size,
            CategoryId: credential.genreId,
            Popular: credential.popular,
            Newest: credential.newest,
            QuizzStatus: credential.status
        }
    }); console.log('All quizz response:', response.data);
    return response.data;
};
export const createQuiz = async (quizForm) => {
    const response = await apiClient.post(`/quizz`, quizForm, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    console.log('Create quizz response:', response.data);
    return response.data;
};

export const getDetailQuizz = async (id) => {
    const response = await apiClient.get(`/quizz/${id}`);
    console.log('Detail quizz response:', response.data);
    return response.data;
};

export const updateQuizAccess = async (quizId, accessDto) => {
    const response = await apiClient.put(`/quizz/${quizId}/access`, accessDto);
    console.log('Upate access respose', response.data);
    return response.data
}

export const updateQuizInfo = async (quizId, info) => {
    const response = await apiClient.patch(`/quizz/${quizId}`, info,
        {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });
    console.log('Upate info respose', response.data);
    return response.data
}

export const createQuestion = async (quizId, questionForm) => {
    const response = await apiClient.post(`/quizz/${quizId}/question`, questionForm, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    console.log('Create question response:', response.data);
    return response.data;
};

export const getQuestions = async (quizId, credential) => {
    const response = await apiClient.get(`/quizz/${quizId}/questions`, {
        params: {
            Keyword: credential.keyword,
            Page: credential.page,
            Size: credential.size,
            Newest: credential.newest
        }
    });
    console.log('Get questions response:', response.data);
    return response.data;
};
export const getQuestion = async (quizId, questionId) => {
    const response = await apiClient.get(`/quizz/${quizId}/question/${questionId}`);
    console.log('Get question response:', response.data);
    return response.data;
};
export const updateQuestion = async (quizId, questionId, questionForm) => {
    const response = await apiClient.put(`/quizz/${quizId}/question/${questionId}`, questionForm, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    console.log('Update question response:', response.data);
    return response.data;
};

export const deleteQuestion = async (quizId, questionId) => {
    const response = await apiClient.delete(`/quizz/${quizId}/question/${questionId}`);
    console.log('Delete question response:', response.data);
    return response.data;
};

export const deleteQuizz = async (quizId) => {
    const response = await apiClient.delete(`/quizz/${quizId}`);
    console.log('Delete quizz response:', response.data);
    return response.data;
};