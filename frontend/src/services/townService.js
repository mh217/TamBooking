import { axiosInstance } from "../axios/axiosInstance"

export async function getTowns() {
  try {
    return await axiosInstance.get(`/towns`)
  }
  catch (error) {
    return error
  }
}
