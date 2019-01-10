import Mock from 'mockjs'
import { setData } from '@/mock/commonResultData'

export const GetAtomicCounter = () => {
  return setData(Mock.mock({
    'MsgSuccessCount|1-10000000': 100,
    'MsgFailCount|1-10000000': 100
  }))
}
