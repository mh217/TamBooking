import { axiosInstance } from "../axios/axiosInstance"

export async function insertClient(clientData, token) {
  try {
    return await axiosInstance.post("/clients/add", clientData, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
  }
  catch (error) {
    return error
  }
}

export async function getClient(id) {
  try {
    return await axiosInstance.get(`/clients/${id}`)
  }
  catch (error) {
    return error
  }
}
