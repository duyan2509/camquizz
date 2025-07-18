import React, { useMemo } from 'react'
import { Input, Select, Button, Pagination, Empty, Modal, Alert, message } from 'antd';
const { Search } = Input;
import GroupQuizz from './GroupQuizz'
import { useGroupQuizzes } from '../../hooks/quizz';

const GroupQuizzes = ({ groupId, ownerId }) => {
    const currentUserId = useMemo(() => {
        try {
            const user = JSON.parse(localStorage.getItem('user'));
            console.log(user?.id || null)
            return user?.id || null;
        } catch {
            return null;
        }
    }, []);
    const isOwnerGroup = useMemo(() => ownerId === currentUserId, [ownerId, currentUserId]);
    const {
        data,
        total,
        fetch,
        credentials,
        setCredentials,
    } = useGroupQuizzes(groupId);
    const onSearch = (value, _e, info) => {
        setCredentials(prev=>({
            ...prev,
            keyword:value
        }))
    }
    return (
        <div>
            <div className="sticky top-0 z-1 pb-4 bg-white">
                <Search
                    placeholder="Enter quiz name"
                    allowClear
                    enterButton="Search"
                    size="large"
                    onSearch={onSearch}
                    className="mb-4"
                />
                <span><span className='text-blue-600'>{total}</span> shared quizz{total > 1 && "es"}</span>
            </div>

            <div className="grid sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {data.map((quiz, index) => (
                    <GroupQuizz
                     groupId={groupId} 
                     quiz={quiz} 
                     isOwnerGroup={isOwnerGroup} 
                     isAuthor={currentUserId === quiz.authorId} 
                     refresh={fetch}/>
                ))}
            </div>

            <Pagination
                className="mt-4 items-center justify-center "
                defaultCurrent={1}
                defaultPageSize={1}
                current={credentials.page}
                onChange={(page) => {
                    setCredentials(prev => ({
                        ...prev,
                        page
                    }))
                }}
                showSizeChanger={false}
                total={total} />
        </div>
    )
}

export default GroupQuizzes