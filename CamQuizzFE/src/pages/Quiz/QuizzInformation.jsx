import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { UploadOutlined, EditOutlined } from '@ant-design/icons'
import { validateAccess } from '../../utils'
import { useDetailQuiz } from '../../hooks/quizz'
import { useMessage } from '../../hooks/useMessage'
import { useGenres } from '../../hooks/useGenres'
import { useGroups } from '../../hooks/group'
import { useNavigate } from 'react-router-dom'
import { useUpdateAcess, useUpdateInfo, useDeleteQuizz } from '../../hooks/quizz'
import { Card, Upload, Button, Spin, Form, Select, Radio, Input, Popconfirm } from 'antd'
const QuizzInformation = () => {
    const { id: quizId } = useParams();
    const navigate = useNavigate();
    const [accessForm] = Form.useForm();
    const [infoForm] = Form.useForm();
    const [image, setImage] = useState(null);
    const [preview, setPreview] = useState(null)
    const { quiz, successDetalQuizz, errDetailQuizz, setQuiz } = useDetailQuiz(quizId);
    const { success, warning, error, contextHolder } = useMessage()
    const { data: groups, setData, total, loading, error: errGroups } = useGroups({ page: 1, size: 100 });
    const { genres, errGenres } = useGenres(false);
    const { access, successAccess, errAccess, updateAccess } = useUpdateAcess(quizId);
    const { successInfo, errInfo, info, updateInfo } = useUpdateInfo(quizId)
    const { successDelete, errDelete, remove, loading: loadingDeleteQuiz} = useDeleteQuizz(quizId);
    const [isPrivate, setPrivate] = useState(false)
    const statusCheck = Form.useWatch('status', accessForm);
    React.useEffect(() => {
        if (statusCheck === "Private")
            setPrivate(true)
        else if (statusCheck === "Public") {
            setPrivate(false)
            accessForm.setFieldsValue({ groupIds: [] })
        }
    }, [statusCheck])
    useEffect(() => {
        if (quiz) {
            setImage(quiz.image)
            setPreview(quiz.image)
        }
    }, [quiz]);
    useEffect(() => {
        if (successAccess) {
            success("Access settings updated successfully")
            setQuiz(prev => ({
                ...prev,
                status: access.status,
                groupIds: access.groupIds || []
            }))
        } else if (errAccess) {
            error(errAccess)
        }
    }, [successAccess]);
    useEffect(() => {
        if (successInfo && info) {
            success("Infor settings updated successfully")
            setQuiz(prev => ({
                ...prev,
                image: info.image,
                genreId: info.genreId,
                name: info.name
            }))
        } else if (errInfo) {
            error(errInfo)
        }
    }, [successInfo]);
    useEffect(() => {
        if (successDelete) {
            success("Quiz deleted successfully");
            navigate('/myquiz');
        } else if (errDelete) {
            error(errDelete);
        }
    }, [loadingDeleteQuiz]);
    const onInfoFinish = values => {
        const formData = new FormData();
        var isModified = false;
        if (quiz.name !== values.name) {
            formData.append('name', values.name);
            isModified = true;
        }
        if (quiz.genreId !== values.genreId) {
            formData.append('genreId', values.genreId);
            isModified = true;
        }

        if (image && image !== quiz.image) {
            formData.append('image', image);
            isModified = true;
        }

        if (!isModified) {
            error("No changes detected");
            return;
        }
        console.log('Received values of form: ', formData);
        updateInfo(formData)
    };


    const onAccessFinish = values => {
        console.log('Access Form Values:', values);
        const result = validateAccess(values);
        if (!result.isValid) {
            error(result.message || "Invalid access settings");
            return;
        }
        updateAccess(values)
    };
    const handleChange = (info) => {
        const file = info.fileList.length > 0 ? info.fileList[info.fileList.length - 1].originFileObj : null
        if (file) {
            console.log('Selected file:', file);
            setImage(file)
            setPreview(URL.createObjectURL(file))
        }
    }
    const handleRemove = () => {
        setImage(null)
        setPreview(null)
    }
    const onDelete = e => {
        remove(quizId)
    };

    if (!successDetalQuizz || loadingDeleteQuiz)
        return <Spin fullscreen />
    return (
        <div>
            {contextHolder}
            <Card
                className='mb-4'
                title={
                    <div className='flex items-center justify-between'>
                        <span className='text-blue-600'>Quiz Information</span>
                        <Popconfirm
                            title="Delete the quizz"
                            description="Are you sure to delete this quiz, include all questions?"
                            onConfirm={onDelete}
                            okText="Yes"
                            cancelText="No"
                        >
                            <Button
                                type="primary"
                                className="ml-4"
                                danger
                                
                            >Delete Quizz</Button>
                        </Popconfirm>
                    </div>}>
                <Form
                    form={infoForm}
                    name="validate_other"
                    className="p-6   md:max-w-2xl w-full "
                    onFinish={onInfoFinish}
                    initialValues={{
                        name: quiz.name,
                        genreId: quiz.genreId,
                        image: quiz.image
                    }}
                >
                    <div className='flex flex-col md:flex-row md:gap-8'>
                        <Form.Item
                            className="w-[290px]"
                            name="image" >
                            <div
                                className={`relative w-[290px] h-[366px] overflow-hidden mb-4 flex items-center justify-center ${preview ? '' : 'bg-[#f0f2f5]'
                                    }`}
                            >
                                {image ? (
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

                            <Form.Item>
                                <Button type="primary" htmlType="submit">
                                    Save
                                </Button>
                            </Form.Item>
                        </div>
                    </div>


                </Form>
            </Card>
            <Card
                title={<span className='text-blue-600'>Quiz Accesses</span>} >
                <Form
                    form={accessForm}
                    name="validate_other"
                    onFinish={onAccessFinish}
                    initialValues={{
                        status: quiz.status,
                        groupIds: quiz.groupIds || []
                    }}
                >

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
                            Save
                        </Button>
                    </Form.Item>

                </Form>
            </Card>

        </div>
    )
}

export default QuizzInformation