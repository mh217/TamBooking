import { useContext, useEffect } from "react"
import { Outlet, useNavigate } from "react-router-dom"
import { AuthContext } from "../App"
import { roles } from "../data/roles"

export default function LoggedInRedirect() {
  const { user } = useContext(AuthContext)
  const navigate = useNavigate()

  useEffect(() => {
    if (user.role === roles.BAND || user.role === roles.CLIENT) {
      navigate("/home")
    }
    else if(user.role === roles.ADMIN) {
      navigate("/reviewsDeletion")
    }
  }, [])

  return (
    <Outlet />
  )
}
