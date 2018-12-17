import Mock from 'mockjs'
import { setData } from '@/mock/commonResultData'

export default {
  RemoveByTerminalPhoneNo () {
    return setData(Mock.mock({
      'boolean|1': true
    }))
  },
  RemoveByChannelId () {
    return setData(Mock.mock({
      'boolean|1': true
    }))
  },
  GetAll () {
    return setData([{
      'ChannelId': 'eadad23',
      'LastActiveTime': '2018-11-27 20:00:00',
      'StartTime': '2018-11-25 20:00:00',
      'TerminalPhoneNo': '123456789012',
      'WebApiPort': 828,
      'RemoteAddressIP': '127.0.0.1:11808'
    }, {
      'ChannelId': 'eadad23',
      'LastActiveTime': '2018-11-27 20:00:00',
      'StartTime': '2018-11-25 20:00:00',
      'TerminalPhoneNo': '123456789013',
      'WebApiPort': 828,
      'RemoteAddressIP': '127.0.0.1:11808'
    }])
  }
}
