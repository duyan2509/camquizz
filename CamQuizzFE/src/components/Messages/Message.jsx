import React from 'react'
import { Tooltip } from 'antd'
import { convertToVNTime } from '../../utils/index'
const Message = ({ message, you, showName }) => {

  return (
    <div >
      <div className={`${you ? ' justify-end' : ' justify-start'} flex `}>
        {showName && <div>{message.sender}</div>}
      </div>

      <div className={`${you ? ' justify-end' : ' justify-start'} flex `}>
        <Tooltip title={convertToVNTime(message.time)} >
          <div className={`rounded-xl p-2 mb-2 w-max ${you ? 'bg-gray-300 text-black ' : 'bg-blue-500 text-white'} `}>{message.message}</div>
        </Tooltip>
      </div>

    </div>
  )
}

export default Message