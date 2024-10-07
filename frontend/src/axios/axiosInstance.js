import axios from "axios"

const BASE_URL = "https://localhost:7175/api"

export const axiosInstance = axios.create(
  {
    baseURL: BASE_URL,
    headers: {
      "Content-Type": "application/json",
    },
  })
