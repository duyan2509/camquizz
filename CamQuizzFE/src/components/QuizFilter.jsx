import React from 'react';
import { Select, Radio, Button } from 'antd';

const QuizFilter = ({ home = false, genres, credential, setCredentials, reset }) => {
  const handleChange = value => {
    setCredentials(prev => ({
      ...prev,
      genreId: value
    }));
  };
  const handleChangeStatus = value => {
    console.log(`selected ${value}`);
    setCredentials(prev => ({
      ...prev,
      status: value
    }))
  };
  return (
    <div className="flex flex-wrap items-center gap-2">
      <span>Category:</span>
      <Select
        value={credential.genreId}
        style={{ width: 120 }}
        onChange={handleChange}
        options={genres.map(genre => ({
          value: genre.id,
          label: genre.name
        }))}
        className="mr-4"
      />
      {home && <>
        <span className="mr-2" >Status: </span>
        <Select
          defaultValue="All"
          style={{ width: 120 }}
          onChange={handleChangeStatus}
          options={[
            { value: 'All', label: 'All' },
            { value: 'Public', label: 'Public' },
            { value: 'Private', label: 'Private' },

          ]}
          className="mr-4"
          value={credential.status}
        />
      </>}
      <span>Sort by:</span>
      <Radio.Group
        className="mr-4"
        value={credential.popular}
        onChange={e => setCredentials(prev => ({
          ...prev,
          popular: e.target.value
        }))}>
        <Radio.Button value={true}>Popular</Radio.Button>
        <Radio.Button value={false}>Unpopular</Radio.Button>
      </Radio.Group>

      <Radio.Group
        className="mr-4"
        value={credential.newest}
        onChange={e => setCredentials(prev => ({
          ...prev,
          newest: e.target.value
        }))}>
        <Radio.Button value={true}>Newest</Radio.Button>
        <Radio.Button value={false}>Oldest</Radio.Button>
      </Radio.Group>

      <Button type="primary" onClick={reset}>Reset</Button>
    </div>
  );
};

export default QuizFilter;
