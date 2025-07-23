import { useState, useEffect, useMemo } from 'react'
import { useSignalR } from '../context/SignalRContext'

export const useChat = () => {
    const connection = useSignalR();
    const [messages, setMessages] = useState([])
    const [totalMsg, setTotalMsg] = useState(0);
    const [page, setPage] = useState(1)
    const [groupName, setGroupName] = useState(null)
    const [err, setErr] = useState(null)
    const [errSend, setErrSend] = useState(null)
    const [currentGroupId, setCurrentGroupId] = useState(null);
    const [isReady, setIsReady] = useState(false)

    const send = (requestDto) => {
        try {
            if (!connection) {
                console.warn("Connection chưa sẵn sàng");
                return;
            }
            setErr(null);
            connection.invoke('SendMessage', requestDto)
        }
        catch (err) {
            console.log("err send group", err)
        }
    }
    const handleErrSendMessage = (response) => {
        console.log('ErrorSendMessage', response)
        setErrSend(response)
    }
    const join = (joinGroupDto) => {
        try {
            if (!connection) {
                console.warn("Connection chưa sẵn sàng");
                return;
            }
            if (currentGroupId != null)
                connection.invoke('LeaveGroup', currentGroupId)
            setGroupName(null)
            setErr(null);
            setMessages([]);
            setTotalMsg(0)
            connection.invoke('JoinGroup', joinGroupDto)
        }
        catch (err) {
            console.log("err join group", err)
        }
    }
    const handleReceiveFirst = (response) => {
        setMessages(response.data)
        setTotalMsg(response.total)
        setGroupName(response.groupName)
        setCurrentGroupId(response.groupId)
        if (response.data.length > 0)
            markLastRead(response.data[response.data.length - 1].id)
    }
    const handleReceiveOldMessage = (response) => {
        console.log('old', response)
        setMessages(prev => [...response.data, ...prev])
        setTotalMsg(response.total)
        setPage(response.page)
    }
    const handleReceiveNewMessage = (response) => {
        console.log('handleReceiveNewMessage', response)
        setMessages(prev => [...prev, response])

    }
    const handleSendSuccess = (response) => {
        console.log('handleSendSuccess', response)
        setMessages(prev => [...prev, response])

    }
    const more = () => {
        try {
            if (!connection) {
                console.warn("Connection chưa sẵn sàng");
                return;
            }
            if (totalMsg > messages.length && messages.length > 0) {
                console.log('ok')
                connection.invoke('LoadMoreMessages', {
                    size: 10,
                    afterId: messages[0].id,
                    afterCreatedAt: messages[0].time
                })
            }
        }
        catch (err) {
            console.log("err load old msg", err)
        }
    }
    const leave = (groupId) => {
        try {
            if (!connection) {
                console.warn("Connection chưa sẵn sàng");
                return;
            }
            connection.invoke('LeaveGroup', groupId)
        }
        catch (err) {
            console.log("err leave group", err)
        }
    }

    const markLastRead = (msgId) => {
        try {
            if (!connection) {
                console.warn("Connection chưa sẵn sàng");
                return;
            }
            connection.invoke('MarkLastRead', msgId)
        }
        catch (err) {
            console.log("err MarkLastRead", err)
        }
    }
    const handleErr = res => {
        setErr(res)
    }

    useEffect(() => {
        if (!connection) return;
        setIsReady(true)

        connection.on('ReceiveFirstMessages', handleReceiveFirst);
        connection.on('JoinGroupError', handleErr)
        connection.on('ReceiveNewMessage', handleReceiveNewMessage)
        connection.on('ReceiveOldMessage', handleReceiveOldMessage)
        connection.on('ErrorSendMessage', handleErrSendMessage)
        connection.on('SendSuccess', handleSendSuccess)
        connection.on('Error', handleErr)


        return () => {
            connection.off('ReceiveFirstMessages', handleReceiveFirst);
            connection.off('JoinGroupError', handleErr);
            connection.off('ReceiveNewMessage', handleReceiveNewMessage);
            connection.off('ReceiveOldMessage', handleReceiveOldMessage);
            connection.off('ErrorSendMessage', handleErrSendMessage)
            connection.off('SendSuccess', handleSendSuccess)
            connection.off('Error', handleErr)

        };
    }, [connection])

    return { join, more, messages, totalMsg, groupName, err, setMessages, send, errSend, leave, markLastRead, isReady }
}


export const useNotiReceived = () => {
    const connection = useSignalR();
    const [receivedNoti, setReceivedNoti] = useState(false);

    const handleReceiveNoti = response => {
        setReceivedNoti(true)
    }
    useEffect(() => {
        if (!connection) return;
        connection.on('UpdatePreview', handleReceiveNoti);
        return () => {
            connection.off('UpdatePreview', handleReceiveNoti);
        };
    }, [connection])
    return { receivedNoti, setReceivedNoti }
}


