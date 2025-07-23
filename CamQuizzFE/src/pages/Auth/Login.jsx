import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { login, reset } from '../../features/auth/authSlice';
import { Button, Form, Input, Card, message, Flex } from 'antd';

const Login = () => {
  const [messageApi, contextHolder] = message.useMessage();

  const dispatch = useDispatch();
  const { status, error } = useSelector((state) => state.auth);
  const navigate = useNavigate();
  const handleSubmit = values => {
    dispatch(login(values));
  };
  useEffect(() => {
    if (status === 'succeeded' && localStorage.getItem('token'))
    {
      messageApi.open({
        type: 'success',
        content: "Login successful!"
      }) 
      navigate('/');
    }
    if (status === 'failed' && error)
      messageApi.open({
        type: 'error',
        content: error
      })
    return () => {
      dispatch(reset());
    }
  }, [localStorage.getItem('token'), status])
  return (
    <div className="flex justify-center bg-white-100">
      {contextHolder}

      <Card
        title={<p className="text-blue-600">Login</p>}
        hoverable
        variant={true}
      >
        <Form
          name="login"
          initialValues={{ remember: true }}
          labelCol={{ span: 8 }}
          style={{ maxWidth: 600 }}
          onFinish={handleSubmit}
          autoComplete="off"
        >
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your Username!' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Username" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your Password!' }]}
          >
            <Input prefix={<LockOutlined />} type="password" placeholder="Password" />
          </Form.Item>
          <Form.Item>
            <Flex justify="flex-end" align="center">
              <a href="">Forgot password</a>
            </Flex>
          </Form.Item>

          <Form.Item>
            <Button
              block
              disabled={status === 'loading'} type="primary" htmlType="submit">
              Login
            </Button>
          </Form.Item>
          <Button block type="link" onClick={() => navigate('/register')}>
            Register
          </Button>
        </Form>
      </Card>

    </div>
  )
}

export default Login