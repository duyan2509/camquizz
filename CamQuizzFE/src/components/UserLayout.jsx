import React, { useState, useEffect} from 'react';
import { BookOutlined, PlusOutlined, UserOutlined, TeamOutlined, MessageOutlined} from '@ant-design/icons';
import { Input } from 'antd';
import { useNavigate, useLocation, Outlet } from 'react-router-dom'
const PAGES = {
  Home: '/',
  MyQuiz: '/myquiz',
  MyGroup: '/mygroup',
  Profile: '/profile',
  CreateQuiz: '/createquiz',
  Login: '/login',
  Message:'/messages'
}
const UserLayout = () => {
  const [activeTab, setActiveTab] = useState(PAGES.Home);
  const [isLoggedIn, setIsLoggedIn] = useState(localStorage.getItem('token')?true:false);
  const location = useLocation();
  const isActive = (path) => location.pathname === path;
  const navigate = useNavigate();
  useEffect(()=>{
    setIsLoggedIn(localStorage.getItem('token')?true:false);
  },[localStorage.getItem('token')])
  const onChange = text => {
    console.log('onChange:', text);
  };
  const onInput = value => {
    console.log('onInput:', value);
  };
  const sharedProps = {
    onChange,
    onInput,
  };
  return (
    <div className="flex flex-col min-h-screen bg-white">
      {/* Header */}
      <header className="bg-white shadow-sm border-b sticky top-0 z-10 pb-4">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            {/* Left Section: Logo and PIN */}
            <div className="flex items-center space-x-8">
              {/* Logo */}
              <div className="flex items-center space-x-2"
                onClick={() => {
                  setActiveTab(PAGES.Home)
                  if (!isActive(PAGES.Home))
                    navigate(PAGES.Home)
                }
                }>
                <BookOutlined className="text-2xl text-blue-600" />
                <span className="text-xl font-bold text-gray-900">CamQuizz</span>
              </div>

              {/* PIN Code */}
              <div className="flex items-center space-x-2">
                <span className="text-sm text-gray-600">PIN:</span>
                <Input.OTP
                  className="font-mono text-lg font-semibold text-blue-600 bg-blue-50 px-3 py-1 rounded-lg border"
                  length={5}
                  {...sharedProps} />

              </div>
            </div>

            {/* Right Section: Navigation and Actions */}
            <div className="flex items-center space-x-6">
              {/* Create Quiz Button */}
              <button className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center space-x-2 transition-colors shadow-sm"
                onClick={() => {
                  setActiveTab(PAGES.CreateQuiz)
                  if (!isActive(PAGES.CreateQuiz))
                    navigate(PAGES.CreateQuiz)
                }
                }
              >
                <PlusOutlined className="text-sm" />
                <span>Create Quiz</span>
              </button>

              {/* Navigation Tabs */}
              <nav className="flex space-x-1">
                <button
                  className={`flex items-center space-x-2 px-4 py-2 rounded-lg transition-colors ${activeTab === PAGES.MyQuiz ? 'bg-blue-100 text-blue-700 font-medium' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-100'
                    }`}
                  onClick={
                    () => {
                      setActiveTab(PAGES.MyQuiz)
                      if (!isActive(PAGES.MyQuiz))
                        navigate(PAGES.MyQuiz)
                    }
                  }
                >
                  <BookOutlined className="text-sm" />
                  <span>My Quiz</span>
                </button>
                <button
                  className={`flex items-center space-x-2 px-4 py-2 rounded-lg transition-colors ${activeTab === PAGES.MyGroup ? 'bg-blue-100 text-blue-700 font-medium' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-100'
                    }`}
                  onClick={() => {
                    setActiveTab(PAGES.MyGroup)
                    if (!isActive(PAGES.MyGroup))
                      navigate(PAGES.MyGroup)
                  }
                  }
                >
                  <TeamOutlined className="text-sm" />
                  <span>My Group</span>
                </button>
                 <button
                  className={`flex items-center space-x-2 px-4 py-2 rounded-lg transition-colors ${activeTab === PAGES.Message ? 'bg-blue-100 text-blue-700 font-medium' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-100'
                    }`}
                  onClick={() => {
                    setActiveTab(PAGES.Message)
                    if (!isActive(PAGES.Message))
                      navigate(PAGES.Message)
                  }
                  }
                >
                  <MessageOutlined className="text-sm" />
                  <span>Messages</span>
                </button>
              </nav>


              {/* Profile Icon */}
              {
                isLoggedIn ? (
                  <button className={`p-2 rounded-full hover:bg-gray-100 transition-colors ${activeTab === PAGES.Profile ? 'bg-blue-100 text-blue-700 font-medium' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-100'
                    }`}
                    onClick={() => {
                      setActiveTab(PAGES.Profile)
                      if (!isActive(PAGES.Profile))
                        navigate(PAGES.Profile)
                    }
                    }
                  >
                    <UserOutlined className="text-lg text-gray-600" />
                  </button>
                ) :
                  (
                    <button className={`p-2 rounded-full hover:bg-gray-100 transition-colors ${activeTab === PAGES.Profile ? 'bg-blue-100 text-blue-700 font-medium' : 'text-gray-600 hover:text-gray-900 hover:bg-gray-100'
                      }`}
                      onClick={() => {
                        setActiveTab(PAGES.Login)
                        if (!isActive(PAGES.Login))
                          navigate(PAGES.Login)
                      }
                      }
                    >
                      <span className="text-lg text-blue-600"
                      > Login</span>
                    </button>
                  )
              }
            </div>
          </div>
        </div>
      </header>

      {/* Body placeholder to show the header in context */}
      <main className="flex-grow max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 w-full">
        <Outlet />
      </main>
      {/* Footer */}
      <footer className="bg-white border-t">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex flex-col sm:flex-row justify-between items-center text-sm text-gray-500">
          <span>&copy; {new Date().getFullYear()} CamQuizz. All rights reserved.</span>
        </div>
      </footer>
    </div>
  );
};

export default UserLayout;