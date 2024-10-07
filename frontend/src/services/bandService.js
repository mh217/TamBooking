import { axiosInstance } from "../axios/axiosInstance"

export async function insertBand(bandData, token) {
  try {
    return await axiosInstance.post("/bands/add", bandData, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
  }
  catch (error) {
    return error
  }
}

export async function getBand(id) {
  try {
    return await axiosInstance.get("/bands", {
      params: {
        id: id
      }
    })
  }
  catch (error) {
    return error
  }
}