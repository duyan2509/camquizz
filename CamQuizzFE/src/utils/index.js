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
