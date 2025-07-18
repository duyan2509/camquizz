import { useEffect, useState } from 'react';
import { getAllMembers, deleteMember, leave, createMember } from '../features/group/memberApi';

export const useMembers = (groupId, credentials) => {
  const [data, setData] = useState([]);
  const [page, setPage] = useState(credentials.page || 1);
  const [size, setSize] = useState(credentials.size || 10);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchData = async (pageToFetch = page) => {
    setLoading(true);
    try {
      const response = await getAllMembers(groupId, { page: pageToFetch, size });
      if (pageToFetch === 1) {
        setData(response.data);
      } else {
        setData(prev => [...prev, ...response.data]);
      }
      setPage(pageToFetch);
      setTotal(response.total);
    } catch (err) {
      setError(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData(1); 
  }, [groupId, size]);

  const loadMore = () => {
    if (data.length < total && !loading) {
      fetchData(page + 1);
    }
  };
  const refresh =()=>{
    fetchData(1);
  }
  return { data, total, loading, error, loadMore, refresh};
};

export const useAddMember = () =>{
  const [loading, setLoading] = useState(false);
  const [error, setError]= useState(null);
  const [success,setSuccess] =useState(false);
  const addMember = async (groupId, email)=>{
    try{
      setLoading(true);
      setError(null);
      setSuccess(false);
      const response = await createMember(groupId,email);
      setLoading(false);
      setSuccess(true);
      return response
    }
    catch (err) {
      setLoading(false);
      setError(err.response.data.message); 
    }
  }
  return {loading,error, success, addMember}
}
export const useLeave = () =>{
  const [loading, setLoading] = useState(false);
  const [error, setError]= useState(null);
  const [success,setSuccess] =useState(false);
  const onLeave = async (groupId)=>{
    try{
      setLoading(true);
      setError(null);
      setSuccess(false);
      const response = await leave(groupId);
      setLoading(false);
      setSuccess(true);
      return response
    }
    catch (err) {
      setLoading(false);
      setError(err.response.data.message); 
    }
  }
  return {loading,error, success, onLeave}
}

export const useKick = () =>{
  const [loading, setLoading] = useState(false);
  const [error, setError]= useState(null);
  const [success,setSuccess] =useState(false);
  const kick = async (groupId , memberId)=>{
    try{
      setLoading(true);
      setError(null);
      setSuccess(false);
      const response = await deleteMember(groupId,memberId);
      setLoading(false);
      setSuccess(true);
      return response
    }
    catch (err) {
      setLoading(false);
      setError(err.response.data.message); 
    }
  }
  return {loading,error, success, kick}
}