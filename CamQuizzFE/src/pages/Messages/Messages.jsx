import React, { useMemo, useState,useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import Conversation from '../../components/Messages/Conversation'
import Conversations from '../../components/Messages/Conversations'
import Login from '../Auth/Login'
const Messages = () => {
    const navigate = useNavigate();
    const { id = null } = useParams();
    const [selectedGroupId, setSelectedGroupId] = useState(id);
    useEffect(() => {
        setSelectedGroupId(id);
    }, [id]);
    const currentUserId = useMemo(() => {
        try {
            const user = JSON.parse(localStorage.getItem('user'));
            if (user == null)
                navigate('/login')
            return user.id;
        } catch {
            return null;
        }
    }, []);

    return (
        <div className='flex h-[550px] justify-stretch  '>
            <aside className={`${id != null ? 'hidden' : 'w-full'}  sm:block sm:w-1/4 h-full overflow-y-auto `}>
                <Conversations
                    selectGroup={id}
                    handleSelect={(id) => {
                        console.log("click", id)
                        navigate(`/messages/${id}`)
                        setSelectedGroupId(id)
                    }} />
            </aside>
            <main className={`${id == null ? 'hidden' : ''}   w-full sm:w-3/4 flex h-full overflow-y-auto `}>
                <Conversation groupId={selectedGroupId} userId={currentUserId} />
            </main>
        </div>
    )
}

export default Messages