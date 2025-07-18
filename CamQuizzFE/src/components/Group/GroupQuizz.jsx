import React, { useEffect, useState } from 'react'
import { Card, Button, Tag, message } from 'antd'
import { convertToVNTime } from './../../utils'
import { useVisible, useRemove } from '../../hooks/quizz'
const GroupQuizz = ({ groupId, quiz, isOwnerGroup = false, isAuthor, refresh }) => {
    const [messageApi, contextHolder] = message.useMessage();
    const { update, loading, err, success } = useVisible(groupId, quiz.quizzId);
    const { err: errRemove, message: messageRemove, remove } = useRemove(groupId, quiz.quizzId)
    const [isHide, setIsHide] = useState(quiz.isHide)
    const handleRemove = async e => {
        await remove();
    };
    const handleVisible = async () => {
        const visible = isHide
        const result = await update(visible)
        if (result) {
            setIsHide(!visible)
            messageApi.open({
                type: 'success',
                content: 'Update successfully'
            })
        }
    }
    useEffect(() => {
        if (err)
            messageApi.open({
                type: 'error',
                content: err
            })
        if (messageRemove) {
            messageApi.open({
                type: 'success',
                content: messageRemove
            })
            refresh();
        }
    }, [err, messageRemove])
    return (
        <Card
            hoverable
            cover={<img alt="example" src="https://os.alipayobjects.com/rmsportal/QBnOOoLaAfKPirc.png" />}>
            {contextHolder}

            <p className='font-medium text-blue-600 text-xl truncate'>{quiz.name}</p>
            <Tag color="geekblue">{quiz.genreName}</Tag>
            <p>{quiz.numberOfQuestions} questions - {quiz.numberOfAttemps} attempts</p>
            <p className="text-gray-500 text-sm">Shared at {convertToVNTime(quiz.shareAt)} by <span class="text-blue-600">{quiz.authorName}</span></p>
            <Button className="mt-4" block type="primary">Start Quiz</Button>

            {isAuthor && <Button
                onClick={handleRemove}
                className="mt-2" block danger type="primary">Remove</Button>}
            {isOwnerGroup && <Button
                onClick={handleVisible}
                className="mt-2" block >{isHide ? 'Show' : 'Hide'}</Button>}
        </Card>
    )
}

export default GroupQuizz