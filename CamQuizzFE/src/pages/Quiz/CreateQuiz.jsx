import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Upload, Button, Typography, Form, Select, Radio, Input, Flex } from 'antd'
import { UploadOutlined, EditOutlined } from '@ant-design/icons'
import { useCreateQuizz } from '../../hooks/quizz'
import { useMessage } from '../../hooks/useMessage'
import { useGenres } from '../../hooks/useGenres'
import { useGroups} from '../../hooks/group'
const CreateQuiz = () => {
  const navigation = useNavigate();
  const { quiz, successCreate, errCreate, createQuizz } = useCreateQuizz();
  const { success, warning, error, contextHolder } = useMessage()
  const { data:groups, setData, total, loading, error: errGroups } = useGroups({ page: 1, size: 100 });
  const { genres, errGenres } = useGenres(false);
  const [form] = Form.useForm();
  const [image, setImage] = useState(null)
  const [preview, setPreview] = useState(null)
  const [isPrivate, setPrivate] = useState(false)
  const status = Form.useWatch('status', form);
  React.useEffect(() => {
    if (status === "Private")
      setPrivate(true)
    else if (status === "Public") {
      setPrivate(false)
      form.setFieldsValue({ groupIds: [] })
    }
  }, [status])
  React.useEffect(() => {
    if (errCreate) {
      error(errCreate)
    }
  }, [errCreate])
  React.useEffect(() => {
    if (successCreate) {
      success("Create successfull")
      form.resetFields();
    }
  }, [successCreate]);
  const handleChange = (info) => {
    const file = info.fileList.length > 0 ? info.fileList[info.fileList.length - 1].originFileObj : null
    if (file) {
      setImage(file)
      setPreview(URL.createObjectURL(file))
    }
  }
  const handleRemove = () => {
    setImage(null)
    setPreview(null)
  }
  const onFinish = values => {
    const formData = new FormData();
    formData.append('name', values.name);
    formData.append('genreId', values.genreId);
    formData.append('status', values.status);

    if (values.status === 'Private') {
      values.groupIds.forEach(id => {
        formData.append('groupIds', id);
      });
    }

    if (image) {
      formData.append('image', image);
    }

    console.log('Received values of form: ', formData);

    createQuizz(formData)
  };

  return (
    <div className='flex w-full flex-col items-center'>
      {contextHolder}
      <Form
        form={form}
        name="validate_other"
        className="p-6 border rounded-lg bg-white md:max-w-2xl w-full "
        onFinish={onFinish}
      >
        <Typography.Title level={4}>Quiz Information Config</Typography.Title>

        <div className='flex flex-col md:flex-row md:gap-8'>
          <Form.Item
            className="w-[290px]"
            name="image" >
            <div
              className={`relative w-[290px] h-[366px] rounded-lg overflow-hidden mb-4 flex items-center justify-center ${preview ? '' : 'bg-[#f0f2f5]'
                }`}
            >
              {preview ? (
                <img
                  src={preview}
                  alt="cover"
                  className="absolute inset-0 w-full h-full object-cover rounded-lg"
                />
              ) : (
                <span className="text-gray-400">Empty Cover</span>
              )}
              <Upload
                showUploadList={false}
                beforeUpload={() => false}
                onRemove={handleRemove}
                onChange={handleChange}
                accept="image/*"
              >
                <Button
                  icon={preview ? <EditOutlined /> : <UploadOutlined />}
                  className="absolute bottom-4 right-4 bg-white/80 rounded-full font-medium"
                >
                  {preview ? 'Change' : 'Select'}
                </Button>
              </Upload>
            </div>
          </Form.Item>
          <div>

            <Form.Item
              name="name" label="Name" rules={[{ required: true }]}>
              <Input />
            </Form.Item>
            <Form.Item
              name="genreId"
              label="Category"
              hasFeedback
              rules={[{ required: true, message: 'Please select quiz category!' }]}
            >
              <Select
                options={genres.map(genre => ({
                  value: genre.id,
                  label: genre.name
                }))}
                placeholder="Please select a category">
              </Select>
            </Form.Item>
            <Form.Item name="status"
              label="Visible Status"
              rules={[{ required: true, message: 'Please pick a status!' }]}
            >
              <Radio.Group
              >
                <Radio.Button value="Public">Public</Radio.Button>
                <Radio.Button value="Private">Private</Radio.Button>
              </Radio.Group>
            </Form.Item>
            {isPrivate && <Form.Item
              name="groupIds"
              label="Private Group"
              rules={[{ required: isPrivate, message: 'Please select your private groups!', type: 'array' }]}
            >
              <Select
                mode="multiple" 
                 options={groups.map(group => ({
                  value: group.id,
                  label: group.name
                }))}
                placeholder="Please select private groups">
              </Select>
            </Form.Item>}
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Create Quizz
              </Button>
            </Form.Item>
          </div>
        </div>

        {quiz && <Typography.Paragraph className='text-center'>
          You can set up questions for quiz {quiz.name}  <Typography.Link onClick={() => navigation(`myquiz/${quiz.id}`)}>here</Typography.Link>
        </Typography.Paragraph>}
      </Form>

    </div>
  )
}

export default CreateQuiz