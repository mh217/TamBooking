import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../App";
import { useCookies } from "react-cookie";
import { roles } from "../data/roles";

const useLogout = () => {
  const { setUser } = useContext(AuthContext)
  const navigate = useNavigate()
  const [_, __, removeCookie] = useCookies(["token"])

  const logout = (destinationPath) => {
    removeCookie("token")
    setUser({ id: null, email: null, role: roles.GUEST, accessToken: null })
    navigate(destinationPath)
  }
  return logout
}

export default useLogout
