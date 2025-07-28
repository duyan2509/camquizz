import React, { useMemo } from 'react';
import { Tabs, Grid,Button } from 'antd';
import { useParams, useLocation, useNavigate, Outlet } from 'react-router-dom';

const { useBreakpoint } = Grid;

const tabRoutes = [
  { key: 'information', label: 'Quiz Information' },
  { key: 'question-setting', label: 'Question Setting' },
  { key: 'report', label: 'Reports' },
];
const QuizzSetting = () => {
  const { id: quizId } = useParams();
  const location = useLocation();
  const navigate = useNavigate();
  const screens = useBreakpoint();

  const isSmallScreen = useMemo(() => !screens.md, [screens]);

  const currentKey = useMemo(() => {
    const match = location.pathname.match(/\/myquiz\/[^/]+\/([^/]+)$/);
    return match ? match[1] : 'information';
  }, [location.pathname]);

  const handleTabChange = (key) => {
    navigate(`/myquiz/${quizId}/${key}`);
  };

  return (
    <div className="" >
      <Tabs
        className=""
        activeKey={currentKey}
        onChange={handleTabChange}
        tabPosition={isSmallScreen ? 'top' : 'left'}
        items={tabRoutes.map((tab) => ({
          key: tab.key,
          label: tab.label,
          children: <Outlet />,
        }))}
        tabBarStyle={{ height: '100%' }}
      />

    </div>
  );
};

export default QuizzSetting;
