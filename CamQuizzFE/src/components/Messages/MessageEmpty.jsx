import React from 'react'
import {Empty, Typography } from 'antd';

const MessageEmpty = () => {
    return (
        <Empty
            description={
                <Typography.Text>
                    Select <a >group</a> from left panel
                </Typography.Text>
            }
        >
        </Empty>
    )
}

export default MessageEmpty