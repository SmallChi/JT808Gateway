export const setData = (data, code = 200) => {
  return {
    'Code': code,
    'Message': '',
    'Data': data
  }
}
