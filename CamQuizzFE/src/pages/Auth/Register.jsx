import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { register , reset } from '../../features/auth/authSlice';
import { Button, Form, Input, Card, message } from 'antd';

const Register = () => {
    const [messageApi, contextHolder] = message.useMessage();
    const dispatch = useDispatch();
    const { status, error } = useSelector((state) => state.auth);
    const navigate = useNavigate();
    const handleSubmit = values => {
        dispatch(register(values));
    };
    useEffect(() => {
        if (status === 'succeeded')
        {
            messageApi.open({
                type:'success',
                content:'Registration successful! Please login.'
            })
            navigate('/login');
        }
        if (status === 'failed' && error)
            messageApi.open({
                type: 'error',
                content: error
            })
            
        return ()=>{
            dispatch(reset());
        }
    }, [status])
    useEffect(()=>{
        if(error)
            message.error(error);
        return ()=>{
            dispatch(reset());
        }
    },[error])
    return (
        <div className="flex justify-center bg-white-100">
            {contextHolder}

            <Card
                title={<p className="text-blue-600">Register</p>}
                hoverable
            >
                <Form
                    name="login"
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 16 }}
                    style={{ maxWidth: 600 }}
                    initialValues={{ remember: true }}
                    onFinish={handleSubmit}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Email"
                        name="email"
                        rules={[
                            { required: true, message: 'Please input your email!' },
                            { type: 'email', message: 'Please input a valid email!' }
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        label="Username"
                        name="username"
                        rules={[{ required: true, message: 'Please input your username!' }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        label="Password"
                        name="password"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input.Password />
                    </Form.Item>
                    <Form.Item
                        label="First Name"
                        name="firstName"
                        rules={[{ required: true, message: 'Please input your first name!' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        label="Last Name"
                        name="lastName"
                        rules={[{ required: true, message: 'Please input your last name!' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        label="Phone"
                        name="phoneNumber"
                        rules={[{ required: true, message: 'Please input your phone number!' },
                        { pattern: /^\d{10}$/, message: 'Please input a valid phone number!' }
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item label={null}>
                        <Button block disabled={status === 'loading'} type="primary" htmlType="submit">
                            Register
                        </Button>
                    </Form.Item>
                    <Form.Item label={null}>
                        <Button block type="link" onClick={() => navigate('/login')}>
                            Login
                        </Button>
                    </Form.Item>
                </Form>
            </Card>

        </div>
    )
}

export default Register