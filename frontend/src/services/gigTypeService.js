import { axiosInstance } from "../axios/axiosInstance"

export async function getGigTypes() {
  try {
    return await axiosInstance.get(`/gigTypes`)
  }
  catch (error) {
    return error
  }
}
