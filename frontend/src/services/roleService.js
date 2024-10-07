import { axiosInstance } from "../axios/axiosInstance";

export async function getRoles() {
  try {
    return await axiosInstance.get("/roles");
  }
  catch (error) {
    return error;
  }
}

