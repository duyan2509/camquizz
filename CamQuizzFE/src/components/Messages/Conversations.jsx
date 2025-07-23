import React, { useEffect, useState } from 'react'
import { Button } from 'antd'
import PreviewMessage from './PreviewMessage';
import { useConversations } from '../../hooks/group';
import { useNotiReceived } from '../../hooks/useChat'
const Conversations = ({ handleSelect, selectGroup }) => {
    const [credentials, setCredentials] = useState({ page: 1, size: 10 })
    const { data, total, loading, error,fetch } = useConversations(credentials);
    const { receivedNoti, setReceivedNoti } = useNotiReceived()
    const handleLoadMore = () => {
        setCredentials(prev => ({
            ...prev,
            page: prev.page + 1
        }))
    }
    useEffect(() => {
        if (receivedNoti) {
            fetch()
            setReceivedNoti(false)
        }
    }, [receivedNoti])
    return (
        <div>
            {
                data.map((conversation, index) => (
                    <PreviewMessage
                        key={index}
                        conversation={conversation}
                        onSelect={() => handleSelect(conversation.groupId)}
                        selectGroup={selectGroup}
                    />
                ))
            }
            {total !== data.length && <Button type="link" block onClick={handleLoadMore}>
                See more
            </Button>}
        </div>
    )
}

export default Conversations