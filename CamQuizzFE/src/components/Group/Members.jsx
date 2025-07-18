import React, { useMemo, useEffect } from 'react'
import { UserDeleteOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { Button, Form, Input, message } from 'antd'
import { useMembers, useAddMember, useKick, useLeave } from "../../hooks/member"
const Members = ({ groupId, ownerId }) => {
    const [messageApi, contextHolder] = message.useMessage();
    const navigate = useNavigate();
    const currentUserId = useMemo(() => {
        try {
            const user = JSON.parse(localStorage.getItem('user'));
            console.log(user?.id || null)
            return user?.id || null;
        } catch {
            return null;
        }
    }, []);
    const isOwner = useMemo(() => ownerId === currentUserId, [ownerId, currentUserId]);
    const { data, total, loading, refresh, loadMore } = useMembers(groupId, { page: 1, size: 10 });
    const { loading: loadingAdd, error: errorAdd, success: successAdd, addMember } = useAddMember();
    const { loading: loadingKick, error: errorKick, success: successKick, kick } = useKick();
    const { loading: loadingLeave, error: errorLeave, success: successLeave, onLeave } = useLeave();

    useEffect(() => {
        if (errorKick)
            messageApi.open({
                type: 'error',
                content: errorKick
            })
        if (errorAdd)
            messageApi.open({
                type: 'error',
                content: errorAdd
            })
        if (successAdd) {
            refresh()
            messageApi.open({
                type: 'success',
                content: 'Add member successfull'
            })
        }
        if (successKick) {
            refresh()
            messageApi.open({
                type: 'success',
                content: 'Kick member successfull'
            })
        }
        if (successLeave) {

            messageApi.open({
                type: 'success',
                content: 'Leave group successfull'
            })
            setTimeout(() => {
                navigate('/mygroup');
            }, 1500);

        }

    }, [errorAdd, successAdd, successKick, successLeave, errorKick, errorLeave])
    const handleAddMember = async (values) => {
        const res = addMember(groupId, values.email);
    };
    const handleKick = async (id) => {
        const res = await kick(groupId, id);
    }
    const handleLeave = async () => {
        await onLeave(groupId);
    }
    return (
        <div>
            {contextHolder}
            {isOwner ?
                <Form
                    onFinish={handleAddMember}
                >
                    <Form.Item name="email" label=""
                        rules={[
                            { required: true, message: 'Please input email type!' },
                            { type: 'email', message: 'Invalid email format' },
                        ]}
                    >
                        <Input
                            size='large'
                            placeholder="Enter email to invite" />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" block htmlType="submit">
                            Add
                        </Button>
                    </Form.Item>
                </Form> :
                <Button
                    className='mb-4'
                    onClick={handleLeave}
                    size='large'
                    danger block>Leave</Button>
            }
            <span className="ml-2"><span className='text-blue-600'>{total}</span> member{total > 1 && "s"}</span>
            <ul className="space-y-2 mt-2">
                {data.map((member, index) => (
                    <li key={index} className="flex justify-between group p-2"
                    >
                        <span>
                            {member.fullName}
                            {member.id === ownerId && " - Owner"}
                        </span>

                        {isOwner && member.id !== ownerId && (
                            <UserDeleteOutlined
                                className="text-red-500 invisible group-hover:visible cursor-pointer"
                                onClick={() => handleKick(member.id)}
                            />
                        )}
                    </li>
                ))}
            </ul>
            {total > data.length &&
                <Button
                    loading={loading}
                    onClick={loadMore}
                    block type="link">
                    Load more
                </Button>
            }
        </div>

    )
}

export default Members