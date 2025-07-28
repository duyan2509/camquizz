import React from 'react'
import QuizCard from '../../components/QuizCard';
import { Input, Radio, Select, Button, Pagination, Empty, Collapse } from 'antd';
import { useGenres } from '../../hooks/useGenres';
import { useAllQuizzes } from '../../hooks/quizz'
import QuizFilter from '../../components/QuizFilter';
const { Search } = Input;

const MyQuiz = () => {
  const { genres, errGenres } = useGenres();
  const { errQuiz, quizzes, credential, setCredentials, total, reset } = useAllQuizzes(false);

  const onSearch = (value) => {
    setCredentials(prev => ({
      ...prev,
      keyword: value,
      page: 1
    }));
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
      <Collapse items={[{
        key: '1',
        label: <span><span className="text-blue-600 font-semibold text-xl">{total}</span> found</span>,
        children: <QuizFilter
          home={true}
          genres={genres}
          credential={credential}
          setCredentials={setCredentials}
          reset={reset}
        />
      }]} defaultActiveKey={['1']} />
      <div className="flex-row ">
        {
          quizzes !== null && quizzes.length > 0 ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-4">
              {quizzes.map(quiz =>
                <QuizCard quiz={quiz} key={quiz.id} guest={false} />
              )}
            </div>
          ) :
            <Empty />
        }
        <Pagination
          className="mt-4 itmes-center justify-center "
          defaultCurrent={1}
          defaultPageSize={1}
          current={credential.page}
          onChange={(page, size) => setCredentials(prev => ({ ...prev, page }))}
          showSizeChanger={false}
          total={total} />
      </div>
    </div>
  )
}

export default MyQuiz