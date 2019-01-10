import axios from '@/libs/api.request'

export const SendText = ({ terminalPhoneNo, text }) => {
  return axios.request({
    url: 'UnificationSend/SendText/' + terminalPhoneNo + '/' + text,
    method: 'get'
  })
}
