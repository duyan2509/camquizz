import { useEffect, useState } from 'react';
import { getAllGroups, createGroup, updateGroup, deleteGroup, getGroup, getConversations } from '../features/group/groupApi';
import { runSudo } from '@react-native-community/cli-doctor/build/tools/installPods';

export const useGroups = (credentials) => {
  const [data, setData] = useState([]);
  const [page, setPage] = useState(1);
  const [size, setSize] = useState(10);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!credentials) return;
    setLoading(true);
    getAllGroups(credentials)
      .then(response => {
        setData(response.data);
        setPage(response.page);
        setSize(response.size);
        setTotal(response.total);
      })
      .catch(setError)
      .finally(() => setLoading(false));
  }, [credentials]);

  return { data, setData, total, loading, error };
};

export const useUpdateGroup = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);
  const handleUpdateGroup = async (groupId, newName) => {
    setLoading(true);
    setError(null);
    setSuccess(false);

    try {
      const result = await updateGroup(groupId, newName);
      setSuccess(true);
      return result;
    } catch (err) {
      setError(err.response.data.message || err.response.data.title);
    } finally {
      setLoading(false);
    }
  };

  return { handleUpdateGroup, loading, error, success };
};

export const useCreateGroup = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);
  const [newGroup, setNewGroup] = useState(null);
  const handleCreateGroup = async (name) => {
    setLoading(true);
    setError(null);
    setSuccess(false);

    try {
      const result = await createGroup(name);
      setSuccess(true);
      setNewGroup(result)
      return result;
    } catch (err) {
      setError(err.response.data.message || err.response.data.title);
    } finally {
      setLoading(false);
    }
  };

  return { handleCreateGroup, loading, error, success, newGroup };
}

export const useDeleteGroup = () => {
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);
  const handleDeleteGroup = async (id) => {
    setError(null);
    setSuccess(false);
    try {
      const result = await deleteGroup(id);
      setSuccess(true);
      return result.success;
    }
    catch (err) {
      console.log("err dlt", err.response.data.message)
      setError(err.response.data.message || err.response.data.title);
    }
  }
  return { error, success, handleDeleteGroup }
}

export const useGroup = (id) => {
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [group, setGroup] = useState(null);
  useEffect(() => {
    if (!id) return;
    setLoading(true);
    getGroup(id)
      .then(response => {
        setGroup(response);
        setSuccess(true)
      })
      .catch(setError)
      .finally(() => setLoading(false));
  }, [id]);
  return { error, success, group, loading }
}

export const useConversations = (credentials) => {
  const [data, setData] = useState([]);
  const [page, setPage] = useState(1);
  const [size, setSize] = useState(10);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetch = async () => {
    try {
      setLoading(true)
      setError(null)
      const result = await getConversations(credentials)
      setData(prev =>
        credentials.page === 1
          ? result.data 
          : [...prev, ...result.data]
      );
      setTotal(result.total)
      setSize(result.size)
      setPage(result.page)
    } catch (err) {
      setError(err.response.data.message)
    }
    finally {
      setLoading(false)
    }
  }
  useEffect(() => {
    fetch();
  }, [credentials.page, credentials.size])
  return { data, total, loading, error,fetch };
};