import Mock from 'mockjs'
import { setData } from '@/mock/commonResultData'

export default {
  SendText () {
    return setData(Mock.mock({
      'boolean|1': true
    }))
  }
}
