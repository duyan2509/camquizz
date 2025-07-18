import { use, useEffect, useState } from 'react';
import { getGroupQuizzes, updateVisible, removeFromGroup} from '../features/group/quizzApi'

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
    return { err, loading, remove, message}
}