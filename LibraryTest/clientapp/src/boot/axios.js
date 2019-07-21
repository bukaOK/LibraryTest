import axios from 'axios'

const apiConfig = process.env.NODE_ENV === 'development' || typeof window === 'undefined' ? {
  baseURL: 'http://localhost:5000/',
  withCredentials: true
} : {
  baseURL: '/',
}
export default async ({ Vue }) => {
  Vue.prototype.$api = axios.create(apiConfig)
}
