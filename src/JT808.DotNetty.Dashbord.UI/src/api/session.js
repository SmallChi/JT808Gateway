import axios from '@/libs/api.request'

export const RemoveByTerminalPhoneNo = (terminalPhoneN) => {
  return axios.request({
    url: 'Session/RemoveByTerminalPhoneNo/' + terminalPhoneN,
    method: 'get'
  })
}

export const RemoveByChannelId = (channelId) => {
  return axios.request({
    url: 'Session/RemoveByChannelId/' + channelId,
    method: 'get'
  })
}

export const GetAll = () => {
  return axios.request({
    url: 'Session/GetAll',
    method: 'get'
  })
}
