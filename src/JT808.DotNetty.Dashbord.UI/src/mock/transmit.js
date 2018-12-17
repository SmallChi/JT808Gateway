import Mock from 'mockjs'
import { setData } from '@/mock/commonResultData'

export default {
  Add () {
    return setData(Mock.mock({
      'boolean|1': true
    }))
  },
  Remove () {
    return setData(Mock.mock({
      'boolean|1': true
    }))
  },
  GetAll () {
    return setData([
      '127.0.0.1:6665',
      '127.0.0.1:6667'
    ])
  }
}
