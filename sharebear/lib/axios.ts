import axios from 'axios';
const baseUrl = process.env.NEXT_PUBLIC_BACKEND_BASE_URL

const instance = axios.create({
    baseURL: baseUrl
})

export default instance;