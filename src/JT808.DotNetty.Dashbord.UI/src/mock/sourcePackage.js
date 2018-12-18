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
  GetAll  () {
    return setData([{
      'RemoteAddress': '127.0.0.1:6665',
      'Registered': true,
      'Active': true,
      'Open': true
    }, {
      'RemoteAddress': '127.0.0.1:6667',
      'Registered': true,
      'Active': true,
      'Open': true
    }])
  }
}
