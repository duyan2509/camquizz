import React from 'react'
import { Card, Typography } from 'antd'
import { useTimeFromNow } from '../../hooks/useTimeFromNow'
// {
//         groupId: "3fa85f64-5717-4562-b3fc-2c963f660010",
//         groupName: "Hội bạn cấp 3",
//         lastMessage: "Tối nay đi ăn không?",
//         senderName: "Huỳnh Thị J",
//         lastMessageAt: "2025-07-18T14:59:49.817Z",
//         unreadCount: 0
//     }
const PreviewMessage = ({ conversation, onSelect, selectGroup }) => {
  const timeAgo = useTimeFromNow(conversation.lastMessageAt)

  return (
    <div
      onClick={() => {
        onSelect();
      }}
      className={`p-4 mb-2 ${conversation.groupId === selectGroup ? 'rounded-lg border bg-blue-100 border-blue-500' : ' bg-white'}`}
    >
      <Typography.Title level={4}
        className="truncate w-[200px]"
      >
        {conversation.groupName}
      </Typography.Title>
      <div className="flex ">
        <span className={`w-4/5 inline-block truncate w-[full] ${conversation.unreadCount>0?'text-blue-500':''}`}>
          {conversation.senderName}{conversation.lastMessageAt!=null?': ':<br/>}{conversation.lastMessage}
        </span>
        {conversation.lastMessageAt!=null&&<span className={`w-1/5 text-sm ${conversation.unreadCount>0?'text-blue-500':''}`}>
          {timeAgo}
        </span>}
      </div>
      {conversation.unreadCount>0 && <span className="text-gray-600 italic">{conversation.unreadCount} unread messages</span>}
    </div>
  )
}

export default PreviewMessage