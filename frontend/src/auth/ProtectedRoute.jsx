import { useContext } from "react"
import { Navigate, Outlet } from "react-router-dom"
import { AuthContext } from "../App"
import { roles } from "../data/roles"

export default function ProtectedRoute({ permittedRoles }) {
  const { user } = useContext(AuthContext)
  return (
    <>
      {permittedRoles.includes(user.role) ?
        <Outlet /> : user.role !== roles.GUEST ? <Navigate to="/forbidden" /> : <Navigate to="/unauthorized" />}
    </>
  )
}
