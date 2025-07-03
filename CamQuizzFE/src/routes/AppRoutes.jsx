import React from 'react';
import UserLayout from '../components/UserLayout';
import Home from '../pages/Home';
import MyQuiz from '../pages/MyQuiz';
import MyGroup from '../pages/MyGroup';
import Profile from '../pages/Profile';
import CreateQuiz from '../pages/CreateQuiz';
import Login from '../pages/Login';
import RequireAuth from './RequireAuth';
import Unauthorized from '../pages/Unauthorized';
import NotFound from '../pages/NotFound'; 
import AttemptHistory from '../pages/AttemptHistory';
import UserProfile from '../pages/UserProfile';
import Packages from '../pages/Packages';
import ReportHistory from '../pages/ReportHistory'
import HostingHistory from '../pages/HostingHistory';
export const userRoutes = [
    {
        path: '/',
        element: <UserLayout />,
        children: [
            { index: true, element: <Home /> },
            { path: 'login', element: <Login /> },
            { path: 'unauthorized', element: <Unauthorized /> },
            {
                element: <RequireAuth allowedRoles={['user', 'admin']} />,
                children: [
                    { path: 'myquiz', element: <MyQuiz /> },
                    { path: 'mygroup', element: <MyGroup /> },
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
                        ]},
                    { path: 'createquiz', element: <CreateQuiz /> },
                ],
            },
        ],
    },
    { path: '*', element: <NotFound /> }

];
