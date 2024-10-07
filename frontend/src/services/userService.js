import { axiosInstance } from "../axios/axiosInstance";

export async function authenticateUser(authData) {
  try {
    return await axiosInstance.post("/user/auth", authData)
  }
  catch (error) {
    return error
  }
}

export async function registerUser(registrationData) {
  try {
    return await axiosInstance.post("/user/register", registrationData)
  }
  catch (error) {
    return error
  }
}

export async function confirmEmail(token) {
  try {
    return await axiosInstance.get(`/user/confirm_email/${token}`)
  }
  catch (error) {
    return error
  }
}

export async function resendConfirmationEmail(token) {
  try {
    return await axiosInstance.get(`/user/resend_confirmation_email/${token}`)
  }
  catch (error) {
    return error
  }
}

export async function changePassword(data, token) {
  try {
    return await axiosInstance.put(`/user/change_password`, data, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
  }
  catch (error) {
    return error
  }
}
