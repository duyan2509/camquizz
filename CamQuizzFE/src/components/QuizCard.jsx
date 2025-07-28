import React from 'react'
import { Card, Button, Tag, Dropdown } from 'antd'
import {convertToVNTime} from '../utils'
import { useNavigate } from 'react-router-dom'



const QuizCard = ({ quiz, guest = true }) => {
    const navigate = useNavigate();
    const handleButtonClick = e => {
        console.log("click")
        navigate(`/myquiz/${quiz.id}`);
    };
    const handleMenuClick = e => {
        message.info('Click on menu item.');
    };

    return (
        <Card
            hoverable
            cover={<img alt="example" src={quiz.image ||"https://os.alipayobjects.com/rmsportal/QBnOOoLaAfKPirc.png"} />}>
            <p className='font-medium text-blue-600 text-xl truncate'>{quiz.name}</p>
            <Tag color="geekblue">{quiz.genreName}</Tag>
            {!guest &&<Tag color="gold">{quiz.status}</Tag>}
            <p>{quiz.numberOfQuestions} questions - {quiz.numberOfAttemps} attempts</p>
            <p className="text-gray-500 text-sm">Last update at {convertToVNTime(quiz.updatedAt)}</p>
            <div className={`mt-4 grid ${guest ? `grid-cols-1` : `grid-cols-2`} gap-2`}>
                <Button type="primary">Start Quiz</Button>
                {!guest && <Button  onClick={handleButtonClick}>
                    Setting
                </Button>}
            </div>
        </Card>
    )
}

export default QuizCard