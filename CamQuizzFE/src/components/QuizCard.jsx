import React from 'react'
import { Card, Button, Tag, Dropdown } from 'antd'
const items = [
    {
        label: 'Information',
        key: '1',
    },
    {
        label: 'Access',
        key: '2',
    },
    {
        label: 'Questions',
        key: '3',
    },
    {
        label: 'View Reports',
        key: '4',
    },
];



const QuizCard = ({ quiz, guest = true }) => {
    console.log(quiz.id)
    const handleButtonClick = e => {
        message.info('Click on left button.');
        console.log('click left button', e);
    };
    const handleMenuClick = e => {
        message.info('Click on menu item.');
        console.log('click', e);
    };
    const menuProps = {
        items,
        onClick: handleMenuClick,
    };
    return (
        <Card
            hoverable
            cover={<img alt="example" src="https://os.alipayobjects.com/rmsportal/QBnOOoLaAfKPirc.png" />}>
            <p className='font-medium text-blue-600 text-xl truncate'>{quiz.name}</p>
            <Tag color="geekblue">{quiz.category}</Tag>
            <p>{quiz.numberQuestions} questions - {quiz.numberAttempts} attempts</p>
            <p className="text-gray-500 text-sm">Created at {quiz.createAt}</p>
            <div className={`mt-4 grid ${guest ? `grid-cols-1` : `grid-cols-2`} gap-2`}>
                <Button type="primary">Start Quiz</Button>
                {!guest && <Dropdown.Button menu={menuProps} onClick={handleButtonClick}>
                    Setting
                </Dropdown.Button>}
            </div>
        </Card>
    )
}

export default QuizCard