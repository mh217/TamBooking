import { useContext, useEffect, useState } from "react"
import { AuthContext } from "../App"
import { Outlet } from "react-router-dom"
import { useCookies } from "react-cookie"
import { jwtDecode } from "jwt-decode"
import { Spinner } from "react-bootstrap"

export default function Validate() {
  const { setUser } = useContext(AuthContext)
  const [isLoading, setIsLoading] = useState(true)
  const [cookies] = useCookies(["token"])

  useEffect(() => {
    if (!isLoading) {
      return
    }
    if(cookies.token){
      const decodedToken = jwtDecode(cookies.token)
      setUser({ id: decodedToken.nameid, email: decodedToken.email, role: decodedToken.role, accessToken: cookies.token })
    }
    setIsLoading(false)
  }, [isLoading])

  return (
    <>
      {isLoading ? <Spinner variant="warning" /> : <Outlet />}
    </>
  )
}
