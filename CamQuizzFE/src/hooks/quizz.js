import { use, useEffect, useState } from 'react';
import { getGroupQuizzes, updateVisible, removeFromGroup } from '../features/group/quizzApi'
import { 
    getAllQuizz, 
    getMyQuizz, 
    createQuiz, 
    getDetailQuizz, 
    updateQuizAccess, 
    updateQuizInfo,
    deleteQuizz,
    getQuestions,
    updateQuestion,
    deleteQuestion,
    getQuestion,
    createQuestion
    } from '../features/quiz/quizApi';
export const useGroupQuizzes = (groupId) => {
    const [data, setData] = useState([]);
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    const [credentials, setCredentials] = useState({
        page: 1,
        size: 5,
        keyword: "",
    })
    const [total, setTotal] = useState(0);
    const fetch = async () => {
        try {
            setLoading(true);
            setErr(null);
            const response = await getGroupQuizzes(groupId, credentials);
            setData(response.data)
            setTotal(response.total);
        }
        catch (err) {
            setErr(err.response.data.message)
        }
        finally {
            setLoading(false);
        }
    }
    useEffect(() => {
        fetch()
    }, [credentials.page, credentials.size, credentials.keyword])
    return { data, err, loading, credentials, setCredentials, fetch, total }
}

export const useVisible = (groupId, quizId) => {
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);
    const update = async (visible) => {
        try {
            setLoading(true)
            setErr(null)
            setSuccess(false);
            const result = await updateVisible(groupId, quizId, visible)
            setSuccess(true);
            return result
        }
        catch (err) {
            setErr(err?.response?.data?.message)
        }
        finally {
            setLoading(false)
        }
    }
    return { err, loading, success, update }
}
export const useRemove = (groupId, quizId) => {
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState(false);
    const remove = async () => {
        try {
            setLoading(true)
            setErr(null)
            setMessage(null)
            const result = await removeFromGroup(groupId, quizId)
            setMessage(result.message)
        }
        catch (err) {
            setErr(err?.response?.data?.message)
        }
        finally {
            setLoading(false)
        }
    }
    return { err, loading, remove, message }
}

export const useAllQuizzes = (isHome) => {
    const [quizzes, setQuizzes] = useState([])
    const [errQuiz, setErrQuiz] = useState(null)
    const [credential, setCredentials] = useState({
        page: 1,
        size: 10,
        genreId: 'All',
        popular: true,
        newest: true,
        status: 'All'
    })
    const [total, setTotal] = useState(0);
    const reset = () => {
        setCredentials({
            keyword: "",
            page: 1,
            size: 10,
            genreId: 'All',
            popular: true,
            newest: true,
            status: null
        })
    }
    const homeFetch = async () => {
        try {
            setErrQuiz(null)
            const result = await getAllQuizz({
                ...credential,
                genreId: credential.genreId === 'All' ? null : credential.genreId
            });
            setQuizzes(result.data)
            setTotal(result.total)
        }
        catch (err) {
            setErrQuiz(err?.response?.data?.message)
        }
    }
    const myQuizzFetch = async () => {
        try {
            setErrQuiz(null)
            const result = await getMyQuizz({
                ...credential,
                genreId: credential.genreId === 'All' ? null : credential.genreId,
                status: credential.status === 'All' ? null : credential.status
            });
            setQuizzes(result.data)
            setTotal(result.total)
        }
        catch (err) {
            console.log(err)
            setErrQuiz(err?.response?.data?.message)
        }
    }
    useEffect(() => {
        const fetch = async () => {
            if (isHome)
                await homeFetch();
            else
                await myQuizzFetch();
        };
        fetch();
    }, [
        credential.page,
        credential.genreId,
        credential.newest,
        credential.size,
        credential.popular,
        credential.status,
        credential.keyword
    ]);

    return { errQuiz, quizzes, credential, setCredentials, total, reset }
}

export const useCreateQuizz = () => {
    const [quiz, setQuiz] = useState(null)
    const [successCreate, setSuccessCreate] = useState(false);
    const [errCreate, setErrCreate] = useState(null);
    const createQuizz = async (form) => {
        try {
            setSuccessCreate(false)
            const result = await createQuiz(form)
            setQuiz(result)
            setSuccessCreate(true)
        }
        catch (err) {
            console.log(err)
            setErrCreate(err?.response?.data?.message)
        }
    }
    return { quiz, successCreate, errCreate, createQuizz }
}

