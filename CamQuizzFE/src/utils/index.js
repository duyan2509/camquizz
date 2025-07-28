export const convertToVNTime = (isoDateString) => {
  if (!isoDateString) return '';
  const date = new Date(isoDateString);

  const vnTime = new Date(date.getTime() + 7 * 60 * 60 * 1000);

  const formatted = vnTime.toLocaleString('vi-VN', {
    hour: '2-digit',
    minute: '2-digit',
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });

  return formatted;
};

export const validateAccess = (values) => {
  const { status, groupIds } = values;
  console.log(status, groupIds);
  const isSameStatus = status === quiz.status;

  const isSameGroupIds =
    Array.isArray(quiz.groupIds) &&
    Array.isArray(groupIds) &&
    quiz.groupIds.length === groupIds.length &&
    quiz.groupIds.every(id => groupIds.includes(id));

  if (isSameStatus && status === "Public") {
    return {
      isValid: false,
      message: "No changes detected"
    };
  }
  if (isSameStatus && isSameGroupIds && status === "Private") {
    return {
      isValid: false,
      message: "No changes detected"
    };
  }
  if (status === "Public") {
    if (!groupIds || groupIds.length === 0) {
      return {
        isValid: true,
        message: "Access settings updated"
      };
    } else {
      return {
        isValid: false,
        message: "Public quizzes should not have group restrictions"
      };
    }
  }

  if (status === "Private") {
    if (!groupIds || groupIds.length === 0) {
      return {
        isValid: false,
        message: "Private quizzes must have at least one group selected"
      };
    }

    const allGroupIdsValid = groupIds.every(id =>
      groups.some(group => group.id === id)
    );

    if (!allGroupIdsValid) {
      return {
        isValid: false,
        message: "All selected groups must be valid"
      };
    }

    return {
      isValid: true,
      message: "Access settings updated"
    };
  }

  return {
    isValid: false,
    message: "Invalid status"
  };
};
