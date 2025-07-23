import React, { useEffect, useState, useRef } from 'react'
import { Input, Form, Button } from 'antd';
import {
  SendOutlined, ArrowLeftOutlined
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom'

import MessageEmpty from './MessageEmpty'
import Message from './Message'
import { useChat } from '../../hooks/useChat'
import { useMessage } from '../../hooks/useMessage';
const Conversation = ({ userId, groupId }) => {
  const navigate = useNavigate();
  const [sendRequest] = Form.useForm();
  const chatBoxRef = useRef(null);
  const [loadClicked, setLoadClicked] = useState(false);
  const { join, messages, totalMsg, groupName, err, more, send, errSend, leave, isReady } = useChat();
  const { success, warning, error, contextHolder } = useMessage();
  useEffect(() => {
    if (groupId != null && isReady)
      join({
        groupId,
        afterId:null,
        afterCreatedAt:null
      })
    return () => {
      if (groupId != null)
        leave(groupId)
    }
  }, [groupId, isReady])

  useEffect(() => {
    const box = chatBoxRef.current;
    if (box && loadClicked) {
      setLoadClicked(false)
      box.scrollTop = 0;
      return
    }
    if (box) {
      box.scrollTop = box.scrollHeight;
    }


  }, [messages]);
  useEffect(() => {
    if (errSend != null)
      error(errSend);
    if (err != null)
      error(err)
  }, [errSend, err])

  const onFinish = (values) => {
    console.log('send', values)
    send(values)
    sendRequest.resetFields();
  };

  const onFinishFailed = (errorInfo) => {
    console.log('Failed:', errorInfo);
  };

  if (!groupId)
    return <MessageEmpty />

  return (
    <div className='flex flex-col h-full w-full rounded-md border'>
      {contextHolder}
      <header className="h-[5%] ml-4 mr-4 text-3xl mt-4 mb-4 flex-wrap">
        <ArrowLeftOutlined className="mr-4 sm:hidden" onClick={()=>navigate("/messages")}/>
        {groupName}
      <hr className="mt-4"/>
      </header>

      <main ref={chatBoxRef} className="flex-grow overflow-x-auto mr-4 ml-4 mt-4">
        {totalMsg > messages.length &&
          <Button type='link'
            className='w-full'
            onClick={() => {
              setLoadClicked(true)
              more()
            }}>Load More</Button>}
        {
          messages.map((msg, index) => (
            <Message
              key={index}
              message={msg}
              you={userId === msg.senderId}
              showName={index === 0 || msg.senderId !== messages[index - 1].senderId}
            />
          ))
        }

      </main>
      <footer className="h-[15%] gap-4 ml-4">
        <Form
          form={sendRequest}
          size='large'
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
          className='flex w-full items-center gap-4'
        >

          <Form.Item
            className=' w-[90%]'
            name="content"
            rules={[{ required: true, message: 'Please enter message!' },
            { max: 500, message: "Message is over 500 characters" }
            ]}
          >
            <Input.TextArea
              count={{
                show: true,
                max: 500,
              }}
              placeholder="Enter message"
              className="w-full"
            />
          </Form.Item>

          <Form.Item className="w-[10%]">
            <Button className="" size="large" htmlType="submit">
              <SendOutlined style={{ fontSize: '32px', color: '#08c' }} />
            </Button>
          </Form.Item>
        </Form>
      </footer>
    </div>
  )
}

export default Conversation