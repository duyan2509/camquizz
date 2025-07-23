import React from 'react';
import UserLayout from '../components/UserLayout';
import Home from '../pages/Quiz/Home';
import MyQuiz from '../pages/Quiz/MyQuiz';
import MyGroup from '../pages/Group/MyGroup';
import Profile from '../pages/Profile/Profile';
import CreateQuiz from '../pages/Quiz/CreateQuiz';
import Login from '../pages/Auth/Login';
import RequireAuth from './RequireAuth';
import Unauthorized from '../components/Unauthorized';
import NotFound from '../components/NotFound';
import AttemptHistory from '../pages/Profile/AttemptHistory';
import UserProfile from '../pages/Profile/UserProfile';
import Packages from '../pages/Profile/Packages';
import ReportHistory from '../pages/Profile/ReportHistory'
import HostingHistory from '../pages/Profile/HostingHistory';
import Register from '../pages/Auth/Register'
import DetailGroup from '../pages/Group/DetailGroup';
import Messages from '../pages/Messages/Messages';
export const userRoutes = [
    {
        path: '/',
        element: <UserLayout />,
        children: [
            { index: true, element: <Home /> },
            { path: 'login', element: <Login /> },
            { path: 'register', element: <Register /> },
            { path: 'unauthorized', element: <Unauthorized /> },
            {
                element: <RequireAuth allowedRoles={['User', 'Admin']} />,
                children: [
                    { path: 'myquiz', element: <MyQuiz /> },
                    { path: 'mygroup', element: <MyGroup /> },
                    { path: 'mygroup/:id', element: <DetailGroup /> },
                    { path: 'messages', element: <Messages /> },
                    { path: 'messages/:id', element: <Messages /> },
                    {
                        path: 'profile',
                        element: <Profile />,
                        children: [
                            { index: true, element: <UserProfile /> },
                            { path: 'user-profile', element: <UserProfile /> },
                            { path: 'attempt-history', element: <AttemptHistory /> },
                            { path: 'packages', element: <Packages /> },
                            { path: 'report-history', element: <ReportHistory /> },
                            { path: 'hosting-history', element: <HostingHistory /> },
                        ]
                    },
                    { path: 'createquiz', element: <CreateQuiz /> },
                ],
            },
        ],
    },
    { path: '*', element: <NotFound /> }

];
