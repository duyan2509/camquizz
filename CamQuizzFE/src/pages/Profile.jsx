import React from 'react'
import { useNavigate, useLocation } from 'react-router-dom';
import { Tabs, Button } from 'antd';
import AttemptHistory from '../pages/AttemptHistory';
import UserProfile from '../pages/UserProfile';
import Packages from '../pages/Packages';
import ReportHistory from '../pages/ReportHistory'
import HostingHistory from '../pages/HostingHistory';

const items = [
  {
    key: 'user-profile',
    label: 'User Profile',
    children: <UserProfile />,
  },
  {
    key: 'attempt-history',
    label: 'Attempt History',
    children:  <AttemptHistory />,
  },
  {
    key: 'hosting-history',
    label: 'Hosting History',
    children:  <HostingHistory />,
  },
  {
    key: 'report-history',
    label: 'Report History',
    children:  <ReportHistory />,
  },
  {
    key: 'packages',
    label: 'Packages',
    children:  <Packages />,
  },
];
const operations = <Button danger type="primary">Logout</Button>;
const Profile = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const onChange = key => {
  location.pathname = `/profile/${key}`;
  navigate(location.pathname);
};
  return (
    <Tabs defaultActiveKey="1"
      items={items}
      onChange={onChange}
      tabBarExtraContent={operations} 
      />
  )
}

export default Profile