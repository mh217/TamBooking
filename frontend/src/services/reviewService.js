import { axiosInstance } from "../axios/axiosInstance";

export async function getReviews(params) {
  try {
    return await axiosInstance.get("/reviews", {
      params: params
    })
  }
  catch (error) {
    return error
  }
}

export async function deleteReview(id, token) {
  try {
    return await axiosInstance.delete(`/reviews/delete/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
  }
  catch (error) {
    return error
  }
}

