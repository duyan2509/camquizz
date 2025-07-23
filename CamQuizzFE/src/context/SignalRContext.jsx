import React, { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { notification } from 'antd';
import {useConversations} from '../hooks/group'
const SignalRContext = createContext(null);

export const SignalRProvider = ({ children }) => {
    const [api, contextHolder] = notification.useNotification();

    const [connection, setConnection] = useState(null);
    const [token, setToken] = useState(() => localStorage.getItem("token"));
    const handleReceiveNewMessage = response => {
        console.log('noti', response)
        api['info']({
            message: response.groupName,
            description:
                `${response.message.sender}: ${response.message.message}`,
        });

    }
    useEffect(() => {
        const handleStorageChange = () => {
            setToken(localStorage.getItem("token"));
        };
        window.addEventListener("storage", handleStorageChange);
        return () => window.removeEventListener("storage", handleStorageChange);
    }, []);

    useEffect(() => {
        if (!token) return;
        const conn = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:5001/chat", {
                accessTokenFactory: () => localStorage.getItem('token')
            })
            .withAutomaticReconnect()
            .build();

        conn.start()
            .then(() => {
                console.log("SignalR Connected");
                conn.on('NotifyNewMessage', handleReceiveNewMessage);
                conn.onreconnected(() => {
                    console.log("SignalR Reconnected");
                    conn.on("NotifyNewMessage", handleReceiveNewMessage); // rebind
                });
                setConnection(conn);

            })
            .catch(console.error);

        return () => {
            conn.stop();
        };
    }, [token]);

    return (
        <SignalRContext.Provider value={connection}>
            {contextHolder}

            {children}
        </SignalRContext.Provider>
    );
};

export const useSignalR = () => useContext(SignalRContext);
