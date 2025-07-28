import React from 'react'
import { useParams } from 'react-router-dom'

const QuizzReport = () => {
    const { id: quizId } = useParams();

    return (
        <div>QuizzReport {quizId}</div>
    )
}

export default QuizzReport