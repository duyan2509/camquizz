import React from 'react'
import QuizCard from '../../components/QuizCard';
import { Input, Radio, Select, Button, Pagination, Empty } from 'antd';
const { Search } = Input;
const mockData = [
  { id: 1, name: 'Math Basics', category: 'Math', numberQuestions: 10, numberAttempts: 25, createAt: '2023-10-01' },
  { id: 2, name: 'Algebra Level 1', category: 'Math', numberQuestions: 12, numberAttempts: 30, createAt: '2023-10-03' },
  { id: 3, name: 'Physics Intro', category: 'Science', numberQuestions: 8, numberAttempts: 15, createAt: '2023-10-05' },
  { id: 4, name: 'World History', category: 'History', numberQuestions: 15, numberAttempts: 35, createAt: '2023-10-07' },
  { id: 5, name: 'English Grammar', category: 'English', numberQuestions: 20, numberAttempts: 50, createAt: '2023-10-09' },
  { id: 6, name: 'Biology 101', category: 'Science', numberQuestions: 11, numberAttempts: 22, createAt: '2023-10-10' },
  { id: 7, name: 'Geography Quiz', category: 'Geography', numberQuestions: 14, numberAttempts: 28, createAt: '2023-10-12' },
  { id: 8, name: 'Chemistry Basics', category: 'Science', numberQuestions: 9, numberAttempts: 18, createAt: '2023-10-14' },
  { id: 9, name: 'Literature Facts', category: 'English', numberQuestions: 13, numberAttempts: 26, createAt: '2023-10-16' },
  { id: 10, name: 'Ancient Civilizations', category: 'History', numberQuestions: 17, numberAttempts: 33, createAt: '2023-10-18' },
  { id: 11, name: 'Geometry Quiz', category: 'Math', numberQuestions: 10, numberAttempts: 20, createAt: '2023-10-20' },
  { id: 12, name: 'Statistics Basics', category: 'Math', numberQuestions: 16, numberAttempts: 32, createAt: '2023-10-22' },
  { id: 13, name: 'Environmental Science', category: 'Science', numberQuestions: 18, numberAttempts: 36, createAt: '2023-10-24' },
  { id: 14, name: 'Grammar Advanced', category: 'English', numberQuestions: 12, numberAttempts: 29, createAt: '2023-10-26' },
  { id: 15, name: 'Modern History', category: 'History', numberQuestions: 14, numberAttempts: 31, createAt: '2023-10-28' },
  { id: 16, name: 'Trigonometry', category: 'Math', numberQuestions: 13, numberAttempts: 27, createAt: '2023-10-30' },
  { id: 17, name: 'Volcanoes and Earthquakes', category: 'Geography', numberQuestions: 11, numberAttempts: 23, createAt: '2023-11-01' },
  { id: 18, name: 'Reading Comprehension', category: 'English', numberQuestions: 15, numberAttempts: 38, createAt: '2023-11-03' },
  { id: 19, name: 'Astronomy Basics', category: 'Science', numberQuestions: 9, numberAttempts: 19, createAt: '2023-11-05' },
  { id: 20, name: 'Civics & Government', category: 'Social Studies', numberQuestions: 10, numberAttempts: 24, createAt: '2023-11-07' },
];
const MyQuiz = () => {
  const [popularSort, setPopularSort] = React.useState('popular');
  const [timeSort, setTimeSort] = React.useState('newest');
  const [pagination, setPagination] = React.useState({
    page: 1,
    size: 20,
    total: 60
  })
  const onSearch = (value, _e, info) =>
    console.log(info === null || info === void 0 ? void 0 : info.source, value);
  const handleChange = value => {
    console.log(`selected ${value}`);
  };
  return (
    <div>
      <Search
        placeholder="Enter quiz name"
        allowClear
        enterButton="Search"
        size="large"
        onSearch={onSearch}
        className="mb-4"
      />
      <div className="flex">
        <span className="mr-2" >Category: </span>
        <Select
          defaultValue="All"
          style={{ width: 120 }}
          onChange={handleChange}
          options={[
            { value: 'jack', label: 'Jack' },
            { value: 'lucy', label: 'Lucy' },
            { value: 'Yiminghe', label: 'yiminghe' },
          ]}
          className="mr-4"
        />
        <span className="mr-2" >Sort by: </span>
        <Radio.Group
          className="mr-4"
          value={popularSort} onChange={e => setPopularSort(e.target.value)}>
          <Radio.Button value="popular">Popular</Radio.Button>
          <Radio.Button value="-popular">Unpopular</Radio.Button>
        </Radio.Group>
        <Radio.Group
          className="mr-4"
          value={timeSort} onChange={e => setTimeSort(e.target.value)}>
          <Radio.Button value="newest">Newest</Radio.Button>
          <Radio.Button value="-newest">Oldest</Radio.Button>
        </Radio.Group>
        <Button className="mr-2 ml-auto" type="primary">Apply</Button>
        <Button type="default">Reset</Button>
      </div>
      <div className="flex-row ">
        <span><span className="text-blue-600 font-semibold text-xl">{pagination.total}</span> found</span>
        {
          mockData !== null && mockData.length > 0 ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-4">
              {mockData.map(quiz =>
                <QuizCard quiz={quiz} key={quiz.id} guest={false} />
              )}
            </div>
          ) :
            <Empty />
        }
        <Pagination
          className="mt-4 itmes-center justify-center "
          defaultCurrent={1}
          defaultPageSize={pagination.size}
          current={pagination.page}
          onChange={(page, size) => setPagination({ ...pagination, page, size })}
          showSizeChanger={false}
          total={pagination.total} />
      </div>
    </div>
  )
}

export default MyQuiz