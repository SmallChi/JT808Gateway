import axios from '@/libs/api.request'

export const GetAtomicCounter = () => {
  return axios.request({
    url: 'AtomicCounter/GetAtomicCounter',
    method: 'get'
  })
}
