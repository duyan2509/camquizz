import { useEffect, useState } from 'react'
import dayjs from 'dayjs'
import relativeTime from 'dayjs/plugin/relativeTime'
import 'dayjs/locale/vi'

dayjs.extend(relativeTime)
dayjs.locale('vi')

export function useTimeFromNow(
  timestamp,
  refreshInterval = 60000 
) {
  const getRelativeTime = () => dayjs(timestamp).fromNow()
  const [time, setTime] = useState(getRelativeTime())

  useEffect(() => {
    const interval = setInterval(() => {
      setTime(getRelativeTime())
    }, refreshInterval)

    return () => clearInterval(interval)
  }, [timestamp, refreshInterval])

  return time
}
