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
    return setData([{
      IP: '127.0.0.1',
      Port: 6667
    },
    {
      IP: '127.0.0.1',
      Port: 6668
    },
    {
      IP: '127.0.0.1',
      Port: 6669
    },
    {
      IP: '127.0.0.1',
      Port: 6670
    }
    ])
  }
}
