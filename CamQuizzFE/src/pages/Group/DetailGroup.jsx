import React from 'react';
import { useParams } from 'react-router-dom';
import { useGroup } from '../../hooks/group';
import Unauthorized from '../../components/Unauthorized';
import { Typography, Spin } from 'antd';
import { convertToVNTime } from '../../utils'
import Members from '../../components/Group/Members';
import GroupQuizzes from '../../components/Group/GroupQuizzes';
const DetailGroup = () => {
    const { id } = useParams();
    const { error, success, group, loading } = useGroup(id);

    if (loading) {
        return (
            <div style={{ textAlign: 'center', paddingTop: 100 }}>
                <Spin size="large" tip="Loading group details..." />
            </div>
        );
    }

    if (error) {
        return <Unauthorized />;
    }

    return (
        <div className="flex-col bg-white">
            {/* Header */}
            <header className="  ">
                <Typography.Title>
                    {group?.name || 'Group'}
                </Typography.Title>
                <Typography.Paragraph>
                    <span className="text-blue-600">{group?.ownerName || 'Owner Name'}</span> create group at <span className="text-blue-600">{convertToVNTime(group?.createdAt) || 'Time'}</span>
                </Typography.Paragraph>
            </header>

            <div className="flex h-screen">
                <main className="w-4/5 mr-2 overflow-y-auto h-full">
                    <GroupQuizzes groupId={id} ownerId={group?.ownerId} />
                </main>

                <aside className="w-1/5 ml-2 overflow-y-auto h-full">
                    <Members groupId={id} ownerId={group?.ownerId} />
                </aside>
            </div>
        </div>

    );
};

export default DetailGroup;