export const useDetailQuiz = (id) => {
    const [quiz, setQuiz] = useState(null)
    const [successDetalQuizz, setSuccess] = useState(false);
    const [errDetailQuizz, setErr] = useState(null);
    const getDetailQuiz = async (id) => {
        try {
            setSuccess(false)
            const result = await getDetailQuizz(id)
            console.log(result.name)
            setQuiz(result)
            setSuccess(true)
        }
        catch (err) {
            console.log(err)
            setErr(err?.response?.data?.message)
        }
    }
    useEffect(() => {
        if (id) {
            getDetailQuiz(id);
        }
    }, [id]);
    return { quiz, successDetalQuizz, errDetailQuizz, setQuiz }
}

export const useUpdateAcess = (quizId) => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null)
    const [access, setAccess] = useState(null);
    const updateAccess = async (accessDto) => {
        try {
            const result = await updateQuizAccess(quizId, accessDto);
            setAccess(result)
        }
        catch (err) {
            setErr(err?.response?.data?.message)
        }
        finally {
            setSuccess(true);
        }
    }
    return { successAccess: success, errAccess:err, access, updateAccess }
}

export const useUpdateInfo = (quizId) => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null)
    const [info, setInfo] = useState(null);
    const updateInfo = async (info) => {
        try {
            const result = await updateQuizInfo(quizId, info);
            setInfo(result)
        }
        catch (err) {
            setErr(err?.response?.data?.message)
        }
        finally {
            setSuccess(true);
        }
    }
    return { successInfo: success, errInfo:err, info, updateInfo }
}

export const useQuestions = (quizId) => {
    const [questions, setQuestions] = useState([]);
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    const [credentials, setCredentials] = useState({
        page: 1,
        size: 10,
        keyword: "",
        newest: true,
    });
    const [total, setTotal] = useState(0);
    const fetch = async () => {
        try {
            setLoading(true);
            setErr(null);
            const response = await getQuestions(quizId, credentials);
            setQuestions(response.data);
            setTotal(response.total);
        }
        catch (err) {
            console.log(err?.response?.data?.message)
            setErr(err?.response?.data?.message)
        }
        finally {
            setLoading(false);
        }
    }
    useEffect(() => {
        fetch()
    }, [quizId, credentials.page, credentials.size, credentials.keyword, credentials.newest]);
    return { questions, errQuestions: err, loading, total, credentials, setCredentials, fetch};
}
export const useCreateQuestion = (quizId) => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null);
    const [question, setQuestion] = useState(null);
    const addQuestion = async (questionForm) => {
        try {
            setSuccess(false);
            const result = await createQuestion(quizId, questionForm);
            setQuestion(result);
            setSuccess(true);
        }
        catch (err) {
            console.log(err)
            setErr(err?.response?.data?.message||err?.response?.data?.title)
        }
    }
    return { successCreate: success, errCreate: err, question, addQuestion };
}
export const useUpdateQuestion = (quizId) => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null);
    const [question, setQuestion] = useState(null);
    const update = async (questionId, questionForm) => {
        try {
            setSuccess(false);
            setErr(null);
            const result = await updateQuestion(quizId, questionId, questionForm);
            setQuestion(result);
            setSuccess(true);
        }
        catch (err) {
            console.log(err)
            setErr(err?.response?.data?.message||err?.response?.data?.title)
        }
    }
    return { successUpdate: success, errUpdate: err, question, update };
}
export const useGetQuestion = (quizId) => {
    const [question, setQuestion] = useState(null);
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    
    const get = async (questionId) => {
        try {
            setLoading(true);
            setErr(null);
            const response = await getQuestion(quizId, questionId);
            setQuestion(response);
        }
        catch (err) {
            console.log(err?.response?.data?.message)
            setErr(err?.response?.data?.message || err?.response?.data?.title);
        }
        finally {
            setLoading(false);
        }
    }
    return { question, errQuestion: err, loading, get };
}
export const useDeleteQuestion = (quizId) => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    
    const remove = async (questionId) => {
        try {
            setLoading(true);
            setErr(null);
            setSuccess(false);
            await deleteQuestion(quizId, questionId);
            setSuccess(true);
        }
        catch (err) {
            console.log(err?.response?.data?.message)
            setErr(err?.response?.data?.message || err?.response?.data?.title);
        }
        finally {
            setLoading(false);
        }
    }
    return { successDelete: success, errDelete: err, loading, remove };
}

export const useDeleteQuizz = () => {
    const [success, setSuccess] = useState(false);
    const [err, setErr] = useState(null);
    const [loading, setLoading] = useState(false);
    
    const remove = async (quizId) => {
        try {
            setLoading(true);
            setErr(null);
            setSuccess(false);
            await deleteQuizz(quizId);
            setSuccess(true);
        }
        catch (err) {
            console.log(err?.response?.data?.message)
            setErr(err?.response?.data?.message || err?.response?.data?.title);
        }
        finally {
            setLoading(false);
        }
    }
    return { successDelete: success, errDelete: err, loading, remove };
}