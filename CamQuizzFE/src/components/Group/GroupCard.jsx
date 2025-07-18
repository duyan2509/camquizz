import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, Button, Tag, Typography, message, Popconfirm } from 'antd'
import { DeleteOutlined } from '@ant-design/icons';
const { Title } = Typography;
import { useUpdateGroup, useDeleteGroup } from '../../hooks/group'
const GroupCard = ({ group, afterSuccessAction }) => {
    const [messageApi, contextHolder] = message.useMessage();
    const navigate = useNavigate();
    const { handleUpdateGroup, error, success } = useUpdateGroup();
    const { error: errorDelete, success: successDelete, handleDeleteGroup } = useDeleteGroup();
    const [name, setName] = useState(group.name);
    const [isOwner, setIsOwner] = useState();
    useEffect(() => {
        const user = JSON.parse(localStorage.getItem('user'));
        setIsOwner(user && user.id === group.ownerId)
    }, [group.ownerId])
    const handleChangeName = async (newName) => {
        console.log(newName, group.id);
        const newGroup = await handleUpdateGroup(group.id, newName)
        if (newGroup) {
            setName(newGroup.name);
        }
    }
    useEffect(() => {
        if (error) {
            messageApi.open({
                type: 'error',
                content: error || '123456789'
            })
        }
        if (errorDelete) {
            messageApi.open({
                type: 'error',
                content: errorDelete || '123456789'
            })
        }
    }, [error, errorDelete])

    useEffect(() => {
        if (success) {
            messageApi.open({
                type: 'success',
                content: 'Group name updated successfully!'
            })
        }

    }, [success])
    const confirm = async e => {
        const deleted = await handleDeleteGroup(group.id);
        if (deleted) {
            afterSuccessAction();
        }
    };
    const cancel = e => {

    };
    return (
        <Card
            hoverable>
            {contextHolder}
            <div
                onClick={
                    () => {
                        navigate(`/mygroup/${group.id}`);
                    }}
            >

                {isOwner ? <Title level={4} editable={{ onChange: handleChangeName }}>{name}</Title>
                    : <Title level={4} >{name}</Title>}
                <div className="flex" >
                    {isOwner ? <Tag color="blue">Owner</Tag>
                        : <Tag color="cyan">Member</Tag>
                    }
                    {group.memberCount > 1 ? <Tag color="magenta">{group.memberCount} members</Tag>
                        : <Tag color="magenta">{group.memberCount} member</Tag>
                    }
                    {group.amountSharedQuizz > 1 ? <Tag color="volcano">{group.amountSharedQuizz} share quizzes</Tag>
                        : <Tag color="volcano">{group.amountSharedQuizz} share quizzes</Tag>
                    }
                </div>
                <p className="text-gray-500 text-sm mt-4">Owner: {group.ownerName}</p>
            </div>
            {isOwner &&
                <Popconfirm
                    title="Delete the group"
                    description="Are you sure to delete this group?"
                    onConfirm={confirm}
                    onCancel={cancel}
                    okText="Yes"
                    cancelText="No"
                >
                    <Button type="primary" block danger className="mt-4">
                        Delete
                        <DeleteOutlined />
                    </Button>
                </Popconfirm>
            }

        </Card>
    )
}

export default GroupCard