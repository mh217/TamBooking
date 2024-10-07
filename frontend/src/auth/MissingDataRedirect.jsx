import { useContext, useEffect } from "react"
import { Outlet, useNavigate } from "react-router-dom"
import { AuthContext } from "../App"
import { roles } from "../data/roles"
import { getClient } from "../services/clientService"
import { HttpStatusCode } from "axios"
import { getBand } from "../services/bandService"

export default function MissingDataRedirect() {
  const { user } = useContext(AuthContext)
  const navigate = useNavigate()

  useEffect(() => {
    const validateData = async () => {
      if (user.role === roles.CLIENT) {
        const response = await getClient(user.id)
        if (response.status === HttpStatusCode.NotFound) {
          navigate("/fillClientData")
        }
      }
      else if (user.role === roles.BAND) {
        const response = await getBand(user.id)
        if (response.data?.length === 0) {
          navigate("/fillBandData")
        }
      }
    }
    validateData()
  }, [])

  return (
    <Outlet />
  )
}
