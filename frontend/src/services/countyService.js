import { axiosInstance } from "../axios/axiosInstance"

export async function getCounties() {
  try {
    return await axiosInstance.get(`/counties`)
  }
  catch (error) {
    return error
  }
}
