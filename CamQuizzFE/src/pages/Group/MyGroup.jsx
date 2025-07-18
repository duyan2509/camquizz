import React, { useEffect } from 'react'
import GroupCard from '../../components/Group/GroupCard';
import { Input, Select, Button, Pagination, Empty, Modal, Alert, message } from 'antd';
const { Search } = Input;
import { useGroups, useCreateGroup } from '../../hooks/group'
import { useModal } from '../../hooks/useModal'

const MyGroup = () => {
  const [messageApi, contextHolder] = message.useMessage();
  const [groupName, setGroupName] = React.useState("")
  const [credentials, setCredentials] = React.useState({
    page: 1,
    size: 10,
    isOwner: '',
    keyword: ''
  });
  const { handleCreateGroup, error: errorCreateGroup, newGroup } = useCreateGroup();
  const { isModalOpen, showModal, handleOk, handleCancel } = useModal(
    // logic before handle ok
    async () => {
      return await handleCreateGroup(groupName)
    },
    // logic before cancel
    () => {
      setGroupName('')
    }
  );
  const { data, total, isLoading, error } = useGroups(credentials);
  const onSearch = (value, _e, info) => {
    setCredentials({ ...credentials, keyword: info === null || info === void 0 ? void 0 : value, page: 1 });
  }
  const handleChange = value => {
    setCredentials({ ...credentials, isOwner: value });
  };
  const afterAction = (action) => {
    messageApi.open({
      type: 'success',
      content: `${action} successfully!`
    })
    setCredentials({ ...credentials, isOwner: true, page: 1 })
  }
  useEffect(() => {
    if (newGroup)
      afterAction("Create group");
  }, [newGroup])

  return (
    <div>
      {contextHolder}
      <div className="flex mb-4">
        <Search
          placeholder="Enter quiz name"
          allowClear
          enterButton="Search"
          size="large"
          onSearch={onSearch}
        />
        <Button className="ml-4 " type="default" size="large" onClick={showModal}>Create your own group</Button>
      </div>
      <div className="flex">
        <span className="mr-2"><span className="text-blue-600 font-semibold text-xl">{total}</span> founded with </span>
        <Select
          defaultValue=""
          style={{ width: 120 }}
          onChange={handleChange}
          options={[
            { value: "", label: 'All' },
            { value: true, label: 'Owner' },
            { value: false, label: 'Member' },
          ]}
          className="mr-4"
        />
        <span>
          <span className="">
            <span className="text-xl"></span>
            role{" "}
            {credentials.keyword !== '' && (
              <span>
                with keyword <span className="text-xl font-semibold text-blue-600">{credentials.keyword}</span>:
              </span>
            )}
          </span>
        </span>

      </div>
      <div className="flex-row ">
        {
          data !== null && data.length > 0 ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-4">
              {data.map(group =>
                <GroupCard group={group} key={group.id} afterSuccessAction={()=>{
                  afterAction("Delete group")
                }} />
              )}
            </div>
          ) :
            <Empty />
        }
        <Pagination
          className="mt-4 itmes-center justify-center "
          defaultCurrent={1}
          defaultPageSize={credentials.size}
          current={credentials.page}
          onChange={(page, size) => setCredentials({ ...credentials, page, size })}
          showSizeChanger={false}
          total={total} />
      </div>
      <Modal
        title="Create your own group"
        closable={{ 'aria-label': 'Custom Close Button' }}
        open={isModalOpen}
        onOk={handleOk}
        onCancel={handleCancel}
        okButtonProps={{
          disabled: groupName === ''
        }}
      >
        <Input
          size="large"
          placeholder="Input your group name"
          onChange={(e) => setGroupName(e.target.value)}
          required
          value={groupName}
        />
        {
          errorCreateGroup && groupName !== '' && <Alert className="mt-4" message={errorCreateGroup} type="error" />
        }
      </Modal>
    </div>
  )
}

export default MyGroup