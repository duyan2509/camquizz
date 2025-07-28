import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { Input, Button, Pagination, Radio, Spin } from 'antd';
import { useMessage } from '../../hooks/useMessage';
import QuestionCard from '../../components/Question/QuestionCard';
import QuestionDrawer from '../../components/Question/QuestionDrawer';
import { useQuestions } from '../../hooks/quizz';
const { Search } = Input;

const QuestionSetting = () => {
    const { id: quizId } = useParams();
    const { questions, errQuestions, loading, total, credentials, setCredentials, fetch } = useQuestions(quizId);
    const [selectedQuestion, setSelectedQuestion] = React.useState(null);
    const [open, setOpen] = useState(false);
    const [create, setCreate] = useState(false);
    const [edit, setEdit] = useState(false);
    console.log("open", open)
    const onSearch = (value, _e, info) => {
        console.log(info === null || info === void 0 ? void 0 : value)
        if(value)
            setCredentials({
                ...credentials,
                keyword: value,
                page: 1
            });
    }

    const handleClose = () => {
        console.log('Closing drawer...');
        setOpen(false);
        setEdit(false);
        setCreate(false);
        setSelectedQuestion(null);
    };

    if(loading)
        return <Spin fullscreeen={true} tip="Loading questions..." />

    return (
        <div className='flex flex-col p-4'>
            <div className="flex mb-4">
                <Search
                    placeholder="Enter question"
                    allowClear
                    enterButton="Search"
                    size="large"
                    onSearch={onSearch}
                    defaultValue={credentials.keyword}
                />
                <Button className="ml-4 "
                    type="default"
                    size="large"
                    onClick={() => {
                        setOpen(true);
                        setCreate(true);
                        setEdit(false);
                    }}>Create new Question</Button>
            </div>
            <div className="flex mb-4">
                <h1                     
                className="mr-4 flex  items-center"    >
                    <span className="text-blue-600 mr-2">{total} </span> {total>1?'results found' : 'result found'}
                </h1>
                <Radio.Group
                    className="mr-4"
                    defaultValue={credentials.newest}
                    onChange={(e) => setCredentials({ ...credentials, newest: e.target.value })}>
                    <Radio.Button value={true}>Newest</Radio.Button>
                    <Radio.Button value={false}>Oldest</Radio.Button>
                </Radio.Group>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {
                    questions.map((question) => (
                        <QuestionCard
                            key={question.id}
                            quizId={quizId}
                            question={question}
                            onEdit={() => {
                                setSelectedQuestion(question.id)
                                setEdit(true);
                                setCreate(false);
                                setOpen(true);
                            }} />
                    ))
                }
            </div>
            <Pagination
                className="mt-4 itmes-center justify-center "
                defaultCurrent={1}
                defaultPageSize={credentials.size}
                current={credentials.page}
                showSizeChanger={false}
                onChange={(page, size) => {
                    setCredentials({
                        ...credentials,
                        page: page,
                        size: size
                    });
                }}
                total={total/credentials.size} />
            <QuestionDrawer
                quizId={quizId}
                questionId={selectedQuestion}
                create={create}
                edit={edit}
                open={open}
                onClose={handleClose}
                afterClose={fetch} 
                />
        </div>
    )
}

export default QuestionSetting