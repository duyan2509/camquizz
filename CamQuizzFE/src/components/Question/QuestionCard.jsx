import React, { useState, useEffect } from 'react';
import { Card, Typography, Image, Tag, Space } from 'antd';
import { RetweetOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useDeleteQuestion } from '../../hooks/quizz';
import { useMessage } from '../../hooks/useMessage';
const QuestionCard = ({ quizId, question, onEdit }) => {
    const [isFront, setIsFront] = useState(true);
    const { success, error, contextHolder } = useMessage();
    const { successDelete, errDelete, remove, loading } = useDeleteQuestion(quizId);
    useEffect(() => {
        if (successDelete) {
            success("Delete question successfully");
        }
        if (errDelete) {
            error(errDelete);
        }
    }
    , [loading]);
    const front = () => (
        <div className="flex flex-col h-full">
            <div className="mb-4 flex justify-start gap-4">
                <Typography.Text>
                    Duration: <Tag color="blue">{question.durationSecond}s</Tag>
                </Typography.Text>
                <Typography.Text>
                    Point: <Tag color="gold">{question.point}</Tag>
                </Typography.Text>
            </div>
            <div className="flex-1 flex items-center justify-center">
                {question.image ? (
                    <Image
                        src={question.image}
                        alt="question"
                        width={290}
                        height={366}
                        style={{ marginBottom: 8 }}
                    />
                ):
                <Typography.Text className="text-gray-500">No question image </Typography.Text>}
            </div>
        </div>
    );

    const back = () => (
        <div className="flex flex-col h-full">
            <Typography.Text strong>Answers:</Typography.Text>
            <div className="flex-1 overflow-y-auto mt-2">
                {question.answers.map((ans) => (
                    <Card
                        key={ans.id}
                        size="small"
                        className={`mb-2 ${ans.isCorrect ? 'border-green-300' : 'border-red-300'
                            } border-2`}
                    >
                        <Space>
                            <Typography.Text>{ans.content}</Typography.Text>
                            {ans.image && <Image src={ans.image} alt="answer" width={40} />}
                            {ans.isCorrect ? (
                                <Tag color="green">Correct</Tag>
                            ) : (
                                <Tag color="red">Wrong</Tag>
                            )}
                        </Space>
                    </Card>
                ))}
            </div>
        </div>
    );

    return (
        <div className="h-[500px]"> 
        {contextHolder}
            <Card
                className="h-full border-2 rounded-lg flex flex-col"
                title={
                    <Space>
                        <Typography.Text>{question.content}</Typography.Text>
                    </Space>
                }
                actions={[
                    <RetweetOutlined key="refresh" onClick={() => setIsFront((prev) => !prev)} />,
                    <EditOutlined key="edit" onClick={onEdit} />,
                    <DeleteOutlined key="more" onClick={()=>{
                        remove(question.id)
                    }}/>,
                ]}
                bodyStyle={{ flex: 1, overflow: 'hidden', display: 'flex', flexDirection: 'column' }} 
            >
                <div className="flex-1 h-full">
                    <div className={`${isFront ? 'block' : 'hidden'} h-full`}>{front()}</div>
                    <div className={`${!isFront ? 'block' : 'hidden'} h-full`}>{back()}</div>
                </div>
            </Card>
        </div>
    );
};

export default QuestionCard;
