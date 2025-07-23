import { useEffect, useState } from 'react'
import dayjs from 'dayjs'
import utc from 'dayjs/plugin/utc'
import relativeTime from 'dayjs/plugin/relativeTime'
import timezone from 'dayjs/plugin/timezone'
import 'dayjs/locale/vi'

dayjs.extend(utc)
dayjs.extend(relativeTime)
dayjs.extend(timezone)

export function useTimeFromNow(
  timestamp,
  refreshInterval = 60000
) {
  const getRelativeTime = () =>
    dayjs.utc(timestamp).tz('Asia/Ho_Chi_Minh').locale('vi').fromNow()

  const [time, setTime] = useState(getRelativeTime())

  useEffect(() => {
    const interval = setInterval(() => {
      setTime(getRelativeTime())
    }, refreshInterval)

    return () => clearInterval(interval)
  }, [timestamp, refreshInterval])

  return time
}
