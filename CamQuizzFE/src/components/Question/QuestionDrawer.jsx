import React, { useState, useEffect, use } from 'react'
import { Drawer, Space, Button, Steps, Form, Image, Upload, Input, InputNumber, Radio } from 'antd';
import { UploadOutlined, EditOutlined, MinusCircleOutlined, PlusOutlined } from '@ant-design/icons'
import { useMessage } from '../../hooks/useMessage';
import { useCreateQuestion, useGetQuestion, useUpdateQuestion } from '../../hooks/quizz';
const steps = [
    {
        title: 'Information Question',
    },
    {
        title: 'Answers',
    }
]
var __rest =
    (this && this.__rest) ||
    function (s, e) {
        var t = {};
        for (var p in s)
            if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
        if (s != null && typeof Object.getOwnPropertySymbols === 'function')
            for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
                if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                    t[p[i]] = s[p[i]];
            }
        return t;
    };
const QuestionDrawer = ({
    edit = false,
    create = false,
    view = false,
    questionId,
    quizId,
    open,
    onClose,
    afterClose
}) => {
    const { error, success, contextHolder } = useMessage();
    const { successCreate, errCreate, question, addQuestion } = useCreateQuestion(quizId);
    const { question: questionDetail, err: errGetQuestion, loading: loadingGetQuestion, get } = useGetQuestion(quizId);
    const { successUpadate, errUpdate, update } = useUpdateQuestion(quizId);
    const [form] = Form.useForm();
    const [preview, setPreview] = useState(null)
    const [image, setImage] = useState(null)

    const [current, setCurrent] = React.useState(0);
    const next = () => {
        setCurrent(current + 1);
    };
    const prev = () => {
        setCurrent(current - 1);
    };
    const onFinish = values => {
        console.log('Form values:', values);
    };
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
    const onSubmitCreate = () => {
        console.log('Submitting form with values:', form.getFieldsValue(true));
        if (image)
            form.setFieldValue('image', image);
        const values = form.getFieldsValue(true);
        addQuestion(values)
    };
    const onSubmitUpdate = () => {
        console.log('Submitting form with values:', form.getFieldsValue(true));
        if (image)
            form.setFieldValue('image', image);
        const values = form.getFieldsValue(true);
        console.log("question id ", questionDetail.id)
        update(questionDetail.id, values)
    };

    useEffect(() => {
        if (successCreate) {
            success("Create question successfully");
            onClose();
            afterClose();
        }
        if (successUpadate) {
            success("Update question successfully");
            onClose();
            afterClose();
        }
    }, [successCreate, successUpadate])
    useEffect(() => {
        if (errCreate) {
            error(errCreate);
        }
        if(errUpdate) {
            error(errUpdate);
        }
    }, [errCreate, errUpdate]);
    useEffect(() => {
        if (questionId && (edit || view)) {
            get(questionId);
        }
    }
        , [questionId, edit]);
    useEffect(() => {
        if (questionDetail) {
            form.setFieldsValue({
                content: questionDetail.content,
                image: questionDetail.image,
                durationSecond: questionDetail.durationSecond,
                point: questionDetail.point,
                answers: questionDetail.answers.map(ans => ({
                    id: ans.id,
                    content: ans.content,
                    isCorrect: ans.isCorrect
                }))
            });
            setPreview(questionDetail.image);
        }
    }, [questionDetail]);


    if (!create && loadingGetQuestion)
        return <div className='flex items-center justify-center h-full'>Loading...</div>
    return (
        <Drawer
            title={create ? "Create Question" : edit ? "Edit Question" : "View Question"}
            size="large"
            open={open}
            onClose={onClose}
            closable={{ 'aria-label': 'Close Button' }}
            extra={
                !view &&
                <Space>
                    <Button onClick={onClose}>Cancel</Button>
                    {create && <Button onClick={onSubmitCreate} type="primary">
                        Create
                    </Button>}
                    {edit && <Button onClick={onSubmitUpdate} type="primary">
                        Update
                    </Button>}
                </Space>
            }
            className='flex flex-col'
        >
            {contextHolder}
            <Steps
                current={current}
                size='small'
                items={steps}
            />
            <Form
                className='h-[85%] overflow-y-auto mt-4'
                form={form}
                name="validate_other"
                onFinish={onFinish}
            >

                {current === 0 && (
                    <div className='flex flex-col items-center justify-center'>
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
                            <Form.Item
                                defaultValue={questionDetail?.content || ""}
                                name="content"
                                label="Question"
                                rules={[{ required: true, message: 'Please input Content' },
                                { min: 2, message: 'Content must be at least 2 characters' },
                                { max: 500, message: 'Content must be less than 500 characters' }
                                ]}
                            >
                                <Input.TextArea showCount maxLength={500} minLength={2} />
                            </Form.Item>
                            <Form.Item
                                defaultValue={questionDetail?.point || 1}
                                label="Point"
                                name="point"
                                rules={[{ required: true, message: 'Please input question point!' },
                                { type: 'number', min: 0, message: 'Point must be a positive number' }
                                ]}
                            >
                                <InputNumber min={0} />
                            </Form.Item>
                            <Form.Item
                                defaultValue={questionDetail?.durationSecond || 1}
                                label="Duration (seconds)"
                                name="durationSecond"
                                rules={[{ required: true, message: 'Please input question point!' },
                                { type: 'number', min: 0, message: 'Point must be a positive number' }
                                ]}
                            >
                                <InputNumber min={0} />
                            </Form.Item>

                        </Form.Item>
                    </div>
                )}
                {current === 1 && (
                    <Form.List
                        defaultValue={questionDetail?.answers || []}
                        name="answers">
                        {(fields, { add, remove }) => (
                            <>
                                {fields.map(_a => {
                                    var { key, name } = _a,
                                        restField = __rest(_a, ['key', '']);
                                    return (
                                        <Space key={key} style={{ display: 'flex', marginBottom: 8 }} align="baseline">
                                            <Form.Item
                                                {...restField}
                                                name={[name, 'content']}
                                                rules={[{ required: true, message: 'Missing answer content' },
                                                { min: 2, message: 'Content must be at least 2 characters' },
                                                { max: 120, message: 'Content must be less than 120 characters' }
                                                ]}
                                            >
                                                <Input placeholder="Answer content" min={2} max={120} />
                                            </Form.Item>
                                            <Form.Item
                                                {...restField}
                                                name={[name, 'isCorrect']}
                                                rules={[{ required: true, message: 'Missing result' }]}
                                            >
                                                <Radio.Group>
                                                    <Radio.Button value={true}>True</Radio.Button>
                                                    <Radio.Button value={false}>False</Radio.Button>
                                                </Radio.Group>
                                            </Form.Item>
                                            <MinusCircleOutlined onClick={() => remove(name)} />
                                        </Space>
                                    );
                                })}
                                <Form.Item>
                                    <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                                        Add field
                                    </Button>
                                </Form.Item>
                            </>
                        )}
                    </Form.List>
                )}
            </Form>
            <div className='h-[5%] flex justify-stretch items-center mt-4'>
                {current < steps.length - 1 && (
                    <Button type="primary" onClick={() => next()}>
                        Next
                    </Button>
                )}
                {current > 0 && (
                    <Button style={{ margin: '0 8px' }} onClick={() => prev()}>
                        Previous
                    </Button>
                )}
            </div>
        </Drawer>
    );
};

export default QuestionDrawer